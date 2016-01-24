using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class GameTaskCompletedMessage
    {
        public long GameTaskId { get; set; }
        public IList<long> UserCompleterIds { get; set; }

        public GameTaskCompletedMessage(long gameTaskId, IList<long> userCompleterIds)
        {
            GameTaskId = gameTaskId;
            UserCompleterIds = userCompleterIds;
        }
    }
}
