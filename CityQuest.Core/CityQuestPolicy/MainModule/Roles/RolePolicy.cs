using Abp.Authorization;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.MainModule.Roles
{
    public class RolePolicy : CityQuestPolicyBase<Role, long>, IRolePolicy
    {
        public RolePolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }
    }
}
