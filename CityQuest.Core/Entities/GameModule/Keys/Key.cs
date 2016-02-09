using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Keys
{
    public class Key : FullAuditedEntity<long, User>
    {
        #region Relations

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public long? OwnerUserId { get; set; }
        public virtual User OwnerUser { get; set; }

        #endregion

        public string KeyValue { get; set; }

        #region Ctors

        public Key() { }

        #endregion
    }
}
