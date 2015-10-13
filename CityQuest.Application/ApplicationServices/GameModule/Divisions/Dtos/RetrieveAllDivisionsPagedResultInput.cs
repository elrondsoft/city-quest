using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions.Dtos
{
    public class RetrieveAllDivisionsPagedResultInput : RetrieveAllPagedResultInput
    {
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public IList<long> DivisionIds { get; set; }
    }
}
