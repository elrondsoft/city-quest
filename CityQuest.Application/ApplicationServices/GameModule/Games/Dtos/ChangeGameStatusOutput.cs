using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class ChangeGameStatusOutput: IOutputDto
    {
        public GameStatusDto NewGameStatus { get; set; }
    }
}
