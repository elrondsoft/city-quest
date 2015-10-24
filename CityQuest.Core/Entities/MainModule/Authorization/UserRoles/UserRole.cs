using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserRoles
{
    public class UserRole : FullAuditedEntity<long, User>
    {
        public virtual long UserId { get; set; }

        public virtual long RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

        public UserRole() { }

        public UserRole(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
