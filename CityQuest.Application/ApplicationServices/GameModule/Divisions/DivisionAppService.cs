using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.Shared;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.CityQuestPolicy.GameModule.Divisions;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.ApplicationServices.GameModule.Divisions
{
    [Abp.Authorization.AbpAuthorize]
    public class DivisionAppService : IDivisionAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IDivisionRepository DivisionRepository { get; set; }
        private IDivisionPolicy DivisionPolicy { get; set; }

        #endregion

        #region ctors

        public DivisionAppService(IUnitOfWorkManager uowManager, 
            IDivisionRepository divisionRepository, 
            IDivisionPolicy divisionPolicy)
        {
            UowManager = uowManager;
            DivisionRepository = divisionRepository;
            DivisionPolicy = divisionPolicy;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllPagedResultOutput<DivisionDto, long> RetrieveAllPagedResult(RetrieveAllDivisionsPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            IQueryable<Division> divisionsQuery = DivisionPolicy.CanRetrieveManyEntities(
                DivisionRepository.GetAll()
                .WhereIf(!input.DivisionIds.IsNullOrEmpty(), r => input.DivisionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())));

            int totalCount = divisionsQuery.Count();
            IReadOnlyList<DivisionDto> divisionDtos = divisionsQuery
                .OrderByDescending(r => r.IsDefault).ThenByDescending(r => r.IsActive).ThenBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Division, DivisionDto>().ToList();

            DivisionRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<DivisionDto, long>()
            {
                Items = divisionDtos,
                TotalCount = totalCount
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllDivisionsLikeComboBoxesOutput RetrieveAllDivisionsLikeComboBoxes(RetrieveAllDivisionsLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> divisionsLikeComboBoxes = DivisionPolicy.CanRetrieveManyEntities(
                DivisionRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllDivisionsLikeComboBoxesOutput()
            {
                Items = divisionsLikeComboBoxes
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<DivisionDto, long> RetrieveAll(RetrieveAllDivisionInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            IList<Division> divisionEntities = DivisionPolicy.CanRetrieveManyEntities( 
                DivisionRepository.GetAll()
                .WhereIf(!input.DivisionIds.IsNullOrEmpty(), r => input.DivisionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                .ToList();

            IList<DivisionDto> result = divisionEntities.MapIList<Division, DivisionDto>();

            DivisionRepository.Includes.Clear();

            return new RetrieveAllOutput<DivisionDto, long>()
            {
                RetrievedEntities = result
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<DivisionDto, long> Retrieve(RetrieveDivisionInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            IList<Division> divisionEntities = DivisionRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (divisionEntities.Count != 1) 
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Division\"");            

            if (!DivisionPolicy.CanRetrieveEntity(divisionEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Division\"");

            DivisionDto divisionEntity = divisionEntities.Single().MapTo<DivisionDto>();

            DivisionRepository.Includes.Clear();

            return new RetrieveOutput<DivisionDto, long>()
            {
                RetrievedEntity = divisionEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<DivisionDto, long> Create(CreateInput<DivisionDto, long> input)
        {
            Division newDivisionEntity = input.Entity.MapTo<Division>();

            newDivisionEntity.IsDefault = false;
            newDivisionEntity.IsActive = true;

            if (!DivisionPolicy.CanCreateEntity(newDivisionEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"Division\"");

            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            DivisionDto newDivisionDto = (DivisionRepository.Insert(newDivisionEntity)).MapTo<DivisionDto>();

            DivisionRepository.Includes.Clear();

            return new CreateOutput<DivisionDto, long>()
            {
                CreatedEntity = newDivisionDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<DivisionDto, long> Update(UpdateInput<DivisionDto, long> input)
        {
            Division newDivisionEntity = input.Entity.MapTo<Division>();

            if (newDivisionEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Division\"");

            if (!DivisionPolicy.CanUpdateEntity(newDivisionEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"Division\"");

            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            DivisionRepository.Update(newDivisionEntity);
            DivisionDto newDivisionDto = (DivisionRepository.Get(newDivisionEntity.Id)).MapTo<DivisionDto>();

            DivisionRepository.Includes.Clear();

            return new UpdateOutput<DivisionDto, long>()
            {
                UpdatedEntity = newDivisionDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Division divisionEntityForDelete = DivisionRepository.Get(input.EntityId);

            if (divisionEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Division\"");

            if (!DivisionPolicy.CanDeleteEntity(divisionEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"Division\"");

            DivisionRepository.Delete(divisionEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public ChangeActivityOutput<DivisionDto, long> ChangeActivity(ChangeActivityInput input)
        {
            DivisionRepository.Includes.Add(r => r.LastModifierUser);
            DivisionRepository.Includes.Add(r => r.CreatorUser);
            DivisionRepository.Includes.Add(r => r.Teams);

            Division divisionEntity = DivisionRepository.Get(input.EntityId);

            if (divisionEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Division\"");

            if (!DivisionPolicy.CanChangeActivityForEntity(divisionEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionChangeActivityDenied, "\"Division\"");

            divisionEntity.IsActive = input.IsActive == null ? !divisionEntity.IsActive : (bool)input.IsActive;

            DivisionDto newDivisionDto = (divisionEntity).MapTo<DivisionDto>();

            DivisionRepository.Update(divisionEntity);

            DivisionRepository.Includes.Clear();

            return new ChangeActivityOutput<DivisionDto, long>()
            {
                Entity = newDivisionDto
            };
        }
    }
}
