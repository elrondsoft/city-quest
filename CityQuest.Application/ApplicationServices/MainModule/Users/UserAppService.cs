using Abp.Application.Services.Dto;
using Abp.UI;
using Castle.Core.Logging;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Mapping;
using CityQuest.Runtime.Sessions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users
{
    public class UserAppService : IUserAppService
    {
        #region Injected Dependencies

        private ICityQuestSession Session { get; set; }

        private IUserRepository UserRepository { get; set; }
        private IUserRoleRepository UserRoleRepository { get; set; }
        private IRoleRepository RoleRepository { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region ctors

        public UserAppService(
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository, 
            IRoleRepository roleRepository,
            ICityQuestSession session)
        {
            Session = session;

            UserRepository = userRepository;
            UserRoleRepository = userRoleRepository;
            RoleRepository = roleRepository;

            Logger = NullLogger.Instance;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllPagedResultOutput<UserDto, long> RetrieveAllPagedResult(RetrieveAllUsersPagedResultInput input)
        {
            UserRepository.Includes.Add(r => r.LastModifierUser);
            UserRepository.Includes.Add(r => r.CreatorUser);
            UserRepository.Includes.Add(r => r.Roles);

            IQueryable<User> usersQuery = UserRepository.GetAll()
                .WhereIf(input.OnlyWithDefaultRole != null, r => r.Roles.Any(e => e.Role.IsDefault))
                .WhereIf(!input.UserIds.IsNullOrEmpty(), r => input.UserIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .WhereIf(!String.IsNullOrEmpty(input.Surname), r => r.Surname.ToLower().Contains(input.Surname.ToLower()))
                .WhereIf(!String.IsNullOrEmpty(input.UserName), r => r.UserName.ToLower().Contains(input.UserName.ToLower()));

            int totalCount = usersQuery.Count();
            IReadOnlyList<UserDto> userDtos = usersQuery
                .OrderBy(r => r.UserName)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<User, UserDto>().ToList();

            UserRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<UserDto, long>()
            {
                Items = userDtos,
                TotalCount = totalCount
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllUsersLikeComboBoxesOutput RetrieveAllUsersLikeComboBoxes(RetrieveAllUsersLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> usersLikeComboBoxes = UserRepository.GetAll()
                .WhereIf(input.OnlyWithDefaultRole != null, r => r.Roles.Any(e => e.Role.IsDefault))
                .WhereIf(input.RoleId != null, r => r.Roles.Any(e => e.RoleId == input.RoleId))
                .WhereIf(!input.UserIds.IsNullOrEmpty(), r => input.UserIds.Contains(r.Id))
                .ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name))
                .ToList();

            return new RetrieveAllUsersLikeComboBoxesOutput()
            {
                Items = usersLikeComboBoxes
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<UserDto, long> RetrieveAll(RetrieveAllUsersInput input)
        {
            IList<User> userEntities = UserRepository.GetAll()
                .WhereIf(input.OnlyWithDefaultRole != null, r => r.Roles.Any(e => e.Role.IsDefault))
                .WhereIf(input.RoleId != null, r => r.Roles.Any(e => e.RoleId == input.RoleId))
                .WhereIf(!input.UserIds.IsNullOrEmpty(), r => input.UserIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .WhereIf(!String.IsNullOrEmpty(input.Surname), r => r.Surname.ToLower().Contains(input.Surname.ToLower()))
                .WhereIf(!String.IsNullOrEmpty(input.UserName), r => r.UserName.ToLower().Contains(input.UserName.ToLower()))
                .ToList();

            IList<UserDto> result = userEntities.MapIList<User, UserDto>();

            return new RetrieveAllOutput<UserDto, long>()
            {
                RetrievedEntities = result
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<UserDto, long> Retrieve(RetrieveUserInput input)
        {
            IList<User> userEntities = UserRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.UserName), r => r.Name.ToLower().Contains(input.UserName.ToLower()))
                .ToList();

            if (userEntities.Count != 1) 
            {
                throw new UserFriendlyException("Inaccessible action!", String.Format("Can not retrieve User with these filters."));            
            }

            UserDto userEntity = userEntities.Single().MapTo<UserDto>();

            return new RetrieveOutput<UserDto, long>()
            {
                RetrievedEntity = userEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<UserDto, long> RetrieveCurrentUserInfo()
        {
            return Retrieve(new RetrieveUserInput() { Id = Session.UserId.Value });
        }

        public CreateOutput<UserDto, long> Create(CreateInput<UserDto, long> input)
        {
            User existingUser = UserRepository.FirstOrDefault(r => r.UserName == input.Entity.UserName);
            if (existingUser != null)
            {
                throw new UserFriendlyException("Wrong username!", String.Format(
                    "There is already a user with username: \"{0}\"! Please, select another user name.", input.Entity.UserName));
            }
            User newUserEntity = new User() 
            {
                EmailAddress = input.Entity.EmailAddress,
                IsEmailConfirmed = true,
                Name = input.Entity.Name,
                Password = input.Entity.Password,
                PhoneNumber = input.Entity.PhoneNumber,
                Roles = new List<UserRole>(),
                Surname = input.Entity.Surname,
                UserName = input.Entity.UserName
            };
            newUserEntity.Password = new PasswordHasher().HashPassword(newUserEntity.Password);

            #region Creating UserRole

            Role defaultRole = RoleRepository.FirstOrDefault(r => r.IsDefault);
            if (defaultRole == null)
            {
                throw new UserFriendlyException("Error during the action!",
                    "Can not get default role! Please, contact your system administrator or system support.");
            }

            newUserEntity.Roles.Clear();
            newUserEntity.Roles.Add(new UserRole()
                {
                    Role = defaultRole,
                    RoleId = defaultRole.Id,
                    User = newUserEntity
                });

            #endregion

            UserDto newUserDto = (UserRepository.Insert(newUserEntity)).MapTo<UserDto>();

            return new CreateOutput<UserDto, long>()
            {
                CreatedEntity = newUserDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<UserDto, long> Update(UpdateInput<UserDto, long> input)
        {
            User userEntityForUpdate = UserRepository.Get(input.Entity.Id);

            if (userEntityForUpdate == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format("There are no User for update."));

            #region Updating fields for entity User

            UserRoleRepository.RemoveRange(userEntityForUpdate.Roles);
            userEntityForUpdate.Roles.Clear();

            userEntityForUpdate.Name = String.IsNullOrEmpty(input.Entity.Name) ? 
                userEntityForUpdate.Name : input.Entity.Name;
            userEntityForUpdate.Surname = String.IsNullOrEmpty(input.Entity.Surname) ? 
                userEntityForUpdate.Surname : input.Entity.Surname;
            if (!String.IsNullOrEmpty(input.Entity.EmailAddress) && userEntityForUpdate.EmailAddress != input.Entity.EmailAddress)
            {
                userEntityForUpdate.EmailAddress = input.Entity.EmailAddress;
                userEntityForUpdate.IsEmailConfirmed = true;
            }
            userEntityForUpdate.PhoneNumber = String.IsNullOrEmpty(input.Entity.PhoneNumber) ?
                userEntityForUpdate.PhoneNumber : input.Entity.PhoneNumber;

            IList<long> usingRoleIds = input.Entity.Roles.Select(r => r.Id).ToList();
            IList<Role> usingRoles = RoleRepository.GetAll().Where(r => usingRoleIds.Contains(r.Id)).ToList();

            foreach (var item in input.Entity.Roles)
            {
                Role currentRole = usingRoles.FirstOrDefault(r => r.Id == item.Id);
                userEntityForUpdate.Roles.Add(new UserRole()
                {
                    Role = currentRole,
                    RoleId = currentRole.Id,
                    User = userEntityForUpdate,
                    UserId = userEntityForUpdate.Id
                });
            }

            #endregion

            UserDto userEntityDto = (UserRepository.Update(userEntityForUpdate)).MapTo<UserDto>(); 

            return new UpdateOutput<UserDto, long>()
            {
                UpdatedEntity = userEntityDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public ChangePasswordOutput ChangePassword(ChangePasswordInput input)
        {
            User currentUser = UserRepository.FirstOrDefault(Session.UserId.Value);

            if (currentUser == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format("There are no User for update."));

            PasswordHasher passwordHasher = new PasswordHasher();

            if (input.CurrentPassword.IsNullOrEmpty() || 
                passwordHasher.VerifyHashedPassword(currentUser.Password, input.CurrentPassword) == PasswordVerificationResult.Failed)
            {
                throw new UserFriendlyException("Wrong password!", String.Format("Old password is not correct."));
            }

            if (input.NewPassword.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Wrong new password!", String.Format("New password is not correct."));
            }

            currentUser.Password = input.NewPassword;
            currentUser.Password = passwordHasher.HashPassword(currentUser.Password);

            return new ChangePasswordOutput();
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdatePublicUserFieldsOutput UpdatePublicUserFields(UpdatePublicUserFieldsInput input)
        {
            User userEntityForUpdate = UserRepository.Get(input.Id);

            if (userEntityForUpdate == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format("There are no User for update."));

            #region Updating public fields for entity User

            userEntityForUpdate.Name = String.IsNullOrEmpty(input.Name) ? userEntityForUpdate.Name : input.Name;
            userEntityForUpdate.Surname = String.IsNullOrEmpty(input.Surname) ? userEntityForUpdate.Surname : input.Surname;
            userEntityForUpdate.PhoneNumber = String.IsNullOrEmpty(input.PhoneNumber) ? userEntityForUpdate.PhoneNumber : input.PhoneNumber;

            if (!String.IsNullOrEmpty(input.EmailAddress) && userEntityForUpdate.EmailAddress != input.EmailAddress)
            {
                userEntityForUpdate.EmailAddress = input.EmailAddress;
                userEntityForUpdate.IsEmailConfirmed = true;
            }

            #endregion

            UserDto userEntityDto = (UserRepository.Update(userEntityForUpdate)).MapTo<UserDto>(); 

            return new UpdatePublicUserFieldsOutput() 
                {
                    User = userEntityDto
                };
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            try
            {
                UserRepository.Delete(input.EntityId);
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format(
                    "Error when deleting user with id = {0}! (exception message: \"{3}\")", input.EntityId, ex.Message));
                throw new UserFriendlyException("Error during the action!", 
                    "Can not delete this user. Please, contact your system administrator or system support.");
            }

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }
    }
}
