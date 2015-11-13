using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTaskTypes
{
    /// <summary>
    /// Is used like ApplicationService for GameTaskType
    /// </summary>
    public interface IGameTaskTypeAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (GameTaskTypes) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<GameTaskTypeDto, long> RetrieveAllPagedResult(RetrieveAllGameTaskTypesPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (GameTaskTypes) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllGameTaskTypesLikeComboBoxesOutput RetrieveAllGameTaskTypesLikeComboBoxes(RetrieveAllGameTaskTypesLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (GameTaskTypes)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<GameTaskTypeDto, long> RetrieveAll(RetrieveAllGameTaskTypeInput input);

        /// <summary>
        /// Is used to retrieve entity (GameTaskType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<GameTaskTypeDto, long> Retrieve(RetrieveGameTaskTypeInput input);

        /// <summary>
        /// Is used to create entity (GameTaskType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<GameTaskTypeDto, long> Create(CreateInput<GameTaskTypeDto, long> input);

        /// <summary>
        /// Is used to update entity (GameTaskType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<GameTaskTypeDto, long> Update(UpdateInput<GameTaskTypeDto, long> input);

        /// <summary>
        /// Is used to delete entity (GameTaskType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change GameTaskType's activity (field IsActive)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<GameTaskTypeDto, long> ChangeActivity(ChangeActivityInput input);
    }
}
