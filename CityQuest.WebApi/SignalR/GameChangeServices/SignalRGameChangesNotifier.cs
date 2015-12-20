using Abp.Dependency;
using CityQuest.Events.Notifiers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.SignalR.GameChangeServices
{
    public class SignalRGameChangesNotifier : ISingletonDependency
    {
        #region Injected Dependencies

        protected IGameChangesNotifier GameChangesNotifier { get; set; }

        protected IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<SignalRGameChangesHub>();
            }
        }

        #endregion

        #region ctors

        public SignalRGameChangesNotifier(IGameChangesNotifier gameChangesNotifier)
        {
            GameChangesNotifier = gameChangesNotifier;
            RegisterCallbacks();
        }

        #endregion

        #region Helpers

        private void RegisterCallbacks()
        {
            GameChangesNotifier.OnKeyActivated += (sender, msg) => { RaiseClientOnKeyActivated(msg); };
            GameChangesNotifier.OnGameAdded += (sender, msg) => { RaiseClientOnGameAdded(msg); };
            GameChangesNotifier.OnGameUpdated += (sender, msg) => { RaiseClientOnGameUpdated(msg); };
            GameChangesNotifier.OnGameStatusChanged += (sender, msg) => { RaiseClientOnGameStatusChanged(msg); };
            GameChangesNotifier.OnGameDeleted += (sender, msg) => { RaiseClientOnGameDeleted(msg); };
            GameChangesNotifier.OnGameDeactivated += (sender, msg) => { RaiseClientOnGameDeactivated(msg); };
            GameChangesNotifier.OnGameActivated += (sender, msg) => { RaiseClientOnGameActivated(msg); };
        }

        #endregion

        #region Raise client methods

        protected virtual void RaiseClientOnKeyActivated(Events.Messages.KeyActivatedMessage msg)
        {
            HubContext.Clients.All.OnKeyActivated(msg);
        }

        protected virtual void RaiseClientOnGameAdded(Events.Messages.GameAddedMessage msg)
        {
            HubContext.Clients.All.OnGameAdded(msg);
        }

        protected virtual void RaiseClientOnGameUpdated(Events.Messages.GameUpdatedMessage msg)
        {
            HubContext.Clients.All.OnGameUpdated(msg);
        }

        protected virtual void RaiseClientOnGameStatusChanged(Events.Messages.GameStatusChangedMessage msg)
        {
            HubContext.Clients.All.OnGameStatusChanged(msg);
        }

        protected virtual void RaiseClientOnGameDeleted(Events.Messages.GameDeletedMessage msg)
        {
            HubContext.Clients.All.OnGameDeleted(msg);
        }

        protected virtual void RaiseClientOnGameDeactivated(Events.Messages.GameDeactivatedMessage msg)
        {
            HubContext.Clients.All.OnGameDeactivated(msg);
        }

        protected virtual void RaiseClientOnGameActivated(Events.Messages.GameActivatedMessage msg)
        {
            HubContext.Clients.All.OnGameActivated(msg);
        }

        #endregion
    }
}
