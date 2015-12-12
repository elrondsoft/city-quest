using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class GameTaskLightDto : EntityDto<long>, IPassivable, IHasOrder
    {
        #region Relations

        public long GameId { get; set; }

        public long GameTaskTypeId { get; set; }
        public GameTaskTypeLightDto GameTaskType { get; set; }

        public IList<TipLightDto> Tips { get; set; }
        public IList<ConditionLightDto> Conditions { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskText { get; set; }
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
