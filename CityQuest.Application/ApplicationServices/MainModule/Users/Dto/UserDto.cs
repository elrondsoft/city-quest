using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class UserDto : FullAuditedEntityDto<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public long? LocationId { get; set; }
        public IList<RoleDto> Roles { get; set; }

        public string LastModifierUserFullName { get; set; }

        #region Ctors

        public UserDto()
        {
            Roles = new List<RoleDto>();
        }

        #endregion
    }
}
