using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class TeamGameStatisticChangedMessage
    {
        public long GameId { get; set; }
        public IList<long> TeamIds { get; set; }

        public TeamGameStatisticChangedMessage(long gameId, IList<long> teamIds)
        {
            GameId = gameId;
            TeamIds = teamIds;
        }
    }
}
