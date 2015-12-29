using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class EndGameInput : IInputDto
    {
        public long GameId { get; set; }
    }
}
