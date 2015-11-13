using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.CityQuestPolicy.GameModule.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.ConditionTypes
{
    public class ConditionTypeAppService : IConditionTypeAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IConditionTypeRepository ConditionTypeRepository { get; set; }
        private IConditionTypePolicy ConditionTypePolicy { get; set; }

        #endregion

        #region ctors

        public ConditionTypeAppService(IUnitOfWorkManager uowManager,
            IConditionTypeRepository conditionTypeRepository,
            IConditionTypePolicy conditionTypePolicy)
        {
            UowManager = uowManager;
            ConditionTypeRepository = conditionTypeRepository;
            ConditionTypePolicy = conditionTypePolicy;
        }

        #endregion

        public RetrieveAllPagedResultOutput<ConditionTypeDto, long> RetrieveAllPagedResult(
            RetrieveAllConditionTypesPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            IQueryable<ConditionType> conditionTypesQuery = ConditionTypePolicy.CanRetrieveManyEntities(
                ConditionTypeRepository.GetAll()
                .WhereIf(!input.ConditionTypeIds.IsNullOrEmpty(), r => input.ConditionTypeIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = conditionTypesQuery.Count();
            IReadOnlyList<ConditionTypeDto> conditionTypeDtos = conditionTypesQuery
                .OrderByDescending(r => r.IsDefault).ThenByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<ConditionType, ConditionTypeDto>().ToList();

            ConditionTypeRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<ConditionTypeDto, long>()
            {
                Items = conditionTypeDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllConditionTypesLikeComboBoxesOutput RetrieveAllConditionTypesLikeComboBoxes(
            RetrieveAllConditionTypesLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> conditionTypesLikeComboBoxes = ConditionTypePolicy.CanRetrieveManyEntities(
                ConditionTypeRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllConditionTypesLikeComboBoxesOutput()
            {
                Items = conditionTypesLikeComboBoxes
            };
        }

        public RetrieveAllOutput<ConditionTypeDto, long> RetrieveAll(RetrieveAllConditionTypeInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            IList<ConditionType> conditionTypeEntities = ConditionTypePolicy.CanRetrieveManyEntities(
                ConditionTypeRepository.GetAll()
                .WhereIf(!input.ConditionTypeIds.IsNullOrEmpty(), r => input.ConditionTypeIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<ConditionTypeDto> result = conditionTypeEntities.MapIList<ConditionType, ConditionTypeDto>();

            ConditionTypeRepository.Includes.Clear();

            return new RetrieveAllOutput<ConditionTypeDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveOutput<ConditionTypeDto, long> Retrieve(RetrieveConditionTypeInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            IList<ConditionType> conditionTypeEntities = ConditionTypeRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (conditionTypeEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"ConditionType\"");

            if (!ConditionTypePolicy.CanRetrieveEntity(conditionTypeEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"ConditionType\"");

            ConditionTypeDto conditionTypeEntity = conditionTypeEntities.Single().MapTo<ConditionTypeDto>();

            ConditionTypeRepository.Includes.Clear();

            return new RetrieveOutput<ConditionTypeDto, long>()
            {
                RetrievedEntity = conditionTypeEntity
            };
        }

        public CreateOutput<ConditionTypeDto, long> Create(CreateInput<ConditionTypeDto, long> input)
        {
            ConditionType newConditionTypeEntity = input.Entity.MapTo<ConditionType>();

            newConditionTypeEntity.IsDefault = false;
            newConditionTypeEntity.IsActive = true;

            if (!ConditionTypePolicy.CanCreateEntity(newConditionTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"ConditionType\"");

            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            ConditionTypeDto newConditionTypeDto = (ConditionTypeRepository.Insert(newConditionTypeEntity)).MapTo<ConditionTypeDto>();

            ConditionTypeRepository.Includes.Clear();

            return new CreateOutput<ConditionTypeDto, long>()
            {
                CreatedEntity = newConditionTypeDto
            };
        }

        public UpdateOutput<ConditionTypeDto, long> Update(UpdateInput<ConditionTypeDto, long> input)
        {
            ConditionType newConditionTypeEntity = input.Entity.MapTo<ConditionType>();

            if (newConditionTypeEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"ConditionType\"");

            if (!ConditionTypePolicy.CanUpdateEntity(newConditionTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"ConditionType\"");

            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            ConditionTypeRepository.Update(newConditionTypeEntity);
            ConditionTypeDto newConditionTypeDto = (ConditionTypeRepository.Get(newConditionTypeEntity.Id)).MapTo<ConditionTypeDto>();

            ConditionTypeRepository.Includes.Clear();

            return new UpdateOutput<ConditionTypeDto, long>()
            {
                UpdatedEntity = newConditionTypeDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            ConditionType conditionTypeEntityForDelete = ConditionTypeRepository.Get(input.EntityId);

            if (conditionTypeEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"ConditionType\"");

            if (!ConditionTypePolicy.CanDeleteEntity(conditionTypeEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"ConditionType\"");

            ConditionTypeRepository.Delete(conditionTypeEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        public ChangeActivityOutput<ConditionTypeDto, long> ChangeActivity(ChangeActivityInput input)
        {
            ConditionTypeRepository.Includes.Add(r => r.LastModifierUser);
            ConditionTypeRepository.Includes.Add(r => r.CreatorUser);

            ConditionType conditionTypeEntity = ConditionTypeRepository.Get(input.EntityId);

            if (conditionTypeEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"ConditionType\"");

            if (!ConditionTypePolicy.CanChangeActivityForEntity(conditionTypeEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeActivityDenied, "\"ConditionType\"");

            conditionTypeEntity.IsActive = input.IsActive == null ? !conditionTypeEntity.IsActive : (bool)input.IsActive;

            ConditionTypeDto newConditionTypeDto = (conditionTypeEntity).MapTo<ConditionTypeDto>();

            ConditionTypeRepository.Update(conditionTypeEntity);

            ConditionTypeRepository.Includes.Clear();

            return new ChangeActivityOutput<ConditionTypeDto, long>()
            {
                Entity = newConditionTypeDto
            };
        }
    }
}
