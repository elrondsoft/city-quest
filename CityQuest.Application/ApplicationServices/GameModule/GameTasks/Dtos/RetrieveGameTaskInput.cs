using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTasks.Dtos
{
    public class RetrieveGameTaskInput : RetrieveInput
    {
        public bool? IsActive { get; set; }
        public string Name { get; set; }
    }
}
