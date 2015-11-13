using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Entities.GameModule.Games.GameTasks.Conditions
{
    public class Condition : FullAuditedEntity<long, User>, IHasOrder
    {
        #region Relations

        public long GameTaskId { get; set; }
        public virtual GameTask GameTask { get; set; }

        public long ConditionTypeId { get; set; }
        public virtual ConditionType ConditionType { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
