using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.CityQuestPolicy.GameModule.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTaskTypes
{
    public class GameTaskTypeAppService : IGameTaskTypeAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IGameTaskTypeRepository GameTaskTypeRepository { get; set; }
        private IGameTaskTypePolicy GameTaskTypePolicy { get; set; }

        #endregion

        #region ctors

        public GameTaskTypeAppService(IUnitOfWorkManager uowManager,
            IGameTaskTypeRepository gameTaskTypeRepository,
            IGameTaskTypePolicy gameTaskTypePolicy)
        {
            UowManager = uowManager;
            GameTaskTypeRepository = gameTaskTypeRepository;
            GameTaskTypePolicy = gameTaskTypePolicy;
        }

        #endregion

        public RetrieveAllPagedResultOutput<GameTaskTypeDto, long> RetrieveAllPagedResult(
            RetrieveAllGameTaskTypesPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            IQueryable<GameTaskType> gameTaskTypesQuery = GameTaskTypePolicy.CanRetrieveManyEntities(
                GameTaskTypeRepository.GetAll()
                .WhereIf(!input.GameTaskTypeIds.IsNullOrEmpty(), r => input.GameTaskTypeIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = gameTaskTypesQuery.Count();
            IReadOnlyList<GameTaskTypeDto> gameTaskTypeDtos = gameTaskTypesQuery
                .OrderByDescending(r => r.IsDefault).ThenByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<GameTaskType, GameTaskTypeDto>().ToList();

            GameTaskTypeRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<GameTaskTypeDto, long>()
            {
                Items = gameTaskTypeDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllGameTaskTypesLikeComboBoxesOutput RetrieveAllGameTaskTypesLikeComboBoxes(
            RetrieveAllGameTaskTypesLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> gameTaskTypesLikeComboBoxes = GameTaskTypePolicy.CanRetrieveManyEntities(
                GameTaskTypeRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name))
                .OrderBy(r => r.DisplayText).ToList();

            return new RetrieveAllGameTaskTypesLikeComboBoxesOutput()
            {
                Items = gameTaskTypesLikeComboBoxes
            };
        }

        public RetrieveAllOutput<GameTaskTypeDto, long> RetrieveAll(RetrieveAllGameTaskTypeInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            IList<GameTaskType> gameTaskTypeEntities = GameTaskTypePolicy.CanRetrieveManyEntities(
                GameTaskTypeRepository.GetAll()
                .WhereIf(!input.GameTaskTypeIds.IsNullOrEmpty(), r => input.GameTaskTypeIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<GameTaskTypeDto> result = gameTaskTypeEntities.MapIList<GameTaskType, GameTaskTypeDto>();

            GameTaskTypeRepository.Includes.Clear();

            return new RetrieveAllOutput<GameTaskTypeDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveOutput<GameTaskTypeDto, long> Retrieve(RetrieveGameTaskTypeInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            IList<GameTaskType> gameTaskTypeEntities = GameTaskTypeRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (gameTaskTypeEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTaskType\"");

            if (!GameTaskTypePolicy.CanRetrieveEntity(gameTaskTypeEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"GameTaskType\"");

            GameTaskTypeDto gameTaskTypeEntity = gameTaskTypeEntities.Single().MapTo<GameTaskTypeDto>();

            GameTaskTypeRepository.Includes.Clear();

            return new RetrieveOutput<GameTaskTypeDto, long>()
            {
                RetrievedEntity = gameTaskTypeEntity
            };
        }

        public CreateOutput<GameTaskTypeDto, long> Create(CreateInput<GameTaskTypeDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTaskType newGameTaskTypeEntity = input.Entity.MapTo<GameTaskType>();

            newGameTaskTypeEntity.IsDefault = false;
            newGameTaskTypeEntity.IsActive = true;

            if (!GameTaskTypePolicy.CanCreateEntity(newGameTaskTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"GameTaskType\"");

            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            GameTaskTypeDto newGameTaskTypeDto = (GameTaskTypeRepository.Insert(newGameTaskTypeEntity)).MapTo<GameTaskTypeDto>();

            GameTaskTypeRepository.Includes.Clear();

            return new CreateOutput<GameTaskTypeDto, long>()
            {
                CreatedEntity = newGameTaskTypeDto
            };
        }

        public UpdateOutput<GameTaskTypeDto, long> Update(UpdateInput<GameTaskTypeDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTaskType newGameTaskTypeEntity = input.Entity.MapTo<GameTaskType>();

            if (newGameTaskTypeEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTaskType\"");

            if (!GameTaskTypePolicy.CanUpdateEntity(newGameTaskTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"GameTaskType\"");

            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            GameTaskTypeRepository.Update(newGameTaskTypeEntity);
            GameTaskTypeDto newGameTaskTypeDto = (GameTaskTypeRepository.Get(newGameTaskTypeEntity.Id)).MapTo<GameTaskTypeDto>();

            GameTaskTypeRepository.Includes.Clear();

            return new UpdateOutput<GameTaskTypeDto, long>()
            {
                UpdatedEntity = newGameTaskTypeDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            GameTaskType gameTaskTypeEntityForDelete = GameTaskTypeRepository.Get(input.EntityId);

            if (gameTaskTypeEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTaskType\"");

            if (!GameTaskTypePolicy.CanDeleteEntity(gameTaskTypeEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"GameTaskType\"");

            GameTaskTypeRepository.Delete(gameTaskTypeEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        public ChangeActivityOutput<GameTaskTypeDto, long> ChangeActivity(ChangeActivityInput input)
        {
            GameTaskTypeRepository.Includes.Add(r => r.LastModifierUser);
            GameTaskTypeRepository.Includes.Add(r => r.CreatorUser);

            GameTaskType gameTaskTypeEntity = GameTaskTypeRepository.Get(input.EntityId);

            if (gameTaskTypeEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"GameTaskType\"");

            if (!GameTaskTypePolicy.CanChangeActivityForEntity(gameTaskTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeActivityDenied, "\"GameTaskType\"");

            gameTaskTypeEntity.IsActive = input.IsActive == null ? !gameTaskTypeEntity.IsActive : (bool)input.IsActive;

            GameTaskTypeDto newGameTaskTypeDto = (gameTaskTypeEntity).MapTo<GameTaskTypeDto>();

            GameTaskTypeRepository.Update(gameTaskTypeEntity);

            GameTaskTypeRepository.Includes.Clear();

            return new ChangeActivityOutput<GameTaskTypeDto, long>()
            {
                Entity = newGameTaskTypeDto
            };
        }
    }
}
