using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Entities.GameModule.Games.GameTasks.Tips
{
    public class Tip : FullAuditedEntity<long, User>, IHasOrder
    {
        #region Relations

        public long GameTaskId { get; set; }
        public virtual GameTask GameTask { get; set; }

        #endregion

        public string Name { get; set; }
        public string TipText { get; set; }
        public int Order { get; set; }

        #region Ctors

        public Tip() { }

        #endregion
    }
}
