using Abp.Dependency;
using CityQuest.Entities.MainModule.Authorization.RoleServices;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserServices
{
    public class UserManager : UserManager<User, long>, ITransientDependency
    {
        private readonly RoleManager _roleManager;

        public UserManager(UserStore userStore, RoleManager roleManager)
            : base(userStore)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            foreach (var role in await GetRolesAsync(userId))
            {
                if (await _roleManager.HasPermissionAsync(role, permissionName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
