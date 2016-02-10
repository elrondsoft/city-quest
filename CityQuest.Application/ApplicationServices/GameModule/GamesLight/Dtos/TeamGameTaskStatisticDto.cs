using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class TeamGameTaskStatisticDto : EntityDto<long>
    {
        #region Relations

        public long GameTaskId { get; set; }
        public string GameTaskName { get; set; }
        public int GameTaskOrder { get; set; }

        public long TeamId { get; set; }
        public string TeamName { get; set; }

        #endregion

        public DateTime GameTaskStartDateTime { get; set; }
        public DateTime GameTaskEndDateTime { get; set; }
        public long GameTaskDurationInTicks { get; set; }

        #region Criterions for statistics

        public int? ReceivedPoints { get; set; }

        #endregion

        #region Ctors

        public TeamGameTaskStatisticDto() { }

        #endregion
    }
}
