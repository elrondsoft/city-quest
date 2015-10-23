using Abp.Dependency;
using Abp.Runtime.Session;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Roles;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.RoleServices
{
    public class RoleStore : IQueryableRoleStore<Role, long>, IRolePermissionStore, ITransientDependency
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionSettingRepository _rolePermissionSettingRepository;
        private readonly IAbpSession _session;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RoleStore(
            IRoleRepository roleRepository,
            IRolePermissionSettingRepository rolePermissionSettingRepository,
            IAbpSession session)
        {
            _roleRepository = roleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
            _session = session;
        }

        public IQueryable<Role> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public async Task CreateAsync(Role role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public async Task UpdateAsync(Role role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(Role role)
        {
            await _roleRepository.DeleteAsync(role.Id);
        }

        public async Task<Role> FindByIdAsync(long roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(role => role.Name == roleName);
        }

        public async Task AddPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new RolePermissionSetting
                {
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        public async Task RemovePermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(Role role)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == role.Id))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        public async Task<bool> HasPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == role.Id &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        public async Task RemoveAllPermissionSettingsAsync(Role role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }

        public void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
