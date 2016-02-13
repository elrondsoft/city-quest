using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityQuest;

namespace CityQuest.Entities.GameModule.Statistics.PlayerGameTaskStatistics
{
    public class PlayerGameTaskStatistic : FullAuditedEntity<long, User>
    {
        #region Relations

        public long GameTaskId { get; set; }
        public virtual GameTask GameTask { get; set; }

        public long PlayerCareerId { get; set; }
        public virtual PlayerCareer PlayerCareer { get; set; }

        #endregion

        public DateTime GameTaskStartDateTime { get; set; }
        public DateTime GameTaskEndDateTime { get; set; }
        public long GameTaskDurationInTicks { get; set; }

        #region Criterions for statistics

        public int? ReceivedPoints { get; set; }

        #endregion

        #region Ctors

        public PlayerGameTaskStatistic() { }

        public PlayerGameTaskStatistic(long gameTaskId, long playerCareerId, DateTime gameTaskStartDateTime,
            DateTime gameTaskEndDateTime, int? receivedPoints, long? durationLag = null) 
            : this()
        {
            durationLag = durationLag == null ? 0 : (((durationLag + (CityQuestConsts.TicksToRoundDateTime / 2) + 1) /
                CityQuestConsts.TicksToRoundDateTime) * CityQuestConsts.TicksToRoundDateTime);

            GameTaskId = gameTaskId;
            PlayerCareerId = playerCareerId;
            GameTaskStartDateTime = gameTaskStartDateTime.RoundDateTime();
            GameTaskEndDateTime = gameTaskEndDateTime.RoundDateTime();
            GameTaskDurationInTicks = durationLag == null ?
                GameTaskEndDateTime.Ticks - GameTaskStartDateTime.Ticks :
                GameTaskEndDateTime.Ticks - GameTaskStartDateTime.Ticks - (long)durationLag;
            ReceivedPoints = receivedPoints;
        }

        #endregion
    }
}
