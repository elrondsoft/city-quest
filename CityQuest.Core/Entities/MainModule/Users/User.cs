using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.Teams;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Users
{
    public class User : FullAuditedEntity<long, User>, IUser<long>
    {
        #region Relations

        public virtual ICollection<Key> ActivatedKeys { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Team> LeadedTeams { get; set; }

        #endregion

        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Is the <see cref="EmailAddress"/> confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }
        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        public string EmailConfirmationCode { get; set; }
        /// <summary>
        /// The last time this user entered to the system.
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }


        //public virtual ICollection<UserLogin> Logins { get; set; }

        //public virtual ICollection<UserRole> Roles { get; set; }

        //public virtual ICollection<UserPermissionSetting> Permissions { get; set; }

        //public virtual ICollection<Setting> Settings { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.Name, this.Surname);
            }
        }
    }
}
