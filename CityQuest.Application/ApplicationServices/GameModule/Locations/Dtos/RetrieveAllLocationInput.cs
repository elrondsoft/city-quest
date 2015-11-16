using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Locations.Dtos
{
    public class RetrieveAllLocationInput : RetrieveAllInput
    {
        public string Name { get; set; }
        public IList<long> LocationIds { get; set; }
    }
}
