using Abp;
using Abp.Authorization;
using Abp.Dependency;
using CityQuest.Entities.MainModule.Roles;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.RoleServices
{
    public class RoleManager : RoleManager<Role, long>, ITransientDependency
    {
        private IRolePermissionStore RolePermissionStore
        {
            get
            {
                if (!(Store is IRolePermissionStore))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }

                return Store as IRolePermissionStore;
            }
        }

        private readonly IPermissionManager _permissionManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="store">Role store</param>
        /// <param name="permissionManager">Permission manager</param>
        public RoleManager(RoleStore store, IPermissionManager permissionManager)
            : base(store)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await HasPermissionAsync(await GetRole(roleName), GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(int roleId, string permissionName)
        {
            return await HasPermissionAsync(await GetRole(roleId), GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="role">The rolepermission</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public async Task<bool> HasPermissionAsync(Role role, Permission permission)
        {
            return permission.IsGrantedByDefault
                ? !(await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, false)))
                : (await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, true)));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRole(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRole(roleName));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(Role role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await HasPermissionAsync(role, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRole(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Grants a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task GrantPermissionAsync(Role role, Permission permission)
        {
            if (await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task ProhibitPermissionAsync(Role role, Permission permission)
        {
            if (!await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits all permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ProhibitAllPermissionsAsync(Role role)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a role.
        /// It removes all permission settings for the role.
        /// Role will have permissions those have <see cref="Permission.IsGrantedByDefault"/> set to true.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ResetAllPermissionsAsync(Role role)
        {
            await RolePermissionStore.RemoveAllPermissionSettingsAsync(role);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns></returns>
        public override Task<IdentityResult> DeleteAsync(Role role)
        {
            if (role.IsStatic)
            {
                throw new AbpException("Can not delete a static role: " + role);
            }

            return base.DeleteAsync(role);
        }

        private Permission GetPermission(string permissionName)
        {
            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + permissionName);
            }

            return permission;
        }

        private async Task<Role> GetRole(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id = " + roleId);
            }

            return role;
        }

        private async Task<Role> GetRole(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name = " + roleName);
            }

            return role;
        }
    }
}
