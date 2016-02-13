using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class RetrieveScoreBoardForGameOutput : IOutputDto
    {
        public IList<ScoreBoardDataDto> ScoreBoardData { get; set; }
    }
}
