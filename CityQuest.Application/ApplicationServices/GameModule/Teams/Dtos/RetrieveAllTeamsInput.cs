using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Teams.Dtos
{
    public class RetrieveAllTeamsInput : RetrieveAllInput
    {
        public bool? IsActive { get; set; }
        public long? DivisionId { get; set; }
        public string Name { get; set; }
        public IList<long> TeamIds { get; set; }
    }
}
