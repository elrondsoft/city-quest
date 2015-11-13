using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions
{
    /// <summary>
    /// Is used like ApplicationService for Condition
    /// </summary>
    public interface IConditionAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Conditions) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<ConditionDto, long> RetrieveAllPagedResult(RetrieveAllConditionsPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (Conditions) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllConditionsLikeComboBoxesOutput RetrieveAllConditionsLikeComboBoxes(RetrieveAllConditionsLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (Conditions)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<ConditionDto, long> RetrieveAll(RetrieveAllConditionInput input);

        /// <summary>
        /// Is used to retrieve entity (Condition) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<ConditionDto, long> Retrieve(RetrieveConditionInput input);

        /// <summary>
        /// Is used to create entity (Condition) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<ConditionDto, long> Create(CreateInput<ConditionDto, long> input);

        /// <summary>
        /// Is used to update entity (Condition) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<ConditionDto, long> Update(UpdateInput<ConditionDto, long> input);

        /// <summary>
        /// Is used to delete entity (Condition) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);
    }
}
