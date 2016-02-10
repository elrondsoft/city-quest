using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestPolicy.GameModule.Teams.TeamRequests;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
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
        private ICityQuestRepositoryBase<TeamRequest, long> TeamRequestRepository { get; set; }
        private ITeamRequestPolicy TeamRequestPolicy { get; set; }

        #endregion

        #region Ctors

        public TeamRequestAppService(IUnitOfWorkManager uowManager,
            ICityQuestRepositoryBase<TeamRequest, long> teamRequestRepository,
            ITeamRequestPolicy teamRequestPolicy)
        {
            UowManager = uowManager;
            TeamRequestRepository = teamRequestRepository;
            TeamRequestPolicy = teamRequestPolicy;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<TeamRequestDto, long> RetrieveAll(RetrieveAllTeamRequestsInput input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<TeamRequestDto, long> Retrieve(RetrieveTeamRequestInput input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<TeamRequestDto, long> Create(CreateInput<TeamRequestDto, long> input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<TeamRequestDto, long> Update(UpdateInput<TeamRequestDto, long> input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public AnswerOnRequestOutput AnswerOnRequest(AnswerOnRequestInput input)
        {
            throw new NotImplementedException();
        }

        [Abp.Authorization.AbpAuthorize]
        public DenyRequestOutput DenyRequest(DenyRequestInput input)
        {
            throw new NotImplementedException();
        }
    }
}
