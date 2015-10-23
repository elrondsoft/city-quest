using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using CityQuest.Entities.MainModule.Authorization.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        private readonly UserManager _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PermissionChecker(UserManager userManager)
        {
            _userManager = userManager;
            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && 
                await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }

        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }
    }


}