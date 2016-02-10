using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos
{
    public class RetrieveAllTeamRequestsInput : RetrieveAllInput
    {
        public long? UserId { get; set; }
        public long? TeamId { get; set; }
    }
}
