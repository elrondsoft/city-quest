using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Keys.Dtos
{
    public class ActivateKeyOutput : IOutputDto
    {
        public GameLightDto ActivatedGame { get; set; }
    }
}
