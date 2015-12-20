using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Statistics.TeamGameStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
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

        public long DivisionId { get; set; }
        public virtual Division Division { get; set; }

        public virtual ICollection<PlayerCareer> PlayerCareers { get; set; }
        public virtual ICollection<TeamGameStatistic> TeamGameStatistics { get; set; }
        public virtual ICollection<TeamGameTaskStatistic> TeamGameTaskStatistics { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string Slogan { get; set; }
        public bool IsActive { get; set; }


        public virtual PlayerCareer Captain
        {
            get
            {
                return PlayerCareers.SingleOrDefault(r => r.IsCaptain);
            }
        }
        //public virtual ICollection<PlayerCareer> Players
        //{
        //    get
        //    {
        //        return (ICollection<PlayerCareer>)PlayerCareers.Where(r => r.CareerDateEnd == null);
        //    }
        //}
    }
}
