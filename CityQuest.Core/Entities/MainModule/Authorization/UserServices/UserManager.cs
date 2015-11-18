using Abp.Dependency;
using Abp.Domain.Uow;
using CityQuest.Entities.MainModule.Authorization.RoleServices;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserServices
{
    public class UserManager : UserManager<User, long>, ITransientDependency
    {
        private readonly RoleManager _roleManager = null;
        private IUnitOfWorkManager _uowManager = null;
        private CityQuestUserPermissionProvider _userPermissionProvider = null;

        public UserManager(
            UserStore userStore, 
            RoleManager roleManager, 
            IUnitOfWorkManager uowManager, 
            CityQuestUserPermissionProvider userPermissionProvider)
            : base(userStore)
        {
            _uowManager = uowManager;
            _roleManager = roleManager;
            _userPermissionProvider = userPermissionProvider;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            string[] permissions;
            using (var uow = _uowManager.Begin())
            {
                _uowManager.Current.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                permissions = await _userPermissionProvider.GetUserPermissionsAsync(user.Id);
                _uowManager.Current.EnableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
            }
            identity.AddClaim(new Claim(CityQuestConsts.PermissionKey, Newtonsoft.Json.JsonConvert.SerializeObject(permissions)));
            return identity;
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
