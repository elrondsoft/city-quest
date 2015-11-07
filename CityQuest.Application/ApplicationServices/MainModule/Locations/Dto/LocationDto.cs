using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Locations.Dto
{
    public class LocationDto : FullAuditedEntityDto<long>, IPassivable
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int Order { get; set; } 


    }
}
