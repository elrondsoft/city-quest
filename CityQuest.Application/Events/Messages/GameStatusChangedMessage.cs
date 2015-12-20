using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameStatusChangedMessage
    {
        public long GameId { get; set; }
        public long NewStatusId { get; set; }
        public GameStatusDto NewGameStatus { get; set; }

        public GameStatusChangedMessage(long gameId, long newStatusId, GameStatusDto newGameStatus)
        {
            GameId = gameId;
            NewStatusId = newStatusId;
            NewGameStatus = newGameStatus;
        }
    }
}
