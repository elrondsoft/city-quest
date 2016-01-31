using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Achievements
{
    public class Achievement : FullAuditedEntity<long, User>
    {
        #region Relations

        public virtual ICollection<User> Users { get; set; }
        
        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
    }
}