using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class ConditionTypeLightDto : EntityDto<long>, IPassivable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }
    }
}
