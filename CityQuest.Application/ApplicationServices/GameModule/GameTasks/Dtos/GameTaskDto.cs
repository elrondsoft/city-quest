using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos;
using CityQuest.ApplicationServices.GameModule.Tips.Dtos;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.ApplicationServices.GameModule.GameTasks.Dtos
{
    public class GameTaskDto : FullAuditedEntityDto<long>, IPassivable, IHasOrder
    {
        #region Relations

        public long GameId { get; set; }

        public long GameTaskTypeId { get; set; }
        public GameTaskTypeDto GameTaskType { get; set; }

        public IList<TipDto> Tips { get; set; }
        public IList<ConditionDto> Conditions { get; set; }

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskText { get; set; }
        public int Order { get; set; }

        public bool IsActive { get; set; }

        #region Ctors

        public GameTaskDto()
        {
            Tips = new List<TipDto>();
            Conditions = new List<ConditionDto>();
        }

        #endregion
    }
}