using Abp.Authorization;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Conditions
{
    public class ConditionPolicy : CityQuestPolicyBase<Condition, long>, IConditionPolicy
    {
        public ConditionPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }
    }
}
