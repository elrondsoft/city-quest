using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users.Dto
{
    public class UpdatePublicUserFieldsOutput: IOutputDto
    {
        public UserDto User { get; set; }
    }
}
