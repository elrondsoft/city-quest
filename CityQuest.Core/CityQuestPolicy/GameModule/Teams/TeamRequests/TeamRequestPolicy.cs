using Abp.Authorization;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Teams.TeamRequests
{
    public class TeamRequestPolicy : CityQuestPolicyBase<TeamRequest, long>, ITeamRequestPolicy
    {
        protected IUserRepository UserRepository { get; set; }

        public TeamRequestPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanAnswerOnRequestFromTeam(long userId, TeamRequest entity)
        {
            return true;
        }

        public bool CanAnswerOnRequestFromTeam(TeamRequest entity)
        {
            return true;
        }

        public bool CanDenyRequestToPlayer(long userId, TeamRequest entity)
        {
            return true;
        }

        public bool CanDenyRequestToPlayer(TeamRequest entity)
        {
            return true;
        }
    }
}
