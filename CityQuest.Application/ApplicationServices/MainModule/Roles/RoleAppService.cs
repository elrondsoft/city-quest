using Abp.Application.Services.Dto;
using Abp.UI;
using Castle.Core.Logging;
using CityQuest.ApplicationServices.MainModule.Permissions.Dto;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Roles
{
    public class RoleAppService : IRoleAppService
    {
        #region Injected Dependencies

        private IRoleRepository RoleRepository { get; set; }
        private IRolePermissionSettingRepository RolePermissionSettingRepository { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region ctors

        public RoleAppService(IRoleRepository roleRepository, IRolePermissionSettingRepository rolePermissionSettingRepository)
        {
            RoleRepository = roleRepository;
            RolePermissionSettingRepository = rolePermissionSettingRepository;

            Logger = NullLogger.Instance;
        }

        #endregion

        public RetrieveAllPagedResultOutput<RoleDto, long> RetrieveAllPagedResult(RetrieveAllRolesPagedResultInput input)
        {
            RoleRepository.Includes.Add(r => r.LastModifierUser);
            RoleRepository.Includes.Add(r => r.CreatorUser);
            RoleRepository.Includes.Add(r => r.Permissions);

            IQueryable<Role> rolesQuery = RoleRepository.GetAll()
                .WhereIf(!input.RoleIds.IsNullOrEmpty(), r => input.RoleIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()));

            int totalCount = rolesQuery.Count();
            IReadOnlyList<RoleDto> roleDtos = rolesQuery
                .OrderByDescending(r => r.IsStatic).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Role, RoleDto>().ToList();

            RoleRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<RoleDto, long>()
            {
                Items = roleDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllRolesLikeComboBoxesOutput RetrieveAllRolesLikeComboBoxes(RetrieveAllRolesLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> rolesLikeComboBoxes = RoleRepository.GetAll().ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllRolesLikeComboBoxesOutput()
            {
                Items = rolesLikeComboBoxes
            };
        }

        public RetrieveAllOutput<RoleDto, long> RetrieveAll(RetrieveAllRoleInput input)
        {
            IList<Role> roleEntities = RoleRepository.GetAll()
                .WhereIf(!input.RoleIds.IsNullOrEmpty(), r => input.RoleIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            IList<RoleDto> result = roleEntities.MapIList<Role, RoleDto>();

            return new RetrieveAllOutput<RoleDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveOutput<RoleDto, long> Retrieve(RetrieveRoleInput input)
        {
            IList<Role> roleEntities = RoleRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (roleEntities.Count != 1) 
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format("Can not retrieve Role with these filters."));            
            }

            RoleDto roleEntity = roleEntities.Single().MapTo<RoleDto>();

            return new RetrieveOutput<RoleDto, long>()
            {
                RetrievedEntity = roleEntity
            };
        }

        public CreateOutput<RoleDto, long> Create(CreateInput<RoleDto, long> input)
        {
            Role newRoleEntity = new Role() 
            { 
                Name = input.Entity.Name,
                DisplayName = input.Entity.DisplayName,
                IsStatic = false,
                IsDefault = false,
                Permissions = new List<RolePermissionSetting>()
            };

            #region Creating role properties

            foreach (var item in input.Entity.Permissions)
            {
                newRoleEntity.Permissions.Add(new RolePermissionSetting()
                {
                    Name = item.DisplayText,
                    Role = newRoleEntity
                });
            }

            #endregion

            RoleDto newRoleDto = (RoleRepository.Insert(newRoleEntity)).MapTo<RoleDto>();

            return new CreateOutput<RoleDto, long>()
            {
                CreatedEntity = newRoleDto
            };
        }

        public UpdateOutput<RoleDto, long> Update(UpdateInput<RoleDto, long> input)
        {
            Role roleEntityForUpdate = RoleRepository.Get(input.Entity.Id);

            if (roleEntityForUpdate == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "There are no Role with Id = {0}. Can not update it.", input.Entity.Id));

            #region Updating Role properties

            RolePermissionSettingRepository.RemoveRange(roleEntityForUpdate.Permissions);

            roleEntityForUpdate.Permissions.Clear();
            roleEntityForUpdate.Name = String.IsNullOrEmpty(input.Entity.Name) ? roleEntityForUpdate.Name : input.Entity.Name;
            roleEntityForUpdate.Name = String.IsNullOrEmpty(input.Entity.DisplayName) ? 
                roleEntityForUpdate.DisplayName : input.Entity.DisplayName;
            foreach (var item in input.Entity.Permissions)
            {
                roleEntityForUpdate.Permissions.Add(new RolePermissionSetting() 
                {
                    Name = item.DisplayText,
                    Role = roleEntityForUpdate,
                    RoleId = roleEntityForUpdate.Id 
                });
            }

            #endregion

            RoleDto newRoleDto = (RoleRepository.Update(roleEntityForUpdate)).MapTo<RoleDto>();

            return new UpdateOutput<RoleDto, long>()
            {
                UpdatedEntity = newRoleDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Role roleEntityForDelete = RoleRepository.Get(input.EntityId);

            if (roleEntityForDelete == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format(
                    "There are no Role with Id = {0}. Can not delete it.", input.EntityId));

            if (roleEntityForDelete.IsStatic == true || roleEntityForDelete.IsDefault == true)
                throw new UserFriendlyException("Inaccessible action!", "Can not delete this Role.");

            try
            {
                RoleRepository.Delete(roleEntityForDelete);
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format(
                    "Error when deleting role with id = {0}, name = {1}, IsStatic = {2}! (exception message: \"{3}\")", 
                    roleEntityForDelete.Id, roleEntityForDelete.Name, roleEntityForDelete.IsStatic, ex.Message));
                throw new UserFriendlyException("Error during the action!", 
                    "Can not delete this role. Please, contact your system administrator or system support.");
            }

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }
    }
}
