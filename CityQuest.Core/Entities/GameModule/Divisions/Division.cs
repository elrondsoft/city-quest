using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Divisions
{
    public class Division : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations

        public virtual ICollection<Team> Teams { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        #region Ctors

        public Division()
        {
            Teams = new HashSet<Team>();
        }

        #endregion
    }
}
