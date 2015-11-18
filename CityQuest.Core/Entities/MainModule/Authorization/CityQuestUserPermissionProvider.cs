using Abp.Dependency;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization
{
    public class CityQuestUserPermissionProvider : ITransientDependency
    {
        protected IUserRepository UserRepository { get; set; }

        public CityQuestUserPermissionProvider(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task<string[]> GetUserPermissionsAsync(long userId)
        {
            User user = await UserRepository.GetAsync(userId);
            IList<string> permissionsFromUser = user.Permissions.Select(r => r.Name).ToList();
            IList<string> permisionsFromRole = user.Roles.SelectMany(r => r.Role.Permissions.Select(e => e.Name)).ToList();
            var allPermissions = permisionsFromRole.Union(permissionsFromUser).Distinct().ToList();
            var stringPermissions = allPermissions.Select(r => r.ToString()).ToArray();
            return stringPermissions;
        }
    }
}
