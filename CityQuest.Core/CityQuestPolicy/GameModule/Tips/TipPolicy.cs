using Abp.Authorization;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Tips
{
    public class TipPolicy : CityQuestPolicyBase<Tip, long>, ITipPolicy
    {
        public TipPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, Tip entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(Tip entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
