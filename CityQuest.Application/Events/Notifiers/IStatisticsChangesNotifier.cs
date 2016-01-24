using Abp.Dependency;
using CityQuest.Events.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Notifiers
{
    public interface IStatisticsChangesNotifier : ITransientDependency
    {
        event EventHandler<GameTaskCompletedMessage> OnGameTaskCompleted;
        event EventHandler<PlayerGameStatisticChangedMessage> OnPlayerGameStatisticChanged;
        event EventHandler<TeamGameStatisticChangedMessage> OnTeamGameStatisticChanged;

        void RaiseOnGameTaskCompleted(GameTaskCompletedMessage msg);
        void RaiseOnPlayerGameStatisticChanged(PlayerGameStatisticChangedMessage msg);
        void RaiseOnTeamGameStatisticChanged(TeamGameStatisticChangedMessage msg);
    }
}
