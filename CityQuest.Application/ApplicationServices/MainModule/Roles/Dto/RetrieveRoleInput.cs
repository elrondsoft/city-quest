using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Roles.Dto
{
    public class RetrieveRoleInput : RetrieveInput
    {
        public string Name { get; set; }
    }
}
