using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Tips.Dtos
{
    public class RetrieveAllTipsPagedResultInput : RetrieveAllPagedResultInput
    {
        public string Name { get; set; }
        public IList<long> TipIds { get; set; }
    }
}
