using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts
{
    public class UnsuccessfulPlayerAttempt : FullAuditedEntity<long, User>
    {
        #region Relations

        public long ConditionId { get; set; }
        public Condition Condition { get; set; }

        public long PlayerCareerId { get; set; }
        public PlayerCareer PlayerCareer { get; set; }
        
        #endregion

        public string InputedValue { get; set; }
        public DateTime AttemptDateTime { get; set; }
    }
}
