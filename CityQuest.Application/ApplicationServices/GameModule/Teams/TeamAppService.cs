using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityQuest.Mapping;
using Abp.Application.Services.Dto;
using Abp.UI;
using CityQuest.CityQuestPolicy.GameModule.Teams;

namespace CityQuest.ApplicationServices.GameModule.Teams
{
    public class TeamAppService : ITeamAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private ITeamRepository TeamRepository { get; set; }
        private ITeamPolicy TeamPolicy { get; set; }

        #endregion

        public TeamAppService(IUnitOfWorkManager uowManager, ITeamRepository teamRepository, ITeamPolicy teamPolicy)
        {
            UowManager = uowManager;
            TeamRepository = teamRepository;
            TeamPolicy = teamPolicy;
        }

        public RetrieveAllPagedResultOutput<TeamDto, long> RetrieveAllPagedResult(RetrieveAllTeamsPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            TeamRepository.Includes.Add(r => r.LastModifierUser);
            TeamRepository.Includes.Add(r => r.CreatorUser);
            TeamRepository.Includes.Add(r => r.Division);

            IQueryable<Team> teamsQuery = TeamPolicy.CanRetrieveManyEntities( 
                TeamRepository.GetAll()
                .WhereIf(!input.TeamIds.IsNullOrEmpty(), r => input.TeamIds.Contains(r.Id))
                .WhereIf(input.DivisionId != null, r => r.DivisionId == input.DivisionId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = teamsQuery.Count();
            IReadOnlyList<TeamDto> teamDtos = teamsQuery
                .OrderByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Team, TeamDto>().ToList();

            TeamRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<TeamDto, long>()
            {
                Items = teamDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllTeamsLikeComboBoxesOutput RetrieveAllTeamsLikeComboBoxes(RetrieveAllTeamsLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> teamsLikeComboBoxes = TeamPolicy.CanRetrieveManyEntities(
                TeamRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllTeamsLikeComboBoxesOutput()
            {
                Items = teamsLikeComboBoxes
            };
        }

        public RetrieveAllOutput<TeamDto, long> RetrieveAll(RetrieveAllTeamsInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            TeamRepository.Includes.Add(r => r.LastModifierUser);
            TeamRepository.Includes.Add(r => r.CreatorUser);
            TeamRepository.Includes.Add(r => r.Division);

            IList<Team> teamEntities = TeamPolicy.CanRetrieveManyEntities( 
                TeamRepository.GetAll()
                .WhereIf(!input.TeamIds.IsNullOrEmpty(), r => input.TeamIds.Contains(r.Id))
                .WhereIf(input.DivisionId != null, r => r.DivisionId == input.DivisionId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<TeamDto> result = teamEntities.MapIList<Team, TeamDto>();

            TeamRepository.Includes.Clear();

            return new RetrieveAllOutput<TeamDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveOutput<TeamDto, long> Retrieve(RetrieveTeamInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IList<Team> teamEntities = TeamRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (teamEntities.Count != 1)
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "Can not retrieve Team with these filters."));
            }

            if (!TeamPolicy.CanRetrieveEntity(teamEntities.Single()))
            {
                throw new UserFriendlyException("Access denied!", String.Format(
                    "You have not permissions to retrieve this Team's entity."));
            }

            TeamDto teamEntity = teamEntities.Single().MapTo<TeamDto>();

            return new RetrieveOutput<TeamDto, long>()
            {
                RetrievedEntity = teamEntity
            };
        }

        public CreateOutput<TeamDto, long> Create(CreateTeamInput input)
        {
            Team newTeamEntity = input.Entity.MapTo<Team>();

            if (!TeamPolicy.CanCreateEntity(newTeamEntity))
            {
                throw new UserFriendlyException("Access denied!", String.Format(
                    "You have not permissions to create this Team's entity."));
            }

            newTeamEntity.IsActive = true;

            TeamRepository.Includes.Add(r => r.Captain);
            TeamRepository.Includes.Add(r => r.LastModifierUser);
            TeamRepository.Includes.Add(r => r.CreatorUser);
            TeamRepository.Includes.Add(r => r.Players);

            TeamDto newTeamDto = (TeamRepository.Insert(newTeamEntity)).MapTo<TeamDto>();

            TeamRepository.Includes.Clear();

            return new CreateOutput<TeamDto, long>()
            {
                CreatedEntity = newTeamDto
            };
        }

        public UpdateOutput<TeamDto, long> Update(UpdateTeamInput input)
        {
            Team newTeamEntity = input.Entity.MapTo<Team>();

            if (newTeamEntity == null)
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "There is not valid Team entity. Can not update to it."));
            }

            if (!TeamPolicy.CanUpdateEntity(newTeamEntity))
            {
                throw new UserFriendlyException(String.Format(
                    "You have not permissions to update this Team's entity."));
            }

            TeamRepository.Includes.Add(r=>r.Captain);
            TeamRepository.Includes.Add(r => r.LastModifierUser);
            TeamRepository.Includes.Add(r => r.CreatorUser);
            TeamRepository.Includes.Add(r => r.Players);

            TeamRepository.Update(newTeamEntity);
            TeamDto newTeamDto = (TeamRepository.Get(newTeamEntity.Id)).MapTo<TeamDto>();

            TeamRepository.Includes.Clear();

            return new UpdateOutput<TeamDto, long>()
            {
                UpdatedEntity = newTeamDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Team teamEntityForDelete = TeamRepository.Get(input.EntityId);

            if (teamEntityForDelete == null)
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "There are no Team with Id = {0}. Can not delete it.", input.EntityId));
            }

            if (!TeamPolicy.CanDeleteEntity(teamEntityForDelete))
            {
                throw new UserFriendlyException(String.Format(
                    "You have not permissions to delete this Team's entity."));
            }

            TeamRepository.Delete(teamEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        public ChangeActivityOutput<TeamDto, long> ChangeActivity(ChangeActivityInput input)
        {
            TeamRepository.Includes.Add(r => r.Captain);
            TeamRepository.Includes.Add(r => r.LastModifierUser);
            TeamRepository.Includes.Add(r => r.CreatorUser);
            TeamRepository.Includes.Add(r => r.Players);

            Team teamEntity = TeamRepository.Get(input.EntityId);

            if (teamEntity == null)
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "There are no Team with Id = {0}. Can not change it's activity.", input.EntityId));
            }

            if (!TeamPolicy.CanChangeActivityForEntity(teamEntity))
            {
                throw new UserFriendlyException(String.Format(
                    "You have not permissions to change activity of this Team's entity."));
            }

            teamEntity.IsActive = input.IsActive == null ? !teamEntity.IsActive : (bool)input.IsActive;
            TeamDto newTeamDto = teamEntity.MapTo<TeamDto>();

            TeamRepository.Includes.Clear();

            TeamRepository.Update(teamEntity);

            return new ChangeActivityOutput<TeamDto, long>()
            {
                Entity = newTeamDto
            };
        }
    }
}
