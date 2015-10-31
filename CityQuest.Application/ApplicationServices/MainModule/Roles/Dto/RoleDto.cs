using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Roles.Dto
{
    public class RoleDto : FullAuditedEntityDto<long>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<PermissionDto> Permissions { get; set; }

        public string DisplayPermissions
        {
            get
            {
                if (Permissions == null)
                    return string.Empty;

                return Permissions.Select(r => r.Name).JoinAsString(", ");
            }
        }
    }
}