using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserLogins
{
    /// <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    public class UserLogin : FullAuditedEntity<long, User>
    {
        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        public virtual string ProviderKey { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
