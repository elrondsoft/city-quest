using Abp.Application.Services.Dto;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class TipLightDto : EntityDto<long>, IHasOrder
    {
        #region Relations

        public long GameTaskId { get; set; }

        #endregion

        public string Name { get; set; }
        public string TipText { get; set; }
        public int Order { get; set; }
    }
}
