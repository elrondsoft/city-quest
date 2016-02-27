using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestPolicy.GameModule.Teams.TeamRequests;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests
{
    [Abp.Authorization.AbpAuthorize]
    public class TeamRequestAppService : ITeamRequestAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private ICityQuestSession Session { get; set; }
        private ICityQuestRepositoryBase<TeamRequest, long> TeamRequestRepository { get; set; }
        private ITeamRequestPolicy TeamRequestPolicy { get; set; }
        private IPlayerCareerRepository PlayerCareerRepository { get; set; }
        private ITeamRepository TeamRepository { get; set; }

        #endregion

        #region Ctors

        public TeamRequestAppService(IUnitOfWorkManager uowManager,
            ICityQuestSession session,
            ICityQuestRepositoryBase<TeamRequest, long> teamRequestRepository,
            ITeamRequestPolicy teamRequestPolicy,
            IPlayerCareerRepository playerCareerRepository,
            ITeamRepository teamRepository)
        {
            UowManager = uowManager;
            Session = session;
            TeamRequestRepository = teamRequestRepository;
            TeamRequestPolicy = teamRequestPolicy;
            PlayerCareerRepository = playerCareerRepository;
            TeamRepository = teamRepository;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<TeamRequestDto, long> RetrieveAll(RetrieveAllTeamRequestsInput input)
        {
            TeamRequestRepository.Includes.Add(r => r.LastModifierUser);
            TeamRequestRepository.Includes.Add(r => r.CreatorUser);
            TeamRequestRepository.Includes.Add(r => r.InvitedUser);
            TeamRequestRepository.Includes.Add(r => r.Team);
            TeamRequestRepository.Includes.Add(r => r.Team.PlayerCareers);

            IQueryable<TeamRequest> teamRequestsQuery = TeamRequestPolicy.CanRetrieveManyEntities(
                TeamRequestRepository.GetAll()
                    .Where(r => r.DeclinedByInviter != true)
                    .Where(r => r.InvitedUserResponse == null)
                    .WhereIf(input.TeamId != null, r => r.TeamId == input.TeamId)
                    .WhereIf(input.CaptainId != null, r => r.Team.PlayerCareers.Any(e => 
                        e.CareerDateEnd == null && e.UserId == input.CaptainId && e.IsCaptain && e.IsActive))
                    .WhereIf(input.UserId != null, r => r.InvitedUserId == input.UserId))
                .OrderBy(r => r.CreationTime);

            IList<TeamRequestDto> result = teamRequestsQuery.ToList().MapIList<TeamRequest, TeamRequestDto>();

            TeamRequestRepository.Includes.Clear();

            return new RetrieveAllOutput<TeamRequestDto, long>()
                {
                    RetrievedEntities = result
                };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<TeamRequestDto, long> Retrieve(RetrieveTeamRequestInput input)
        {
            TeamRequestRepository.Includes.Add(r => r.LastModifierUser);
            TeamRequestRepository.Includes.Add(r => r.CreatorUser);
            TeamRequestRepository.Includes.Add(r => r.InvitedUser);
            TeamRequestRepository.Includes.Add(r => r.Team);

            IList<TeamRequest> teamRequestEntities = TeamRequestRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(input.TeamId != null, r => r.TeamId == input.TeamId)
                .WhereIf(input.UserId != null, r => r.InvitedUserId == input.UserId)
                .ToList();

            if (teamRequestEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"TeamRequest\"");

            if (!TeamRequestPolicy.CanRetrieveEntity(teamRequestEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"TeamRequest\"");

            TeamRequestDto teamRequestEntity = teamRequestEntities.Single().MapTo<TeamRequestDto>();

            TeamRequestRepository.Includes.Clear();

            return new RetrieveOutput<TeamRequestDto, long>()
            {
                RetrievedEntity = teamRequestEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<TeamRequestDto, long> Create(CreateTeamRequestInput input)
        {
            TeamRequest newTeamRequestEntity;
            try
            {
                long teamId = PlayerCareerRepository
                    .Single(r => r.UserId == (Session.UserId ?? 0) && r.CareerDateEnd == null).TeamId;

                newTeamRequestEntity = new TeamRequest()
                    {
                        InvitedUserId = input.InvitedUserId,
                        TeamId = teamId
                    };

                if (!TeamRequestPolicy.CanCreateEntity(newTeamRequestEntity))
                    throw new Exception();
            }
            catch
            {
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"TeamRequest\"");
            }

            TeamRequestRepository.Includes.Add(r => r.LastModifierUser);
            TeamRequestRepository.Includes.Add(r => r.CreatorUser);
            TeamRequestRepository.Includes.Add(r => r.InvitedUser);
            TeamRequestRepository.Includes.Add(r => r.Team);

            TeamRequestDto newTeamRequestDto = (TeamRequestRepository.Insert(newTeamRequestEntity)).MapTo<TeamRequestDto>();

            TeamRequestRepository.Includes.Clear();

            return new CreateOutput<TeamRequestDto, long>()
                {
                    CreatedEntity = newTeamRequestDto
                };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<TeamRequestDto, long> Update(UpdateInput<TeamRequestDto, long> input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            TeamRequest teamRequestEntityForDelete = TeamRequestRepository.Get(input.EntityId);

            if (teamRequestEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"TeamRequest\"");

            if (!TeamRequestPolicy.CanDeleteEntity(teamRequestEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"TeamRequest\"");

            TeamRequestRepository.Delete(teamRequestEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public AnswerOnRequestOutput AnswerOnRequest(AnswerOnRequestInput input)
        {
            TeamRequest teamRequestEntity = TeamRequestRepository.Get(input.TeamRequestId);

            if (teamRequestEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"TeamRequest\"");

            if (!TeamRequestPolicy.CanAnswerOnRequestFromTeam(teamRequestEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"TeamRequest\"");

            DateTime currentDT = DateTime.Now;

            teamRequestEntity.InvitedUserResponse = input.Answer;
            teamRequestEntity.InvitedUserResponseDateTime = currentDT;
            TeamRequestRepository.Update(teamRequestEntity);

            #region Creating new career if joined new team

            if (teamRequestEntity.InvitedUserResponse == true)
            {
                CreateNewPlayerCareer(teamRequestEntity.InvitedUserId, teamRequestEntity.TeamId, currentDT);
            }

            #endregion

            return new AnswerOnRequestOutput()
                {
                    TeamRequest = teamRequestEntity.MapTo<TeamRequestDto>(),
                };
        }

        [Abp.Authorization.AbpAuthorize]
        public DenyRequestOutput DenyRequest(DenyRequestInput input)
        {
            TeamRequest teamRequestEntity = TeamRequestRepository.Get(input.TeamRequestId);

            if (teamRequestEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"TeamRequest\"");

            if (!TeamRequestPolicy.CanDenyRequestToPlayer(teamRequestEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"TeamRequest\"");

            DateTime currentDT = DateTime.Now;

            teamRequestEntity.DeclinedByInviter = true;
            teamRequestEntity.DeclinedByInviterDateTime = currentDT;
            TeamRequestRepository.Update(teamRequestEntity);

            return new DenyRequestOutput() { };
        }

        [Abp.Authorization.AbpAuthorize]
        public LeaveCurrentTeamOutput LeaveCurrentTeam(LeaveCurrentTeamInput input)
        {
            long newTeamId = TeamRepository.Single(r => r.IsActive && r.IsDefault == true).Id;

            DateTime currentDT = DateTime.Now;

            CreateNewPlayerCareer(Session.UserId ?? 0, newTeamId, currentDT);

            return new LeaveCurrentTeamOutput() { };
        }

        #region Helpers 

        private void CreateNewPlayerCareer(long userId, long newTeamId, DateTime currentDT)
        {
            PlayerCareerRepository.Includes.Add(r => r.Team.PlayerCareers);

            var carrersForUpdating = PlayerCareerRepository.GetAll()
                .Where(r => r.UserId == userId && r.CareerDateEnd == null).ToList();

            foreach (var item in carrersForUpdating)
            {
                if (item.IsCaptain == true)
                {
#warning Using to prevent deactivating default team
                    if (item.Team.IsDefault == true)
                    {
                        throw new Abp.UI.UserFriendlyException("Unallowed action!",
                            "Captain of default team can not leave it. This action will be available after transfering captaincy to another player.");
                    }

                    var newCaptain = item.Team.CurrentPlayers.FirstOrDefault(r => !r.IsCaptain);
                    if (newCaptain == null)
                    {
                        //deactivate team
                        item.Team.IsActive = false;
                        TeamRepository.Update(item.Team);
                    }
                    else
                    {
                        newCaptain.IsCaptain = true;
                    }
                }
                item.CareerDateEnd = currentDT;
                item.IsCaptain = false;
                PlayerCareerRepository.Update(item);
            }

            PlayerCareer newPlayerCareer = new PlayerCareer()
            {
                CareerDateEnd = null,
                CareerDateStart = currentDT,
                IsCaptain = false,
                TeamId = newTeamId,
                UserId = userId,
                IsActive = true,
            };
            PlayerCareerRepository.Insert(newPlayerCareer);

            PlayerCareerRepository.Includes.Clear();
        }

        #endregion
    }
}
