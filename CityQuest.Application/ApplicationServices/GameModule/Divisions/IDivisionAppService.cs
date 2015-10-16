using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.Shared;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions
{
    /// <summary>
    /// Is used like ApplicationService for Division
    /// </summary>
    public interface IDivisionAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Divisions) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<DivisionDto, long> RetrieveAllPagedResult(RetrieveAllDivisionsPagedResultInput input);
        
        /// <summary>
        /// Is used to retrieve all entities (Divisions) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllDivisionsLikeComboBoxesOutput RetrieveAllDivisionsLikeComboBoxes(RetrieveAllDivisionsLikeComboBoxesInput input);
        
        /// <summary>
        /// Is used to retrieve all entities (Divisions)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<DivisionDto, long> RetrieveAll(RetrieveAllDivisionInput input);
        
        /// <summary>
        /// Is used to retrieve entity (Division) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<DivisionDto, long> Retrieve(RetrieveDivisionInput input);
        
        /// <summary>
        /// Is used to create entity (Division) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<DivisionDto, long> Create(CreateInput<DivisionDto, long> input);
        
        /// <summary>
        /// Is used to update entity (Division) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<DivisionDto, long> Update(UpdateInput<DivisionDto, long> input);
        
        /// <summary>
        /// Is used to delete entity (Division) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change Division's activity (field IsActive)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<DivisionDto, long> ChangeActivity(ChangeActivityInput input);
    }
}
