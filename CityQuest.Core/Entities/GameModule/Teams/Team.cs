using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Statistics.TeamGameStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
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
        public virtual ICollection<TeamRequest> TeamRequests { get; set; }
        public virtual ICollection<TeamGameStatistic> TeamGameStatistics { get; set; }
        public virtual ICollection<TeamGameTaskStatistic> TeamGameTaskStatistics { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string Slogan { get; set; }

        public bool IsActive { get; set; }

        #region Ctors

        public Team()
        {
            PlayerCareers = new HashSet<PlayerCareer>();
            TeamGameStatistics = new HashSet<TeamGameStatistic>();
            TeamGameTaskStatistics = new HashSet<TeamGameTaskStatistic>();
            TeamRequests = new HashSet<TeamRequest>();
        }

        #endregion

        public virtual PlayerCareer Captain 
        {
            get
            {
                return PlayerCareers.SingleOrDefault(r => r.IsCaptain && r.IsActive);
            }
        }

        public virtual IQueryable<PlayerCareer> CurrentPlayers 
        {
            get
            {
                return PlayerCareers.Where(r => r.CareerDateEnd == null && r.IsActive).AsQueryable<PlayerCareer>();
            }
        }
    }
}
