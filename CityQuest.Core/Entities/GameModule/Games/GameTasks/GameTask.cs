using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Games.GameTasks
{
    public class GameTask : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public long GameTaskTypeId { get; set; }
        public virtual GameTaskType GameTaskType { get; set; }

        public virtual ICollection<Tip> Tips { get; set; }
        public virtual ICollection<Condition> Conditions { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskText { get; set; }

        public bool IsActive { get; set; }
    }
}
