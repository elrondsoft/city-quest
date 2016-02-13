using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Permissions.Dto
{
    public class PermissionDto : EntityDto<long>
    {
        public string Value { get; set; }
        public string DisplayText { get; set; }

        #region Ctors

        public PermissionDto() { }

        #endregion
    }
}
