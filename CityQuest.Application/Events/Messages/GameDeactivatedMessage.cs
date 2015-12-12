using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameDeactivatedMessage
    {
        public long GameId { get; set; }

        public GameDeactivatedMessage(long gameId)
        {
            GameId = gameId;
        }
    }
}
