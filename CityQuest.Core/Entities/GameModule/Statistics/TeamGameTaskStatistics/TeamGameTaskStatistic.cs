using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics
{
    public class TeamGameTaskStatistic : FullAuditedEntity<long, User>
    {
        #region Relations

        public long GameTaskId { get; set; }
        public virtual GameTask GameTask { get; set; }

        public long TeamId { get; set; }
        public virtual Team Team { get; set; }

        #endregion

        public DateTime GameTaskStartDateTime { get; set; }
        public DateTime GameTaskEndDateTime { get; set; }
        public long GameTaskDurationInTicks { get; set; }

        #region Criterions for statistics

        public int? ReceivedPoints { get; set; }

        #endregion
    }
}
