using Abp.Authorization;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Locations
{
    public class LocationPolicy : CityQuestPolicyBase<Location, long>, ILocationPolicy
    {
        public LocationPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }
    }
}
