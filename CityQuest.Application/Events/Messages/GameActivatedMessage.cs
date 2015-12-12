using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameActivatedMessage
    {
        public long GameId { get; set; }

        public GameActivatedMessage(long gameId)
        {
            GameId = gameId;
        }
    }
}
