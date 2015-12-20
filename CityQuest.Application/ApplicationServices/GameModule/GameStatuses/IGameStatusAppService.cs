using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameStatuses
{
    /// <summary>
    /// Is used like ApplicationService for GameStatus
    /// </summary>
    public interface IGameStatusAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (GameStatuses)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<GameStatusDto, long> RetrieveAll(RetrieveAllGameStatusesInput input);

        /// <summary>
        /// Is used to retrieve all entities (GameStatuses) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllGameStatusesLikeComboBoxesOutput RetrieveAllGameStatusesLikeComboBoxes(RetrieveAllGameStatusesLikeComboBoxesInput input);
    }
}
