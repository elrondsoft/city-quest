using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTasks
{
    /// <summary>
    /// Is used like ApplicationService for GameTask
    /// </summary>
    public interface IGameTaskAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (GameTasks) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<GameTaskDto, long> RetrieveAllPagedResult(RetrieveAllGameTasksPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (GameTasks) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllGameTasksLikeComboBoxesOutput RetrieveAllGameTasksLikeComboBoxes(RetrieveAllGameTasksLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (GameTasks)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<GameTaskDto, long> RetrieveAll(RetrieveAllGameTaskInput input);

        /// <summary>
        /// Is used to retrieve entity (GameTask) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<GameTaskDto, long> Retrieve(RetrieveGameTaskInput input);

        /// <summary>
        /// Is used to create entity (GameTask) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<GameTaskDto, long> Create(CreateInput<GameTaskDto, long> input);

        /// <summary>
        /// Is used to update entity (GameTask) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<GameTaskDto, long> Update(UpdateInput<GameTaskDto, long> input);

        /// <summary>
        /// Is used to delete entity (GameTask) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change GameTask's activity (field IsActive)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<GameTaskDto, long> ChangeActivity(ChangeActivityInput input);

        /// <summary>
        /// Is used to retrieve GameTasks for Game
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveGameTasksForGameOutput RetrieveGameTasksForGame(RetrieveGameTasksForGameInput input);
    }
}
