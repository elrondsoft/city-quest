using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class TryToPassConditionInput : IInputDto
    {
        public long ConditionId { get; set; }
        public string Value { get; set; }
    }
}
