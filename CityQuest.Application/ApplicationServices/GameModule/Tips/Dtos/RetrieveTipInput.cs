using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Tips.Dtos
{
    public class RetrieveTipInput : RetrieveInput
    {
        public string Name { get; set; }
    }
}
