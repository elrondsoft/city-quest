using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions.Dtos
{
    public class RetrieveConditionsForGameTaskOutput : IOutputDto
    {
        public IList<ConditionDto> Conditions { get; set; }
    }
}
