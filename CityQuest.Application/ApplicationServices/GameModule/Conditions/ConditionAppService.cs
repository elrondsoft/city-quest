using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestPolicy.GameModule.Conditions;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Conditions
{
    [Abp.Authorization.AbpAuthorize]
    public class ConditionAppService : IConditionAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IConditionRepository ConditionRepository { get; set; }
        private IGameTaskRepository GameTaskRepository { get; set; }
        private IConditionPolicy ConditionPolicy { get; set; }
        private IGamePolicy GamePolicy { get; set; }

        #endregion

        #region ctors

        public ConditionAppService(IUnitOfWorkManager uowManager,
            IConditionRepository conditionRepository,
            IGameTaskRepository gameTaskRepository,
            IConditionPolicy conditionPolicy,
            IGamePolicy gamePolicy)
        {
            UowManager = uowManager;
            ConditionRepository = conditionRepository;
            GameTaskRepository = gameTaskRepository;
            ConditionPolicy = conditionPolicy;
            GamePolicy = gamePolicy;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllPagedResultOutput<ConditionDto, long> RetrieveAllPagedResult(
            RetrieveAllConditionsPagedResultInput input)
        {
            ConditionRepository.Includes.Add(r => r.LastModifierUser);
            ConditionRepository.Includes.Add(r => r.CreatorUser);
            ConditionRepository.Includes.Add(r => r.ConditionType);

            IQueryable<Condition> conditionsQuery = ConditionPolicy.CanRetrieveManyEntities(
                ConditionRepository.GetAll()
                .WhereIf(!input.ConditionIds.IsNullOrEmpty(), r => input.ConditionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .OrderBy(r => r.Order);

            int totalCount = conditionsQuery.Count();
            IReadOnlyList<ConditionDto> conditionDtos = conditionsQuery
                .OrderBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Condition, ConditionDto>().ToList();

            ConditionRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<ConditionDto, long>()
            {
                Items = conditionDtos,
                TotalCount = totalCount
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllConditionsLikeComboBoxesOutput RetrieveAllConditionsLikeComboBoxes(
            RetrieveAllConditionsLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> conditionsLikeComboBoxes = ConditionPolicy.CanRetrieveManyEntities(
                ConditionRepository.GetAll()).ToList().OrderBy(r => r.Order)
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllConditionsLikeComboBoxesOutput()
            {
                Items = conditionsLikeComboBoxes
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<ConditionDto, long> RetrieveAll(RetrieveAllConditionInput input)
        {
            ConditionRepository.Includes.Add(r => r.LastModifierUser);
            ConditionRepository.Includes.Add(r => r.CreatorUser);
            ConditionRepository.Includes.Add(r => r.ConditionType);

            IList<Condition> conditionEntities = ConditionPolicy.CanRetrieveManyEntities(
                ConditionRepository.GetAll()
                .WhereIf(!input.ConditionIds.IsNullOrEmpty(), r => input.ConditionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .OrderBy(r => r.Order)
                .ToList();

            IList<ConditionDto> result = conditionEntities.MapIList<Condition, ConditionDto>();

            ConditionRepository.Includes.Clear();

            return new RetrieveAllOutput<ConditionDto, long>()
            {
                RetrievedEntities = result
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<ConditionDto, long> Retrieve(RetrieveConditionInput input)
        {
            ConditionRepository.Includes.Add(r => r.LastModifierUser);
            ConditionRepository.Includes.Add(r => r.CreatorUser);
            ConditionRepository.Includes.Add(r => r.ConditionType);

            IList<Condition> conditionEntities = ConditionRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (conditionEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Condition\"");

            if (!ConditionPolicy.CanRetrieveEntity(conditionEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Condition\"");

            ConditionDto conditionEntity = conditionEntities.Single().MapTo<ConditionDto>();

            ConditionRepository.Includes.Clear();

            return new RetrieveOutput<ConditionDto, long>()
            {
                RetrievedEntity = conditionEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<ConditionDto, long> Create(CreateInput<ConditionDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            Condition newConditionEntity = input.Entity.MapTo<Condition>();

            if (!ConditionPolicy.CanCreateEntity(newConditionEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"Condition\"");

            ConditionRepository.Includes.Add(r => r.LastModifierUser);
            ConditionRepository.Includes.Add(r => r.CreatorUser);
            ConditionRepository.Includes.Add(r => r.ConditionType);

            ConditionDto newConditionDto = (ConditionRepository.Insert(newConditionEntity)).MapTo<ConditionDto>();

            ConditionRepository.Includes.Clear();

            return new CreateOutput<ConditionDto, long>()
            {
                CreatedEntity = newConditionDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<ConditionDto, long> Update(UpdateInput<ConditionDto, long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            Condition newConditionEntity = input.Entity.MapTo<Condition>();

            if (newConditionEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Condition\"");

            if (!ConditionPolicy.CanUpdateEntity(newConditionEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"Condition\"");

            ConditionRepository.Includes.Add(r => r.LastModifierUser);
            ConditionRepository.Includes.Add(r => r.CreatorUser);
            ConditionRepository.Includes.Add(r => r.ConditionType);

            ConditionRepository.Update(newConditionEntity);
            ConditionDto newConditionDto = (ConditionRepository.Get(newConditionEntity.Id)).MapTo<ConditionDto>();

            ConditionRepository.Includes.Clear();

            return new UpdateOutput<ConditionDto, long>()
            {
                UpdatedEntity = newConditionDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            throw new NotSupportedException("This method is implemented but it is not safely to use it.");

            Condition conditionEntityForDelete = ConditionRepository.Get(input.EntityId);

            if (conditionEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Condition\"");

            if (!ConditionPolicy.CanDeleteEntity(conditionEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"Condition\"");

            ConditionRepository.Delete(conditionEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveConditionsForGameTaskOutput RetrieveConditionsForGameTask(RetrieveConditionsForGameTaskInput input)
        {
            if (!(input.GameTaskId > 0))
                return new RetrieveConditionsForGameTaskOutput() { Conditions = new List<ConditionDto>() };

            GameTaskRepository.Includes.Add(r => r.Game);
            GameTaskRepository.Includes.Add(r => r.Conditions);

            GameTask gameTaskEntity = GameTaskRepository.Get(input.GameTaskId);
            Game gameEntity = gameTaskEntity.Game;

            if (gameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanRetrieveEntity(gameEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Game\"");

            IList<Condition> conditionEntities = gameTaskEntity.Conditions.ToList();
            IList<ConditionDto> conditionsForGameTask = conditionEntities.MapIList<Condition, ConditionDto>();

            GameTaskRepository.Includes.Clear();

            return new RetrieveConditionsForGameTaskOutput()
            {
                Conditions = conditionsForGameTask
            };
        }
    }
}
