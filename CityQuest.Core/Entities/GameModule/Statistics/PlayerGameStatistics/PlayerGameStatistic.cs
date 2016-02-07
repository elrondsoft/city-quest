using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Statistics.PlayerGameStatistics
{
    public class PlayerGameStatistic : FullAuditedEntity<long, User>
    {
        #region Relations

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public long PlayerCareerId { get; set; }
        public virtual PlayerCareer PlayerCareer { get; set; }

        #endregion

        public DateTime GameStartDateTime { get; set; }
        public DateTime GameEndDateTime { get; set; }
        public long GameDurationInTicks { get; set; }

        #region Criterions for statistics

        public int? ReceivedPoints { get; set; }

        #endregion

        #region Ctors

        public PlayerGameStatistic() { }

        public PlayerGameStatistic(long gameId, long playerCareerId, DateTime gameStartDateTime, DateTime gameEndDateTime, long? durationLag = null)
        {
            GameId = gameId;
            PlayerCareerId = playerCareerId;
            GameStartDateTime = gameStartDateTime;
            GameEndDateTime = gameEndDateTime;
            GameDurationInTicks = durationLag == null ?
                gameEndDateTime.Ticks - gameStartDateTime.Ticks :
                gameEndDateTime.Ticks - gameStartDateTime.Ticks - (long)durationLag;
        }

        #endregion
    }
}
