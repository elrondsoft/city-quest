using Abp.Authorization;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.MainModule.Users
{
    public class UserPolicy : CityQuestPolicyBase<User, long>, IUserPolicy
    {
        public UserPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }
    }
}