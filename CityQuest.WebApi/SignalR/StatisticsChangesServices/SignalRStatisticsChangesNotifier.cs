using Abp.Dependency;
using CityQuest.Events.Notifiers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.SignalR.StatisticsChangesServices
{
    public class SignalRStatisticsChangesNotifier : ISingletonDependency
    {
        #region Injected Dependencies

        protected IStatisticsChangesNotifier StatisticsChangesNotifier { get; set; }

        protected IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<SignalRStatisticsChangesHub>();
            }
        }

        #endregion

        #region ctors

        public SignalRStatisticsChangesNotifier(IStatisticsChangesNotifier statisticsChangesNotifier)
        {
            StatisticsChangesNotifier = statisticsChangesNotifier;
            RegisterCallbacks();
        }

        #endregion

        #region Helpers

        private void RegisterCallbacks()
        {
            StatisticsChangesNotifier.OnGameTaskCompleted += (sender, msg) => { RaiseClientOnGameTaskCompleted(msg); };
            StatisticsChangesNotifier.OnPlayerGameStatisticChanged += (sender, msg) => { RaiseClientOnPlayerGameStatisticChanged(msg); };
            StatisticsChangesNotifier.OnTeamGameStatisticChanged += (sender, msg) => { RaiseClientOnTeamGameStatisticChanged(msg); };
        }

        #endregion

        #region Raise client methods

        protected virtual void RaiseClientOnGameTaskCompleted(Events.Messages.GameTaskCompletedMessage msg)
        {
            HubContext.Clients.All.OnGameTaskCompleted(msg);
        }

        protected virtual void RaiseClientOnPlayerGameStatisticChanged(Events.Messages.PlayerGameStatisticChangedMessage msg)
        {
            HubContext.Clients.All.OnPlayerGameStatisticChange(msg);
        }

        protected virtual void RaiseClientOnTeamGameStatisticChanged(Events.Messages.TeamGameStatisticChangedMessage msg)
        {
            HubContext.Clients.All.OnTeamGameStatisticChanged(msg);
        }

        #endregion
    }
}