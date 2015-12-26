using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Games
{
    public interface IGamePolicy : ICityQuestPolicyBase<Game, long>
    {
        bool CanChangeActivityForEntity(long userId, Game entity);
        bool CanChangeActivityForEntity(Game entity);

        bool CanGenerateKeysForEntity(long userId, Game entity);
        bool CanGenerateKeysForEntity(Game entity);

        /// <summary>
        /// Is used to allow game status change operation 
        /// (checking can current game's status be changed to new one or not by inputed user)
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <param name="entity">game's entity</param>
        /// <param name="oldEntityStatus">old game's status</param>
        /// <param name="newEntityStatus">new game's status</param>
        /// <returns>bool result of allowing operation</returns>
        bool CanChangeStatusForEntity(long userId, Game entity, GameStatus oldEntityStatus, GameStatus newEntityStatus);

        /// <summary>
        /// Is used to allow game status change operation 
        /// (checking can current game's status be changed to new one or not by inputed user)
        /// </summary>
        /// <param name="entity">game's entity</param>
        /// <param name="oldEntityStatus">old game's status</param>
        /// <param name="newEntityStatus">new game's status</param>
        /// <returns>bool result of allowing operation</returns>
        bool CanChangeStatusForEntity(Game gameEntity, GameStatus oldEntityStatus, GameStatus newEntityStatus);

        #region Game light policy

        bool CanRetrieveEntityLight(long userId, Game entity);
        bool CanRetrieveEntityLight(Game entity);
        IQueryable<Game> CanRetrieveManyEntitiesLight(long userId, IQueryable<Game> entities);
        IQueryable<Game> CanRetrieveManyEntitiesLight(IQueryable<Game> entities);

        #endregion
    }
}
