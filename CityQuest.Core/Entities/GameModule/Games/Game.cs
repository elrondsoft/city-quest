using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Entities.GameModule.Games
{
    public class Game : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations
        public long LocationId { get; set; }
        public virtual Location Location { get; set; }

        public virtual ICollection<Key> Keys { get; set; }
        public virtual ICollection<GameTask> GameTasks { get; set; }
        #endregion

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        
        public bool IsActive { get; set; }
    }
}
