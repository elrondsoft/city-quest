using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Roles.Dto
{
    public class PermissionDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
