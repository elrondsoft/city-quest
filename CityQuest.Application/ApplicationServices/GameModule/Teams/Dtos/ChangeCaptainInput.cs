using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Teams.Dtos
{
    public class ChangeCaptainInput : IInputDto
    {
        public long TeamId { get; set; }
        public long NewCaptainCareerId { get; set; }
    }
}
