using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Locations.Dtos
{
    public class RetrieveAllLocationsPagedResultInput : RetrieveAllPagedResultInput
    {
        public string Name { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public IList<long> LocationIds { get; set; }
    }
}
