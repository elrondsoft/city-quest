using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions.Dtos
{
    public class RetrieveConditionsForGameTaskInput : IInputDto
    {
        public long GameTaskId { get; set; }
    }
}
