using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests
{
    public interface ITeamRequestAppService : IApplicationService
    {
        RetrieveAllOutput<TeamRequestDto, long> RetrieveAll(RetrieveAllTeamRequestsInput input);

        RetrieveOutput<TeamRequestDto, long> Retrieve(RetrieveTeamRequestInput input);

        CreateOutput<TeamRequestDto, long> Create(CreateTeamRequestInput input);

        UpdateOutput<TeamRequestDto, long> Update(UpdateInput<TeamRequestDto, long> input);

        DeleteOutput<long> Delete(DeleteInput<long> input);

        AnswerOnRequestOutput AnswerOnRequest(AnswerOnRequestInput input);

        DenyRequestOutput DenyRequest(DenyRequestInput input);

        LeaveCurrentTeamOutput LeaveCurrentTeam(LeaveCurrentTeamInput input);
    }
}
