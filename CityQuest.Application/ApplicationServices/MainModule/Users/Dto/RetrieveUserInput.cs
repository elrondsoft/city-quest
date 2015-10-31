using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class RetrieveUserInput : RetrieveInput
    {
        public string UserName { get; set; }
    }
}
