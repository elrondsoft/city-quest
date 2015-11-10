using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTasks.Dtos
{
    public class GameTaskDto : FullAuditedEntityDto<long>, IPassivable
    {
        #region Relations

        public long GameId { get; set; }

        public long GameTaskTypeId { get; set; }
        //public virtual GameTaskTypeDto GameTaskType { get; set; }

        //public virtual IList<TipDto> Tips { get; set; }
        //public virtual IList<ConditionDto> Conditions { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskText { get; set; }

        public bool IsActive { get; set; }
    }
}