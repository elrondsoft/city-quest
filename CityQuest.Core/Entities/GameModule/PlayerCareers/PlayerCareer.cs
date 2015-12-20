using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using CityQuest.Entities.GameModule.Statistics.PlayerGameStatistics;
using CityQuest.Entities.GameModule.Statistics.PlayerGameTaskStatistics;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.PlayerCareers
{
    public class PlayerCareer : FullAuditedEntity<long, User>, IPassivable
    {
        #region Relations

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public long TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<SuccessfulPlayerAttempt> SuccessfulPlayerAttempts { get; set; }
        public virtual ICollection<UnsuccessfulPlayerAttempt> UnsuccessfulPlayerAttempts { get; set; }
        public virtual ICollection<PlayerGameStatistic> PlayerGameStatistics { get; set; }
        public virtual ICollection<PlayerGameTaskStatistic> PlayerGameTaskStatistics { get; set; }

        #endregion

        public bool IsCaptain { get; set; }
        public DateTime CareerDateStart { get; set; }
        public DateTime? CareerDateEnd { get; set; }

        public bool IsActive { get; set; }
    }
}
