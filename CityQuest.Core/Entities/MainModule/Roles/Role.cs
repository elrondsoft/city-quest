using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Roles
{
    public class Role : FullAuditedEntity<long, User>, IRole<long>
    {
        /// <summary>
        /// Unique name of this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this a default role? (new user will get it)
        /// Default role can not be deleted
        /// </summary>
        public bool IsDefault { get; set; }

        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Role() { }

        public Role(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Id, Name);
        }
    }
}