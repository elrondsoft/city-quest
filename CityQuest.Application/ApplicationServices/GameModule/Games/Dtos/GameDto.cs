using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class GameDto : FullAuditedEntityDto<long>, IPassivable
    {
        #region Relations

        public long LocationId { get; set; }
        public long GameStatusId { get; set; }
        public GameStatusDto GameStatus { get; set; }

        public IList<GameTaskDto> GameTasks { get; set; }

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }
        public string LocationName { get; set;}

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
