using Abp.Authorization;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.ConditionTypes
{
    public class ConditionTypePolicy : CityQuestPolicyBase<ConditionType, long>, IConditionTypePolicy
    {
        public ConditionTypePolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, ConditionType entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(ConditionType entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
