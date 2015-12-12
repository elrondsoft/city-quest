using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameDeletedMessage
    {
        public long GameId { get; set; }

        public GameDeletedMessage(long gameId)
        {
            GameId = gameId;
        }
    }
}
