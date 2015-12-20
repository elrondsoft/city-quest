using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameUpdatedMessage
    {
        public long GameId { get; set; }

        public GameUpdatedMessage(long gameId)
        {
            GameId = gameId;
        }
    }
}
