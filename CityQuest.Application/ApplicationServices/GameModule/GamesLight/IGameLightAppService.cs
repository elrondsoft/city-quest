using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight
{
    /// <summary>
    /// Is used like ApplicationService for Game and would be used by players
    /// this ApplicationService would allow to retrieve information
    /// </summary>
    public interface IGameLightAppService : IApplicationService
    {
        RetrieveGameCollectionOutput RetrieveGameCollection(RetrieveGameCollectionInput input);
        RetrieveGameLightOutput RetrieveGameLight(RetrieveGameLightInput input);
        RetrieveGameLightTasksOutput RetrieveGameLightTasks(RetrieveGameLightTasksInput input);
        TryToPassConditionOutput TryToPassCondition(TryToPassConditionInput input);
        RetrieveGameTaskResultsOutput RetrieveGameTaskResults(RetrieveGameTaskResultsInput input);
        RetrieveScoreBoardForGameOutput RetrieveScoreBoardForGame(RetrieveScoreBoardForGameInput input);
    }
}
