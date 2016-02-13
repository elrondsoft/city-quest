using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Statistics.TeamGameStatistics
{
    public class TeamGameStatistic : FullAuditedEntity<long, User>
    {
        #region Relations

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public long TeamId { get; set; }
        public virtual Team Team { get; set; }

        #endregion

        public DateTime GameStartDateTime { get; set; }
        public DateTime GameEndDateTime { get; set; }
        public long GameDurationInTicks { get; set; }

        #region Criterions for statistics

        public int? ReceivedPoints { get; set; }

        #endregion

        #region Ctors

        public TeamGameStatistic() { }

        public TeamGameStatistic(long gameId, long teamId, DateTime gameStartDateTime, DateTime gameEndDateTime, 
            int? receivedPoints, long? durationLag = null)
            : this()
        {
            durationLag = durationLag == null ? 0 : (((durationLag + (CityQuestConsts.TicksToRoundDateTime / 2) + 1) /
                CityQuestConsts.TicksToRoundDateTime) * CityQuestConsts.TicksToRoundDateTime);

            GameId = gameId;
            TeamId = teamId;
            GameStartDateTime = gameStartDateTime.RoundDateTime();
            GameEndDateTime = gameEndDateTime.RoundDateTime();
            GameDurationInTicks = durationLag == null ?
                GameEndDateTime.Ticks - GameStartDateTime.Ticks :
                GameEndDateTime.Ticks - GameStartDateTime.Ticks - (long)durationLag;
            ReceivedPoints = receivedPoints;
        }

        #endregion
    }
}
