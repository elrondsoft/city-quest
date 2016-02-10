using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos
{
    public class AnswerOnRequestInput : IInputDto
    {
        public long TeamRequestId { get; set; }
        public bool Answer { get; set; }
    }
}
