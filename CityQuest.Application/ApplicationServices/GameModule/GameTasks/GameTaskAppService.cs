using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.CityQuestPolicy.GameModule.GameTasks;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTasks
{
    public class GameTaskAppService : IGameTaskAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IGameRepository GameRepository { get; set; }
        private IGameTaskRepository GameTaskRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }
        private IGameTaskPolicy GameTaskPolicy { get; set; }

        #endregion

        #region ctors

        public GameTaskAppService(IUnitOfWorkManager uowManager, 
            IGameRepository gameRepository, 
            IGameTaskRepository gameTaskRepository, 
            IGamePolicy gamePolicy,
            IGameTaskPolicy gameTaskPolicy)
        {
            UowManager = uowManager;
            GameRepository = gameRepository;
            GameTaskRepository = gameTaskRepository;
            GamePolicy = gamePolicy;
            GameTaskPolicy = gameTaskPolicy;
        }

        #endregion

        public RetrieveAllPagedResultOutput<GameTaskDto, long> RetrieveAllPagedResult(RetrieveAllGameTasksPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            IQueryable<GameTask> gameTasksQuery = GameTaskPolicy.CanRetrieveManyEntities(
                GameTaskRepository.GetAll()
                .WhereIf(!input.GameTaskIds.IsNullOrEmpty(), r => input.GameTaskIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = gameTasksQuery.Count();
            IReadOnlyList<GameTaskDto> gameTaskDtos = gameTasksQuery
                .OrderBy(r => r.Order).ThenByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<GameTask, GameTaskDto>().ToList();

            GameTaskRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<GameTaskDto, long>()
            {
                Items = gameTaskDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllGameTasksLikeComboBoxesOutput RetrieveAllGameTasksLikeComboBoxes(RetrieveAllGameTasksLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> gameTasksLikeComboBoxes = GameTaskPolicy.CanRetrieveManyEntities(
                GameTaskRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllGameTasksLikeComboBoxesOutput()
            {
                Items = gameTasksLikeComboBoxes
            };
        }

        public RetrieveAllOutput<GameTaskDto, long> RetrieveAll(RetrieveAllGameTaskInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            IList<GameTask> gameTaskEntities = GameTaskPolicy.CanRetrieveManyEntities(
                GameTaskRepository.GetAll()
                .WhereIf(!input.GameTaskIds.IsNullOrEmpty(), r => input.GameTaskIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<GameTaskDto> result = gameTaskEntities.MapIList<GameTask, GameTaskDto>();

            GameTaskRepository.Includes.Clear();

            return new RetrieveAllOutput<GameTaskDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveOutput<GameTaskDto, long> Retrieve(RetrieveGameTaskInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            IList<GameTask> gameTaskEntities = GameTaskRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (gameTaskEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTask\"");

            if (!GameTaskPolicy.CanRetrieveEntity(gameTaskEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"GameTask\"");

            GameTaskDto gameTaskEntity = gameTaskEntities.Single().MapTo<GameTaskDto>();

            GameTaskRepository.Includes.Clear();

            return new RetrieveOutput<GameTaskDto, long>()
            {
                RetrievedEntity = gameTaskEntity
            };
        }

        public CreateOutput<GameTaskDto, long> Create(CreateInput<GameTaskDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTask newGameTaskEntity = input.Entity.MapTo<GameTask>();

            newGameTaskEntity.IsActive = true;

            if (!GameTaskPolicy.CanCreateEntity(newGameTaskEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"GameTask\"");

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            GameTaskDto newGameTaskDto = (GameTaskRepository.Insert(newGameTaskEntity)).MapTo<GameTaskDto>();

            GameTaskRepository.Includes.Clear();

            return new CreateOutput<GameTaskDto, long>()
            {
                CreatedEntity = newGameTaskDto
            };
        }

        public UpdateOutput<GameTaskDto, long> Update(UpdateInput<GameTaskDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTask newGameTaskEntity = input.Entity.MapTo<GameTask>();

            if (newGameTaskEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTask\"");

            if (!GameTaskPolicy.CanUpdateEntity(newGameTaskEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"GameTask\"");

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            GameTaskRepository.Update(newGameTaskEntity);
            GameTaskDto newGameTaskDto = (GameTaskRepository.Get(newGameTaskEntity.Id)).MapTo<GameTaskDto>();

            GameTaskRepository.Includes.Clear();

            return new UpdateOutput<GameTaskDto, long>()
            {
                UpdatedEntity = newGameTaskDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTask gameTaskEntityForDelete = GameTaskRepository.Get(input.EntityId);

            if (gameTaskEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTask\"");

            if (!GameTaskPolicy.CanDeleteEntity(gameTaskEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"GameTask\"");

            GameTaskRepository.Delete(gameTaskEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        public ChangeActivityOutput<GameTaskDto, long> ChangeActivity(ChangeActivityInput input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTaskRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskRepository.Includes.Add(r => r.CreatorUser);
            GameTaskRepository.Includes.Add(r => r.GameTaskType);
            GameTaskRepository.Includes.Add(r => r.Conditions);
            GameTaskRepository.Includes.Add(r => r.Tips);

            GameTask gameTaskEntity = GameTaskRepository.Get(input.EntityId);

            if (gameTaskEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTask\"");

            if (!GameTaskPolicy.CanChangeActivityForEntity(gameTaskEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeActivityDenied, "\"GameTask\"");

            gameTaskEntity.IsActive = input.IsActive == null ? !gameTaskEntity.IsActive : (bool)input.IsActive;

            GameTaskDto newGameTaskDto = (gameTaskEntity).MapTo<GameTaskDto>();

            GameTaskRepository.Update(gameTaskEntity);

            GameTaskRepository.Includes.Clear();

            return new ChangeActivityOutput<GameTaskDto, long>()
            {
                Entity = newGameTaskDto
            };
        }

        public RetrieveGameTasksForGameOutput RetrieveGameTasksForGame(RetrieveGameTasksForGameInput input)
        {
            if (!(input.GameId > 0))
                return new RetrieveGameTasksForGameOutput() { GameTasks = new List<GameTaskDto>() };

            GameRepository.Includes.Add(r => r.GameTasks);

            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanRetrieveEntity(gameEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Game\"");

            IList<GameTask> gameTasksEntities = gameEntity.GameTasks.ToList();
            IList<GameTaskDto> gameTasksForGame = gameTasksEntities.MapIList<GameTask, GameTaskDto>();

            GameRepository.Includes.Clear();

            return new RetrieveGameTasksForGameOutput()
            {
                GameTasks = gameTasksForGame
            };
        }
    }
}
