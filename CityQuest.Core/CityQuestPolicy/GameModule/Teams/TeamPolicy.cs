using Abp.Authorization;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Teams
{
    public class TeamPolicy : CityQuestPolicyBase<Team, long>, ITeamPolicy
    {
        public TeamPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }
    }
}
