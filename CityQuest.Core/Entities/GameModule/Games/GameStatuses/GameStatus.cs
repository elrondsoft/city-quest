using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Games.GameStatuses
{
    public class GameStatus : FullAuditedEntity<long, User>
    {
        #region Relations

        public virtual ICollection<Game> Games { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string NextAllowedStatusNames { get; set; }
        public bool IsDefault { get; set; }
    }
}
