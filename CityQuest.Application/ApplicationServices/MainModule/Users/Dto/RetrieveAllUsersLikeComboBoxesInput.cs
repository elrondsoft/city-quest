using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class RetrieveAllUsersLikeComboBoxesInput : IInputDto
    {
        public bool? OnlyWithDefaultRole { get; set; }
        public long? RoleId { get; set; }
        public IList<long> UserIds { get; set; }
    }
}
