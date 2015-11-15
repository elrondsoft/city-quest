using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Tips.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Tips
{
    /// <summary>
    /// Is used like ApplicationService for Tip
    /// </summary>
    public interface ITipAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Tips) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<TipDto, long> RetrieveAllPagedResult(RetrieveAllTipsPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (Tips) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllTipsLikeComboBoxesOutput RetrieveAllTipsLikeComboBoxes(RetrieveAllTipsLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (Tips)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<TipDto, long> RetrieveAll(RetrieveAllTipInput input);

        /// <summary>
        /// Is used to retrieve entity (Tip) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<TipDto, long> Retrieve(RetrieveTipInput input);

        /// <summary>
        /// Is used to create entity (Tip) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<TipDto, long> Create(CreateInput<TipDto, long> input);

        /// <summary>
        /// Is used to update entity (Tip) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<TipDto, long> Update(UpdateInput<TipDto, long> input);

        /// <summary>
        /// Is used to delete entity (Tip) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to retrieve Tips for GameTask
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveTipsForGameTaskOutput RetrieveTipsForGameTask(RetrieveTipsForGameTaskInput input);
    }
}
