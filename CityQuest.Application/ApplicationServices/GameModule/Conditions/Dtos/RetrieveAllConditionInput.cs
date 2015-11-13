using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions.Dtos
{
    public class RetrieveAllConditionInput : RetrieveAllInput
    {
        public string Name { get; set; }
        public IList<long> ConditionIds { get; set; }
    }
}
