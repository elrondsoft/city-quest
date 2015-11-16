using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Locations.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Locations
{
    /// <summary>
    /// Is used like ApplicationService for Location
    /// </summary>
    public interface ILocationAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Locations) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<LocationDto, long> RetrieveAllPagedResult(RetrieveAllLocationsPagedResultInput input);
        
        /// <summary>
        /// Is used to retrieve all entities (Locations) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllLocationsLikeComboBoxesOutput RetrieveAllLocationsLikeComboBoxes(RetrieveAllLocationsLikeComboBoxesInput input);
        
        /// <summary>
        /// Is used to retrieve all entities (Locations)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<LocationDto, long> RetrieveAll(RetrieveAllLocationInput input);
        
        /// <summary>
        /// Is used to retrieve entity (Location) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<LocationDto, long> Retrieve(RetrieveLocationInput input);
        
        /// <summary>
        /// Is used to create entity (Location) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<LocationDto, long> Create(CreateInput<LocationDto, long> input);
        
        /// <summary>
        /// Is used to update entity (Location) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<LocationDto, long> Update(UpdateInput<LocationDto, long> input);
        
        /// <summary>
        /// Is used to delete entity (Location) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);
    }
}
