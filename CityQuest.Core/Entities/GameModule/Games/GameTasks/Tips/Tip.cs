using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Entities.GameModule.Games.GameTasks.Tips
{
    public class Tip : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations

        public long GameTaskId { get; set; }
        public virtual GameTask GameTask { get; set; }

        #endregion

        public string Name { get; set; }
        public string TipText { get; set; }

        public bool IsActive { get; set; }
    }
}
