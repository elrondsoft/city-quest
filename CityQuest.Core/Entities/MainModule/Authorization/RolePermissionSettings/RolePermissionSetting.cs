using CityQuest.Entities.MainModule.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.RolePermissionSettings
{
    public class RolePermissionSetting : PermissionSetting
    {
        public virtual Role Role { get; set; }
        public long RoleId { get; set; }
    }
}