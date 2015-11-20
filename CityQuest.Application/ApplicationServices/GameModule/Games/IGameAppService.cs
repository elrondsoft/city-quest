using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games
{
    /// <summary>
    /// Is used like ApplicationService for Game
    /// </summary>
    public interface IGameAppService : IApplicationService
    {
        /// <summary>
        /// Is used to retrieve all entities (Games) like PagedResult
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllPagedResultOutput<GameDto, long> RetrieveAllPagedResult(RetrieveAllGamesPagedResultInput input);

        /// <summary>
        /// Is used to retrieve all entities (Games) like ComboBoxes
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllGamesLikeComboBoxesOutput RetrieveAllGamesLikeComboBoxes(RetrieveAllGamesLikeComboBoxesInput input);

        /// <summary>
        /// Is used to retrieve all entities (Games)
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveAllOutput<GameDto, long> RetrieveAll(RetrieveAllGamesInput input);

        /// <summary>
        /// Is used to retrieve entity (Game) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        RetrieveOutput<GameDto, long> Retrieve(RetrieveGameInput input);

        /// <summary>
        /// Is used to create entity (Game) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        CreateOutput<GameDto, long> Create(CreateGameInput input);

        /// <summary>
        /// Is used to update entity (Game) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        UpdateOutput<GameDto, long> Update(UpdateGameInput input);

        /// <summary>
        /// Is used to delete entity (Game) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        DeleteOutput<long> Delete(DeleteInput<long> input);

        /// <summary>
        /// Is used to change activity for entity (Game) 
        /// </summary>
        /// <param name="input">object with input params</param>
        /// <returns>object with output params</returns>
        ChangeActivityOutput<GameDto, long> ChangeActivity(ChangeActivityInput input);
    }
}