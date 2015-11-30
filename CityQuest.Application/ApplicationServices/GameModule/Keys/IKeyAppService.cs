using Abp.Application.Services;
using CityQuest.ApplicationServices.GameModule.Keys.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Keys
{
    /// <summary>
    /// Is used like ApplicationService for Key
    /// </summary>
    public interface IKeyAppService : IApplicationService
    {
        /// <summary>
        /// Is used to generate new Keys for Game
        /// </summary>
        /// <param name="input">input parameter</param>
        /// <returns>output parameter</returns>
        GenerateKeysForGameOutput GenerateKeysForGame(GenerateKeysForGameInput input);

        /// <summary>
        /// Is used to try activate Key 
        /// </summary>
        /// <param name="input">input parameter</param>
        /// <returns>output parameter</returns>
        ActivateKeyOutput ActivateKey(ActivateKeyInput input);
    }
}
