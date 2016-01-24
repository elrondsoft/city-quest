using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.SignalR.StatisticsChangesServices
{
    public class SignalRStatisticsChangesHub : Hub
    {
        #region Injected Dependencies

        protected SignalRStatisticsChangesNotifier SignalRStatisticsChangesNotifier { get; set; }

        #endregion

        #region ctors

        public SignalRStatisticsChangesHub()
        {
            SignalRStatisticsChangesNotifier = Abp.Dependency.IocManager.Instance.Resolve<SignalRStatisticsChangesNotifier>();
        }

#warning Not working (need to resolve it later)
        public SignalRStatisticsChangesHub(SignalRStatisticsChangesNotifier signalRStatisticsChangesNotifier)
        {
            SignalRStatisticsChangesNotifier = signalRStatisticsChangesNotifier;
        }

        #endregion

        #region Hub's server methods

        #endregion
    }
}