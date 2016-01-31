using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class RetrieveGameLightTasksOutput : IOutputDto
    {
        public GameLightDto Game { get; set; }
        public IList<GameTaskLightDto> GameTasks { get; set; }
        public IList<long> CompletedGameTaskIds { get; set; }
        public long? InProgressGameTaskId { get; set; }
    }
}
