using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions.Dtos
{
    public class ConditionDto : FullAuditedEntityDto<long>, IHasOrder
    {
        #region Relations

        public long GameTaskId { get; set; }

        public long ConditionTypeId { get; set; }
        public ConditionTypeDto ConditionType { get; set; }

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string ValueToPass { get; set; }
        public int Order { get; set; }
        public int? Points { get; set; }
    }
}
