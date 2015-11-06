using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Teams
{
    /// <summary>
    /// Is used like ApplicationService for Team
    /// </summary>
    public interface ITeamAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Teams) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<TeamDto, long> RetrieveAllPagedResult(RetrieveAllTeamsPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (Teams) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllTeamsLikeComboBoxesOutput RetrieveAllTeamsLikeComboBoxes(RetrieveAllTeamsLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (Teams)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<TeamDto, long> RetrieveAll(RetrieveAllTeamsInput input);

        /// <summary>
        /// Is used to retrieve entity (Team) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<TeamDto, long> Retrieve(RetrieveTeamInput input);

        /// <summary>
        /// Is used to create entity (Team) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<TeamDto, long> Create(CreateTeamInput input);

        /// <summary>
        /// Is used to update entity (Team) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<TeamDto, long> Update(UpdateTeamInput input);

        /// <summary>
        /// Is used to delete entity (Team) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change Team's activity (field IsActive)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<TeamDto, long> ChangeActivity(ChangeActivityInput input);
    }
}
