using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions.Dtos
{
    public class RetrieveDivisionInput : RetrieveInput
    {
        public long? DivisionId { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
    }
}
