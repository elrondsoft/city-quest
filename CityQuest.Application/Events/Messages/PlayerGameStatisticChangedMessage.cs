using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class PlayerGameStatisticChangedMessage
    {
        public long GameId { get; set; }
        public IList<long> PlayerIds { get; set; }

        public PlayerGameStatisticChangedMessage(long gameId, IList<long> playerIds)
        {
            GameId = gameId;
            PlayerIds = playerIds;
        }
    }
}
