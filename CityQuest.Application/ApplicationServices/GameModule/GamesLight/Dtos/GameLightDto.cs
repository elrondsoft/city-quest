using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class GameLightDto : EntityDto<long>, IPassivable
    {
        #region Relations

        public long LocationId { get; set; }
        public long GameStatusId { get; set; }

        //public IList<GameTaskLightDto> GameTasks { get; set; }
        public long GameTasksCount { get; set; }

        public string LocationName { get; set; }
        public string GameStatusName { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
