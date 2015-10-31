using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class ChangePasswordInput : IInputDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string NewPasswordRepeat { get; set; }
    }
}
