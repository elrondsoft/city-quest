using CityQuest.Entities.MainModule.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.RoleServices
{
    interface IRolePermissionStore
    {
        /// <summary>
        /// Adds a permission grant setting to a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(Role role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a role.
        /// </summary>
        /// <param name="role">Role </param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(Role role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(Role role);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(Role role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// </summary>
        /// <param name="role">Role</param>
        Task RemoveAllPermissionSettingsAsync(Role role);
    }
}
