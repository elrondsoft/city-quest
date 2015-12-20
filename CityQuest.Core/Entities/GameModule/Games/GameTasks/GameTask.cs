using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Statistics.PlayerGameTaskStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Games.GameTasks
{
    public class GameTask : FullAuditedEntity<long, User>, IPassivable, IHasOrder
    {
        #region Relations

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public long GameTaskTypeId { get; set; }
        public virtual GameTaskType GameTaskType { get; set; }

        public virtual ICollection<Tip> Tips { get; set; }
        public virtual ICollection<Condition> Conditions { get; set; }

        public virtual ICollection<PlayerGameTaskStatistic> PlayerGameTaskStatistics { get; set; }
        public virtual ICollection<TeamGameTaskStatistic> TeamGameTaskStatistics { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskText { get; set; }
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
