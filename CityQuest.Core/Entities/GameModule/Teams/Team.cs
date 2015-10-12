using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Teams
{
    public class Team : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations

        public long CaptainId { get; set; }
        public User Captain { get; set; }

        public long DivisionId { get; set; }
        public virtual Division Division { get; set; }

        public virtual ICollection<User> Players { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string Slogan { get; set; }

        public bool IsActive { get; set; }
    }
}
