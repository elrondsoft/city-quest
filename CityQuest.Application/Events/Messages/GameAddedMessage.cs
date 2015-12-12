using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameAddedMessage
    {
        public long GameId { get; set; }

        public GameAddedMessage(long gameId)
        {
            GameId = gameId;
        }
    }
}
