using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos
{
    public class AnswerOnRequestOutput : IOutputDto
    {
        public TeamRequestDto TeamRequest { get; set; }
    }
}
