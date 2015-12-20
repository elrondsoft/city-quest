using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Mapping;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight
{
    public class GameLightAppService : IGameLightAppService
    {
        #region Injected Dependencies

        private ICityQuestSession Session { get; set; }
        private IGameRepository GameRepository { get; set; }
        private IKeyRepository KeyRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }

        #endregion

        #region ctors

        public GameLightAppService(
            ICityQuestSession session,
            IGameRepository gameRepository,
            IKeyRepository keyRepository,
            IGamePolicy gamePolicy)
        {
            Session = session;
            GameRepository = gameRepository;
            KeyRepository = keyRepository;
            GamePolicy = gamePolicy;
        }

        #endregion

        public RetrieveGameCollectionOutput RetrieveGameCollection(RetrieveGameCollectionInput input)
        {
            if (Session.UserId == null) 
            { 
                return new RetrieveGameCollectionOutput();
            }

            KeyRepository.Includes.Add(r => r.Game);
            KeyRepository.Includes.Add(r => r.Game.Location);
            KeyRepository.Includes.Add(r => r.Game.GameStatus);
            KeyRepository.Includes.Add(r => r.Game.GameTasks);

            List<GameLightDto> gameCollection = GamePolicy.CanRetrieveManyEntities(
                KeyRepository.GetAll()
                .Where(r => r.OwnerUserId == Session.UserId).Select(r => r.Game))
                .ToList().MapIList<Game, GameLightDto>().ToList();

            KeyRepository.Includes.Clear();

            return new RetrieveGameCollectionOutput()
            {
                GameCollection = gameCollection 
            };
        }

        public RetrieveGameLightOutput RetrieveGameLight(RetrieveGameLightInput input)
        {
            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null || !GamePolicy.CanRetrieveEntity(gameEntity))
            {
                return new RetrieveGameLightOutput();
            }

            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameTasks);

            GameLightDto game = gameEntity.MapTo<GameLightDto>();

            KeyRepository.Includes.Clear();

            return new RetrieveGameLightOutput()
            {
                Game = game
            };
        }
    }
}
