using Abp.Dependency;
using Abp.Runtime.Session;
using CityQuest.Entities.MainModule.Authorization.UserLogins;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserServices
{
    public class UserStore :
        IUserPasswordStore<User, long>,
        IUserEmailStore<User, long>,
        IUserLoginStore<User, long>,
        IUserRoleStore<User, long>,
        IQueryableUserStore<User, long>,
        IUserLockoutStore<User, long>,
        IUserTwoFactorStore<User, long>,
        IUserPhoneNumberStore<User, long>,
        ITransientDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAbpSession _session;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserStore(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IAbpSession session)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public async Task<User> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => 
                    (user.UserName.ToLower() == userName.ToLower() || 
                        user.EmailAddress.ToLower() == userName.ToLower()) && 
                    user.IsEmailConfirmed);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<string> GetPasswordHashAsync(User user)
        {
            return (await _userRepository.GetAsync(user.Id)).Password;
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            return !string.IsNullOrEmpty((await _userRepository.GetAsync(user.Id)).Password);
        }

        public async Task SetEmailAsync(User user, string email)
        {
            user.EmailAddress = email;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<string> GetEmailAsync(User user)
        {
            return (await _userRepository.GetAsync(user.Id)).EmailAddress;
        }

        public async Task<bool> GetEmailConfirmedAsync(User user)
        {
            return (await _userRepository.GetAsync(user.Id)).IsEmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(user => user.EmailAddress.ToLower() == email.ToLower());
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
                );
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return (await _userLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public async Task<User> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );
            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(userLogin.UserId);
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            await _userRoleRepository.InsertAsync(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            //TODO: This is not implemented as async.
            var roleNames = _userRoleRepository.Query(userRoles => (from userRole in userRoles
                                                                    join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                                                                    where userRole.UserId == user.Id
                                                                    select role.Name).ToList());

            return Task.FromResult<IList<string>>(roleNames);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        public IQueryable<User> Users
        {
            get { return _userRepository.GetAll(); }
        }

        public void Dispose()
        {
            //No need to dispose since using IOC.
        }

        #region lockout
        public Task<int> GetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task<bool>.Factory.StartNew(() => false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task<bool>.Factory.StartNew(() => false); ;
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }


        #region phone
        public Task<string> GetPhoneNumberAsync(User user)
        {
            return Task.Factory.StartNew(() => "Unknown");
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return Task.Factory.StartNew(() => false);
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
