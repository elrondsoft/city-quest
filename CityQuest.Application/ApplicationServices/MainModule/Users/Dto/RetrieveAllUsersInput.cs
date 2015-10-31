using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class RetrieveAllUsersInput : RetrieveAllInput
    {
        public bool? OnlyWithDefaultRole { get; set; }
        public long? RoleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public IList<long> UserIds { get; set; }
    }
}
