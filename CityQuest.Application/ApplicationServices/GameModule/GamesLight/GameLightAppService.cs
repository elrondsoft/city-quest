using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Statistics.PlayerGameTaskStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Events.Messages;
using CityQuest.Events.Notifiers;
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
    [Abp.Authorization.AbpAuthorize]
    public class GameLightAppService : IGameLightAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private ICityQuestSession Session { get; set; }
        private IUserRepository UserRepository { get; set; }
        private IGameRepository GameRepository { get; set; }
        private ICityQuestRepositoryBase<PlayerGameTaskStatistic, long> PlayerGameTaskStatisticRepository { get; set; }
        private ICityQuestRepositoryBase<TeamGameTaskStatistic, long> TeamGameTaskStatisticRepository { get; set; }
        private IConditionRepository ConditionRepository { get; set; }
        private ISuccessfullPlayerAttemptRepository SuccessfullPlayerAttemptRepository { get; set; }
        private IUnsuccessfullPlayerAttemptRepository UnsuccessfullPlayerAttemptRepository { get; set; }
        private IKeyRepository KeyRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }
        private IStatisticsChangesNotifier StatisticsChangesNotifier { get; set; }

        #endregion

        #region ctors

        public GameLightAppService(
            IUnitOfWorkManager uowManager,
            ICityQuestSession session,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            ICityQuestRepositoryBase<PlayerGameTaskStatistic, long> playerGameTaskStatisticRepository,
            ICityQuestRepositoryBase<TeamGameTaskStatistic, long> teamGameTaskStatisticRepository,
            IConditionRepository conditionRepository,
            ISuccessfullPlayerAttemptRepository successfullPlayerAttemptRepository,
            IUnsuccessfullPlayerAttemptRepository unsuccessfullPlayerAttemptRepository,
            IKeyRepository keyRepository,
            IGamePolicy gamePolicy,
            IStatisticsChangesNotifier statisticsChangesNotifier)
        {
            UowManager = uowManager;
            Session = session;
            UserRepository = userRepository;
            GameRepository = gameRepository;
            PlayerGameTaskStatisticRepository = playerGameTaskStatisticRepository;
            TeamGameTaskStatisticRepository = teamGameTaskStatisticRepository;
            ConditionRepository = conditionRepository;
            SuccessfullPlayerAttemptRepository = successfullPlayerAttemptRepository;
            UnsuccessfullPlayerAttemptRepository = unsuccessfullPlayerAttemptRepository;
            KeyRepository = keyRepository;
            GamePolicy = gamePolicy;
            StatisticsChangesNotifier = statisticsChangesNotifier;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
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

        [Abp.Authorization.AbpAuthorize]
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

        [Abp.Authorization.AbpAuthorize]
        public RetrieveGameLightTasksOutput RetrieveGameLightTasks(RetrieveGameLightTasksInput input)
        {
            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.GameTasks.Select(e => e.GameTaskType));
            GameRepository.Includes.Add(r => r.GameTasks.Select(e => e.Conditions));
            GameRepository.Includes.Add(r => r.GameTasks.Select(e => e.Tips));
            GameRepository.Includes.Add(r => r.GameTasks.Select(e => e.Conditions.Select(k => k.ConditionType)));

            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null || !GamePolicy.CanRetrieveEntityLight(gameEntity))
            {
                return new RetrieveGameLightTasksOutput();
            }

            IList<GameTaskLightDto> gameTasks = gameEntity.GameTasks
                .Where(r => r.IsActive)
                .OrderBy(r => r.Order)
                .ToList().MapIList<GameTask, GameTaskLightDto>().ToList();

            UserRepository.Includes.Add(r => r.PlayerCareers);

            long? currentUserTeamId = UserRepository.Get(Session.UserId ?? 0).CurrentPlayerTeamId;

            if (currentUserTeamId == null)
            {
                return new RetrieveGameLightTasksOutput()
                {
                    GameTasks = gameTasks
                };
            }

            TeamGameTaskStatisticRepository.Includes.Add(r => r.GameTask);

            IList<long> completedGameTaskIds = TeamGameTaskStatisticRepository.GetAll()
                .Where(r => r.GameTask.GameId == input.GameId && r.TeamId == currentUserTeamId)
                .Select(r => r.GameTaskId)
                .ToList();

            long? inProgressGameTaskId = gameTasks.Where(r => !completedGameTaskIds.Contains(r.Id)).Count() > 0 ? 
                (long?)gameTasks.Where(r => !completedGameTaskIds.Contains(r.Id)).OrderBy(r => r.Order).First().Id : null;

            IList<GameTaskLightDto> availableGameTasks = gameTasks
                .Where(r => completedGameTaskIds.Contains(r.Id) || r.Id == inProgressGameTaskId)
                .OrderBy(r => r.Order)
                .ToList();

            TeamGameTaskStatisticRepository.Includes.Clear();
            UserRepository.Includes.Clear();
            GameRepository.Includes.Clear();

            return new RetrieveGameLightTasksOutput()
                {
                    GameTasks = availableGameTasks,
                    CompletedGameTaskIds = completedGameTaskIds,
                    InProgressGameTaskId = inProgressGameTaskId
                };
        }

        [Abp.Authorization.AbpAuthorize]
        public TryToPassConditionOutput TryToPassCondition(TryToPassConditionInput input)
        {
            bool result = false;

            #region Retrieving current player career

            UserRepository.Includes.Add(r => r.PlayerCareers);
            UserRepository.Includes.Add(r => r.PlayerCareers.Select(e => e.Team.PlayerCareers));

            PlayerCareer currCareer = UserRepository.Get(Session.UserId ?? 0)
                .PlayerCareers.Where(r => r.CareerDateEnd == null).SingleOrDefault();

            UserRepository.Includes.Clear();

            #endregion

            if (currCareer != null)
            {
                #region Retrieving current condition

                ConditionRepository.Includes.Add(r => r.GameTask.Game);

                Condition currentCondition = ConditionRepository.Get(input.ConditionId);
                if (currentCondition == null || currentCondition.GameTask == null || currentCondition.GameTask.Game == null ||
                    !GamePolicy.CanRetrieveEntityLight(currentCondition.GameTask.Game))
                {
                    //throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Game\"");
                    return new TryToPassConditionOutput() { Result = result };
                }

                ConditionRepository.Includes.Clear();

                #endregion

                IList<long> teamPlayerCareerIds = currCareer.Team.PlayerCareers.Select(e => e.Id).ToList();
                bool alreadyPassedThisCondition = TeamGameTaskStatisticRepository.GetAll()
                    .Where(r => r.TeamId == currCareer.TeamId && r.GameTaskId == currentCondition.GameTaskId).Count() != 0 || 
                    PlayerGameTaskStatisticRepository.GetAll()
                    .Where(r => teamPlayerCareerIds.Contains(r.PlayerCareerId) && r.GameTaskId == currentCondition.GameTaskId).Count() != 0;
                if (!alreadyPassedThisCondition)
                {
                    result = HandleAttempt(currentCondition, currCareer, input.Value);
                }
            }

            return new TryToPassConditionOutput() 
            { 
                Result = result 
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveGameTaskResultsOutput RetrieveGameTaskResults(RetrieveGameTaskResultsInput input)
        {
            Game gameEntity = GameRepository.FirstOrDefault(r => r.Id == input.GameId);

            if (gameEntity == null || !GamePolicy.CanRetrieveEntityLight(gameEntity))
            {
                return new RetrieveGameTaskResultsOutput() { TeamGameTaskStatistics = new List<TeamGameTaskStatisticDto>() };
            }

            TeamGameTaskStatisticRepository.Includes.Add(r => r.Team);
            TeamGameTaskStatisticRepository.Includes.Add(r => r.GameTask);

            IList<TeamGameTaskStatisticDto> teamGameTaskStatistics = TeamGameTaskStatisticRepository.GetAll()
                .Where(r => r.GameTask.GameId == input.GameId)
                .ToList()
                .MapIList<TeamGameTaskStatistic, TeamGameTaskStatisticDto>();

            TeamGameTaskStatisticRepository.Includes.Clear();

            return new RetrieveGameTaskResultsOutput()
                {
                    TeamGameTaskStatistics = teamGameTaskStatistics
                };
        }

        #region Helpers

        private bool HandleAttempt(Condition condition, PlayerCareer playerCareer, string inputedValue)
        {
            var attemptResult = false;
            DateTime attemptDateTime = DateTime.Now;
            if (String.Equals(inputedValue, condition.ValueToPass))
            {
                attemptResult = true;

                SuccessfulPlayerAttempt newSuccessfullAttempt = new SuccessfulPlayerAttempt()
                {
                    AttemptDateTime = attemptDateTime,
                    ConditionId = condition.Id,
                    InputedValue = inputedValue,
                    PlayerCareerId = playerCareer.Id
                };
                SuccessfullPlayerAttemptRepository.Insert(newSuccessfullAttempt);

                IList<long> userCompleterIds = new List<long>();
                
                #region Creating Statistic entities

                IList<PlayerGameTaskStatistic> newPlayerGameTaskStatistics = new List<PlayerGameTaskStatistic>();
                foreach (var item in playerCareer.Team.PlayerCareers)
                {
                    newPlayerGameTaskStatistics.Add(
                        new PlayerGameTaskStatistic()
                            {
                                GameTaskId = condition.GameTaskId,
                                PlayerCareerId = item.Id,
                                GameTaskStartDateTime = attemptDateTime,
                                GameTaskEndDateTime = attemptDateTime,
                                GameTaskDurationInTicks = 0
                            });
                    userCompleterIds.Add(item.UserId);
                }
                PlayerGameTaskStatisticRepository.AddRange(newPlayerGameTaskStatistics);

                TeamGameTaskStatistic newTeamGameTaskStatistic = new TeamGameTaskStatistic()
                    {
                        GameTaskId = condition.GameTaskId,
                        TeamId = playerCareer.TeamId,
                        GameTaskStartDateTime = attemptDateTime,
                        GameTaskEndDateTime = attemptDateTime,
                        GameTaskDurationInTicks = 0
                    };
                TeamGameTaskStatisticRepository.Insert(newTeamGameTaskStatistic);

                #endregion

                UowManager.Current.Completed += (sender, e) =>
                    {
                        StatisticsChangesNotifier.RaiseOnGameTaskCompleted(new GameTaskCompletedMessage(condition.GameTaskId, userCompleterIds));
                    };
            }
            else
            {
                UnsuccessfulPlayerAttempt newUnsuccessfulAttempt = new UnsuccessfulPlayerAttempt()
                {
                    AttemptDateTime = attemptDateTime,
                    ConditionId = condition.Id,
                    InputedValue = inputedValue,
                    PlayerCareerId = playerCareer.Id
                };
                UnsuccessfullPlayerAttemptRepository.Insert(newUnsuccessfulAttempt);
            }
            return attemptResult;
        }

        #endregion
    }
}
