using CityQuest.Events.Notifiers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.SignalR.GameChangeServices
{
    public class SignalRGameChangesHub : Hub
    {
        #region Injected Dependencies

        protected SignalRGameChangesNotifier SignalRGameChangesNotifier { get; set; }

        #endregion

        #region ctors

        public SignalRGameChangesHub()
        {
            SignalRGameChangesNotifier = Abp.Dependency.IocManager.Instance.Resolve<SignalRGameChangesNotifier>();
        }

#warning Not working (need to resolve it later)
        public SignalRGameChangesHub(SignalRGameChangesNotifier signalRGameChangesNotifier)
        {
            SignalRGameChangesNotifier = signalRGameChangesNotifier;
        }

        #endregion

        #region Hub's server methods

        #endregion
    }
}