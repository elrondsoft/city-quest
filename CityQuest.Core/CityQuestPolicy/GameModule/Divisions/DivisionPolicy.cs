using Abp.Authorization;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Divisions
{
    public class DivisionPolicy : CityQuestPolicyBase<Division, long>, IDivisionPolicy
    {
        public DivisionPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, Division entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(Division entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
