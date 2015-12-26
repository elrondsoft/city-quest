using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.UI;
using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using CityQuest.ApplicationServices.GameModule.Tips.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Events.Messages;
using CityQuest.Events.Notifiers;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games
{
    [Abp.Authorization.AbpAuthorize]
    public class GameAppService : IGameAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IGameRepository GameRepository { get; set; }
        private IGameStatusRepository GameStatusRepository { get; set; }
        private IGameTaskRepository GameTaskRepository { get; set; }
        private IConditionRepository ConditionRepository { get; set; }
        private ITipRepository TipRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }
        private IGameChangesNotifier GameChangesNotifier { get; set; }

        #endregion

        #region ctors

        public GameAppService(IUnitOfWorkManager uowManager, 
            IGameRepository gameRepository,
            IGameStatusRepository gameStatusRepository,
            IGameTaskRepository gameTaskRepository,
            IConditionRepository conditionRepository,
            ITipRepository tipRepository,
            IGamePolicy gamePolicy,
            IGameChangesNotifier gameChangesNotifier)
        {
            UowManager = uowManager;
            GameRepository = gameRepository;
            GameStatusRepository = gameStatusRepository;
            GameTaskRepository = gameTaskRepository;
            ConditionRepository = conditionRepository;
            TipRepository = tipRepository;
            GamePolicy = gamePolicy;
            GameChangesNotifier = gameChangesNotifier;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllPagedResultOutput<GameDto, long> RetrieveAllPagedResult(RetrieveAllGamesPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);
            GameRepository.Includes.Add(r => r.GameTasks);

            IQueryable<Game> gamesQuery = GamePolicy.CanRetrieveManyEntities(
                GameRepository.GetAll()
                .WhereIf(!input.GameIds.IsNullOrEmpty(), r => input.GameIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = gamesQuery.Count();
            IReadOnlyList<GameDto> gameDtos = gamesQuery
                .OrderByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Game, GameDto>().ToList();

            GameRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<GameDto, long>()
            {
                Items = gameDtos,
                TotalCount = totalCount
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllGamesLikeComboBoxesOutput RetrieveAllGamesLikeComboBoxes(RetrieveAllGamesLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> gamesLikeComboBoxes = GamePolicy.CanRetrieveManyEntities(
                GameRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name))
                .OrderBy(r => r.DisplayText).ToList();

            return new RetrieveAllGamesLikeComboBoxesOutput()
            {
                Items = gamesLikeComboBoxes
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<GameDto, long> RetrieveAll(RetrieveAllGamesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IList<Game> gameEntities = GamePolicy.CanRetrieveManyEntities( 
                GameRepository.GetAll()
                .WhereIf(!input.GameIds.IsNullOrEmpty(), r => input.GameIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<GameDto> result = gameEntities.MapIList<Game, GameDto>();

            return new RetrieveAllOutput<GameDto, long>()
            {
                RetrievedEntities = result
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<GameDto, long> Retrieve(RetrieveGameInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);

            IList<Game> gameEntities = GameRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (gameEntities.Count != 1) 
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanRetrieveEntity(gameEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Game\"");

            GameDto gameEntity = gameEntities.Single().MapTo<GameDto>();

            GameRepository.Includes.Clear();

            return new RetrieveOutput<GameDto, long>()
            {
                RetrievedEntity = gameEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<GameDto, long> Create(CreateGameInput input)
        {
            Game newGameEntity = input.Entity.MapTo<Game>();

            if (!GamePolicy.CanCreateEntity(newGameEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"Game\"");

            newGameEntity.IsActive = true;
            try
            {
                newGameEntity.GameStatusId = GameStatusRepository.GetAll().Where(r => r.IsDefault).Single().Id;
            }
            catch
            {
                throw new UserFriendlyException("Default game status not found!", 
                    "Default game status not found! Please contact your system administrator.");
            }

            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);

            long newGameId = GameRepository.InsertAndGetId(newGameEntity);

            UowManager.Current.Completed += (sender, e) =>
            {
                GameChangesNotifier.RaiseOnGameAdded(new GameAddedMessage(newGameId));
            };

            GameDto newGameDto = GameRepository.Get(newGameId).MapTo<GameDto>();

            GameRepository.Includes.Clear();

            return new CreateOutput<GameDto, long>()
            {
                CreatedEntity = newGameDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<GameDto, long> Update(UpdateGameInput input)
        {

            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);

            Game existGameEntity = GameRepository.Get(input.Entity.Id);

            if (existGameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanUpdateEntity(existGameEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"Game\"");

            UpdateGameEntity(existGameEntity, input.Entity);

            GameDto newGameDto = (GameRepository.Get(input.Entity.Id)).MapTo<GameDto>();

            UowManager.Current.Completed += (sender, e) =>
            {
                GameChangesNotifier.RaiseOnGameUpdated(new GameUpdatedMessage(newGameDto.Id));
            };

            GameRepository.Includes.Clear();

            return new UpdateOutput<GameDto, long>()
            {
                UpdatedEntity = newGameDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Game gameEntityForDelete = GameRepository.Get(input.EntityId);

            if (gameEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanDeleteEntity(gameEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"Game\"");

            GameRepository.Delete(gameEntityForDelete);

            UowManager.Current.Completed += (sender, e) =>
            {
                GameChangesNotifier.RaiseOnGameDeleted(new GameDeletedMessage(input.EntityId));
            };

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public ChangeActivityOutput<GameDto, long> ChangeActivity(ChangeActivityInput input)
        {
            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);

            Game gameEntity = GameRepository.Get(input.EntityId);

            if (gameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanChangeActivityForEntity(gameEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeActivityDenied, "\"Game\"");

            gameEntity.IsActive = input.IsActive == null ? !gameEntity.IsActive : (bool)input.IsActive;

            GameDto newGameDto = (gameEntity).MapTo<GameDto>();

            GameRepository.Update(gameEntity);

            if (newGameDto.IsActive)
            {
                UowManager.Current.Completed += (sender, e) =>
                {
                    GameChangesNotifier.RaiseOnGameActivated(new GameActivatedMessage(input.EntityId));
                };
            }
            else
            {
                UowManager.Current.Completed += (sender, e) =>
                {
                    GameChangesNotifier.RaiseOnGameDeactivated(new GameDeactivatedMessage(input.EntityId));
                };
            }

            GameRepository.Includes.Clear();

            return new ChangeActivityOutput<GameDto, long>()
            {
                Entity = newGameDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public ChangeGameStatusOutput ChangeGameStatus(ChangeGameStatusInput input)
        {
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameStatus.CreatorUser);
            GameRepository.Includes.Add(r => r.GameStatus.LastModifierUser);

            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            GameStatusDto newGameStatusDto = gameEntity.GameStatus.MapTo<GameStatusDto>();

            if (gameEntity.GameStatusId != input.NewGameStatusId)
            {
                GameStatusRepository.Includes.Add(r => r.CreatorUser);
                GameStatusRepository.Includes.Add(r => r.LastModifierUser);

                GameStatus newGameStatus = GameStatusRepository.Get(input.NewGameStatusId);

                if (!GamePolicy.CanChangeStatusForEntity(gameEntity, gameEntity.GameStatus, newGameStatus))
                    throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeStatusDenied, "\"Game\"");

                gameEntity.GameStatusId = newGameStatus.Id;
                gameEntity.GameStatus = null;
                GameRepository.Update(gameEntity);

                newGameStatusDto = newGameStatus.MapTo<GameStatusDto>();

                UowManager.Current.Completed += (sender, e) =>
                {
                    GameChangesNotifier.RaiseOnGameStatusChanged(
                        new GameStatusChangedMessage(input.GameId, input.NewGameStatusId, newGameStatusDto));
                };

            }

            GameStatusRepository.Includes.Clear();
            GameRepository.Includes.Clear();

            return new ChangeGameStatusOutput()
            {
                NewGameStatus = newGameStatusDto
            };
        }

        #region Helpers

        private Game UpdateGameEntity(Game existGameEntity, GameDto newGameEntity)
        {
            existGameEntity.Name = newGameEntity.Name;
            existGameEntity.Description = newGameEntity.Description;
            existGameEntity.IsActive = newGameEntity.IsActive;

            UpdateGameEntityRelations(existGameEntity, newGameEntity);

            return GameRepository.Update(existGameEntity);
        }

        private void UpdateGameEntityRelations(Game existGameEntity, GameDto newGameEntity)
        {
            IList<GameTask> gameTasksForDelete = existGameEntity.GameTasks.ToList();

            foreach (GameTaskDto newGameTask in newGameEntity.GameTasks)
            {
                if (newGameTask.Id > 0)
                {
                    GameTask existGameTask = existGameEntity.GameTasks.First(r => r.Id == newGameTask.Id);
                    gameTasksForDelete.Remove(existGameTask);
                    UpdateGameTaskEntity(existGameTask, newGameTask);
                }
                else
                {
                    GameTaskRepository.Insert(newGameTask.MapTo<GameTask>());
                }
            }

            GameTaskRepository.RemoveRange(gameTasksForDelete);
        }

        private GameTask UpdateGameTaskEntity(GameTask existGameTask, GameTaskDto newGameTask)
        {
            existGameTask.Description = newGameTask.Description;
            existGameTask.GameTaskTypeId = newGameTask.GameTaskTypeId;
            existGameTask.IsActive = newGameTask.IsActive;
            existGameTask.Name = newGameTask.Name;
            existGameTask.Order = newGameTask.Order;
            existGameTask.TaskText = newGameTask.TaskText;

            UpdateGameTaskEntityRelations(existGameTask, newGameTask);

            return GameTaskRepository.Update(existGameTask);
        }

        private void UpdateGameTaskEntityRelations(GameTask existGameTask, GameTaskDto newGameTask)
        {
            #region Updating Conditions

            IList<Condition> conditionsForDelete = existGameTask.Conditions.ToList();

            foreach (ConditionDto newCondition in newGameTask.Conditions)
            {
                if (newCondition.Id > 0)
                {
                    Condition existCondition = existGameTask.Conditions.First(r => r.Id == newCondition.Id);
                    conditionsForDelete.Remove(existCondition);
                    UpdateConditionEntity(existCondition, newCondition);
                }
                else
                {
                    ConditionRepository.Insert(newCondition.MapTo<Condition>());
                }
            }

            ConditionRepository.RemoveRange(conditionsForDelete);

            #endregion

            #region Updating Tips

            IList<Tip> tipsForDelete = existGameTask.Tips.ToList();

            foreach (TipDto newTip in newGameTask.Tips)
            {
                if (newTip.Id > 0)
                {
                    Tip existTip = existGameTask.Tips.First(r => r.Id == newTip.Id);
                    tipsForDelete.Remove(existTip);
                    UpdateTipEntity(existTip, newTip);
                }
                else
                {
                    TipRepository.Insert(newTip.MapTo<Tip>());
                }
            }

            TipRepository.RemoveRange(tipsForDelete);

            #endregion
        }

        private Condition UpdateConditionEntity(Condition existCondition, ConditionDto newCondition)
        {
            existCondition.Description = newCondition.Description;
            existCondition.ConditionTypeId = newCondition.ConditionTypeId;
            existCondition.Name = newCondition.Name;
            existCondition.Order = newCondition.Order;

            return ConditionRepository.Update(existCondition);
        }

        private Tip UpdateTipEntity(Tip existTip, TipDto newTip)
        {
            existTip.TipText = newTip.TipText;
            existTip.Name = newTip.Name;
            existTip.Order = newTip.Order;

            return TipRepository.Update(existTip);
        }

        #endregion
    }
}