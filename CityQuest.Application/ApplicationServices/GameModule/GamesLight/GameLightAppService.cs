using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Exceptions;
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
        private IUserRepository UserRepository { get; set; }
        private IGameRepository GameRepository { get; set; }
        private IConditionRepository ConditionRepository { get; set; }
        private ISuccessfulPlayerAttemptRepository SuccessfulPlayerAttemptRepository { get; set; }
        private IUnsuccessfulPlayerAttemptRepository UnsuccessfulPlayerAttemptRepository { get; set; }
        private IKeyRepository KeyRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }

        #endregion

        #region ctors

        public GameLightAppService(
            ICityQuestSession session,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IConditionRepository conditionRepository,
            ISuccessfulPlayerAttemptRepository successfulPlayerAttemptRepository,
            IUnsuccessfulPlayerAttemptRepository unsuccessfulPlayerAttemptRepository,
            IKeyRepository keyRepository,
            IGamePolicy gamePolicy)
        {
            Session = session;
            UserRepository = userRepository;
            GameRepository = gameRepository;
            ConditionRepository = conditionRepository;
            SuccessfulPlayerAttemptRepository = successfulPlayerAttemptRepository;
            UnsuccessfulPlayerAttemptRepository = unsuccessfulPlayerAttemptRepository;
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
            KeyRepository.Includes.Add(r => r.Game.GameTasks.Select(e => e.GameTaskType));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Conditions));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Conditions.Select(k => k.ConditionType)));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Tips));

            List<GameLightDto> gameCollection = GamePolicy.CanRetrieveManyEntitiesLight(
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
            KeyRepository.Includes.Add(r => r.Game);
            KeyRepository.Includes.Add(r => r.Game.Location);
            KeyRepository.Includes.Add(r => r.Game.GameStatus);
            KeyRepository.Includes.Add(r => r.Game.GameTasks);
            KeyRepository.Includes.Add(r => r.Game.GameTasks.Select(e => e.GameTaskType));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Conditions));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Conditions.Select(k => k.ConditionType)));
            //KeyRepository.Includes.Add(r => r.Game.GameTasks.SelectMany(e => e.Tips));

            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null || !GamePolicy.CanRetrieveEntityLight(gameEntity))
            {
                return new RetrieveGameLightOutput();
            }

            GameLightDto game = gameEntity.MapTo<GameLightDto>();

            KeyRepository.Includes.Clear();

            return new RetrieveGameLightOutput()
            {
                Game = game
            };
        }

        public RetrieveGameResultsAndTasksOutput RetrieveGameResultsAndTasks(RetrieveGameResultsAndTasksInput input)
        {
            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null || !GamePolicy.CanRetrieveEntityLight(gameEntity))
            {
                return new RetrieveGameResultsAndTasksOutput();
            }

            // TODO:

            return new RetrieveGameResultsAndTasksOutput();
        }

        public TryToPassConditionOutput TryToPassCondition(TryToPassConditionInput input)
        {
            bool result = false;

            UserRepository.Includes.Add(r => r.PlayerCareers);

            PlayerCareer currCareer = UserRepository.Get(Session.UserId ?? 0)
                .PlayerCareers.Where(r => r.CareerDateEnd == null).SingleOrDefault();

            UserRepository.Includes.Clear();

            if (currCareer != null)
            {
                ConditionRepository.Includes.Add(r => r.GameTask.Game);

                Condition currentCondition = ConditionRepository.Get(input.ConditionId);
                if (currentCondition == null || currentCondition.GameTask == null || currentCondition.GameTask.Game == null ||
                    !GamePolicy.CanRetrieveEntityLight(currentCondition.GameTask.Game))
                {
                    //throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Game\"");
                    return new TryToPassConditionOutput() { Result = false };
                }

                ConditionRepository.Includes.Clear();

                result = String.Equals(input.Value, currentCondition.ValueToPass);
                if (result)
                {
                    SuccessfulPlayerAttempt newSuccessfulAttempt = new SuccessfulPlayerAttempt()
                    {
                        AttemptDateTime = DateTime.Now,
                        ConditionId = currentCondition.Id,
                        InputedValue = input.Value,
                        PlayerCareerId = currCareer.Id
                    };
                    SuccessfulPlayerAttemptRepository.Insert(newSuccessfulAttempt);
#warning TODO: send notification!
                }
                else
                {
                    UnsuccessfulPlayerAttempt newUnsuccessfulAttempt = new UnsuccessfulPlayerAttempt()
                    {
                        AttemptDateTime = DateTime.Now,
                        ConditionId = currentCondition.Id,
                        InputedValue = input.Value,
                        PlayerCareerId = currCareer.Id
                    };
                    UnsuccessfulPlayerAttemptRepository.Insert(newUnsuccessfulAttempt);
                }
            }

            return new TryToPassConditionOutput() 
            { 
                Result = result 
            };
        }
    }
}
