using CityQuest.Entities.GameModule.Teams.TeamRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Teams.TeamRequests
{
    public interface ITeamRequestPolicy : ICityQuestPolicyBase<TeamRequest, long>
    {
        bool CanAnswerOnRequestFromTeam(long userId, TeamRequest entity);
        bool CanAnswerOnRequestFromTeam(TeamRequest entity);

        bool CanDenyRequestToPlayer(long userId, TeamRequest entity);
        bool CanDenyRequestToPlayer(TeamRequest entity);
    }
}
