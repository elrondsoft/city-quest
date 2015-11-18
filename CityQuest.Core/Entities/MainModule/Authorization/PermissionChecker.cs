using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using CityQuest.Entities.MainModule.Authorization.UserServices;
using CityQuest.Runtime.Sessions;
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
        public ICityQuestSession Session { get; set; }

        public PermissionChecker(UserManager userManager, ICityQuestSession session)
        {
            _userManager = userManager;
            Session = session;
            Logger = NullLogger.Instance;
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            return Session.UserId.HasValue &&
                await IsGrantedAsync(Session.UserId.Value, permissionName);
        }

        //public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        //{
        //    return await UserManager.IsGrantedAsync(userId, permissionName);
        //}

        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            bool result = ((!Session.Permissions.IsNullOrEmpty()) && Session.Permissions.Contains(permissionName)) ? true : false;
            return result;
        }
    }
}