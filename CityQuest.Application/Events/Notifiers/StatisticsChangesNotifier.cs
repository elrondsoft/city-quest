using Abp.Dependency;
using CityQuest.Events.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Notifiers
{
    public class StatisticsChangesNotifier : IStatisticsChangesNotifier, ISingletonDependency
    {
        #region Events

        public event EventHandler<GameTaskCompletedMessage> OnGameTaskCompleted;
        public event EventHandler<PlayerGameStatisticChangedMessage> OnPlayerGameStatisticChanged;
        public event EventHandler<TeamGameStatisticChangedMessage> OnTeamGameStatisticChanged;

        #endregion

        #region Methods to raise events

        public virtual void RaiseOnGameTaskCompleted(GameTaskCompletedMessage msg)
        {
            EventHandler<GameTaskCompletedMessage> handler = OnGameTaskCompleted;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnPlayerGameStatisticChanged(PlayerGameStatisticChangedMessage msg)
        {
            EventHandler<PlayerGameStatisticChangedMessage> handler = OnPlayerGameStatisticChanged;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnTeamGameStatisticChanged(TeamGameStatisticChangedMessage msg)
        {
            EventHandler<TeamGameStatisticChangedMessage> handler = OnTeamGameStatisticChanged;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        #endregion
    }
}
