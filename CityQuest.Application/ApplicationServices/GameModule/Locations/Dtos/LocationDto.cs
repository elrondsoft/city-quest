using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Locations.Dtos
{
    public class LocationDto : FullAuditedEntityDto<long>
    {
        #region Relations

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
    }
}
