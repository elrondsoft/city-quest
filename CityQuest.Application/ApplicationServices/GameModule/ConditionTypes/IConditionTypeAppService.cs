using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.ConditionTypes
{
    /// <summary>
    /// Is used like ApplicationService for ConditionType
    /// </summary>
    public interface IConditionTypeAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (ConditionTypes) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<ConditionTypeDto, long> RetrieveAllPagedResult(RetrieveAllConditionTypesPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (ConditionTypes) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllConditionTypesLikeComboBoxesOutput RetrieveAllConditionTypesLikeComboBoxes(RetrieveAllConditionTypesLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (ConditionTypes)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<ConditionTypeDto, long> RetrieveAll(RetrieveAllConditionTypeInput input);

        /// <summary>
        /// Is used to retrieve entity (ConditionType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<ConditionTypeDto, long> Retrieve(RetrieveConditionTypeInput input);

        /// <summary>
        /// Is used to create entity (ConditionType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<ConditionTypeDto, long> Create(CreateInput<ConditionTypeDto, long> input);

        /// <summary>
        /// Is used to update entity (ConditionType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<ConditionTypeDto, long> Update(UpdateInput<ConditionTypeDto, long> input);

        /// <summary>
        /// Is used to delete entity (ConditionType) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change ConditionType's activity (field IsActive)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<ConditionTypeDto, long> ChangeActivity(ChangeActivityInput input);
    }
}
