using Abp.Application.Services.Dto;
using Abp.UI;
using Castle.Core.Logging;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
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

        protected ILogger Logger { get; set; }

        #endregion

        #region ctors

        public UserAppService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, ICityQuestSession session)
        {
            Session = session;

            UserRepository = userRepository;
            UserRoleRepository = userRoleRepository;

            Logger = NullLogger.Instance;
        }

        #endregion

        public RetrieveAllPagedResultOutput<UserDto, long> RetrieveAllPagedResult(RetrieveAllUsersPagedResultInput input)
        {
            UserRepository.Includes.Add(r => r.LastModifierUser);
            UserRepository.Includes.Add(r => r.CreatorUser);
            UserRepository.Includes.Add(r => r.Roles);

            IQueryable<User> usersQuery = UserRepository.GetAll()
                .WhereIf(input.OnlyWithDefaultRole != null, r => r.Roles.Any(e => e.Role.IsDefault))
                .WhereIf(input.RoleId != null, r => r.Roles.Any(e => e.RoleId == input.RoleId))
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
            User newUserEntity = input.Entity.MapTo<User>();
            newUserEntity.Password = new PasswordHasher().HashPassword(newUserEntity.Password);

            UserDto newUserDto = (UserRepository.Insert(newUserEntity)).MapTo<UserDto>();

            return new CreateOutput<UserDto, long>()
            {
                CreatedEntity = newUserDto
            };
        }

        public UpdateOutput<UserDto, long> Update(UpdateInput<UserDto, long> input)
        {
            User userEntityForUpdate = UserRepository.Get(input.Entity.Id);

            if (userEntityForUpdate == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format("There are no User for update."));

            UserRoleRepository.RemoveRange(userEntityForUpdate.Roles);
            userEntityForUpdate.Roles.Clear();
            foreach (var item in input.Entity.Roles)
            {
                userEntityForUpdate.Roles.Add(new UserRole()
                {
                    RoleId = item.Id,
                    User = userEntityForUpdate,
                    UserId = userEntityForUpdate.Id
                });
            }

            UserDto userEntityDto = (UserRepository.Update(userEntityForUpdate)).MapTo<UserDto>(); ;

            return new UpdateOutput<UserDto, long>()
            {
                UpdatedEntity = userEntityDto
            };
        }

        public ChangePasswordOutput ChangePassword(ChangePasswordInput input)
        {
            User currentUser = UserRepository.FirstOrDefault(Session.UserId.Value);

            if (currentUser == null)
                throw new UserFriendlyException("Inaccessible action!", String.Format("There are no User for update."));

            if (currentUser.Password != input.CurrentPassword)
            {
                throw new UserFriendlyException("Wrong password!", String.Format("Old password is not correct."));
            }

            currentUser.Password = input.NewPassword;
            currentUser.Password = new PasswordHasher().HashPassword(currentUser.Password);

            return new ChangePasswordOutput();
        }

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
