using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.Shared;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions
{
    public class DivisionAppService : IDivisionAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private IDivisionRepository DivisionRepository { get; set; }

        #endregion

        public DivisionAppService(IUnitOfWorkManager uowManager, IDivisionRepository divisionRepository)
        {
            UowManager = uowManager;
            DivisionRepository = divisionRepository;
        }

        public RetrieveAllPagedResultOutput<DivisionDto, long> RetrieveAllPagedResult(RetrieveAllDivisionsPagedResultInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IQueryable<Division> divisionsQuery = DivisionRepository.GetAll()
                .WhereIf(!input.DivisionIds.IsNullOrEmpty(), r => input.DivisionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()));

            int totalCount = divisionsQuery.Count();
            IReadOnlyList<DivisionDto> divisionDtos = divisionsQuery
                .OrderBy(r => r.Name)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Division, DivisionDto>().ToList();

            return new RetrieveAllPagedResultOutput<DivisionDto, long>()
            {
                Items = divisionDtos,
                TotalCount = totalCount
            };
        }

        public RetrieveAllDivisionsLikeComboBoxesOutput RetrieveAllDivisionsLikeComboBoxes(RetrieveAllDivisionsLikeComboBoxesInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IReadOnlyList<ComboboxItemDto> divisionsLikeComboBoxes = DivisionRepository.GetAll()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name)).ToList();

            return new RetrieveAllDivisionsLikeComboBoxesOutput()
            {
                Items = divisionsLikeComboBoxes
            };
        }

        public RetrieveAllOutput<DivisionDto, long> RetrieveAll(RetrieveAllDivisionInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IList<Division> divisionEntities = DivisionRepository.GetAll()
                .WhereIf(!input.DivisionIds.IsNullOrEmpty(), r => input.DivisionIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            IList<DivisionDto> result = divisionEntities.MapIList<Division, DivisionDto>();

            return new RetrieveAllOutput<DivisionDto, long>()
            {
                RetrievedItems = result
            };
        }

        public RetrieveOutput<DivisionDto, long> Retrieve(RetrieveDivisionInput input)
        {
            if (input.IsActive ?? true)
                UowManager.Current.EnableFilter(Filters.IPassivableFilter);

            IList<Division> divisionEntities = DivisionRepository.GetAll()
                .WhereIf(input.DivisionId != null, r => r.Id == input.DivisionId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (divisionEntities.Count != 1) 
            {
                throw new UserFriendlyException(String.Format(
                    "Can not retrieve Division with these filters."));            
            }
//TODO: add policy
            //if (false)
            //{
            //    throw new AbpAuthorizationException(String.Format(
            //        "You have not permissions to retrieve this Division's entity.")); 
            //}

            DivisionDto divisionEntity = divisionEntities.Single().MapTo<DivisionDto>();

            return new RetrieveOutput<DivisionDto, long>()
            {
                RetrievedItem = divisionEntity
            };
        }

        public CreateOutput<DivisionDto, long> Create(CreateInput<DivisionDto, long> input)
        {
            Division newDivisionEntity = input.EntityDto.MapTo<Division>();

            DivisionDto newDivisionDto = (DivisionRepository.Insert(newDivisionEntity)).MapTo<DivisionDto>();

            return new CreateOutput<DivisionDto, long>()
            {
                CreatedItem = newDivisionDto
            };
        }

        public UpdateOutput<DivisionDto, long> Update(UpdateInput<DivisionDto, long> input)
        {
            Division newDivisionEntity = input.EntityDto.MapTo<Division>();

            if (newDivisionEntity == null)
            {
                throw new UserFriendlyException(String.Format(
                    "There are no Division with Id = {0}. Can not update it.", input.EntityDto.Id));
            }

            DivisionDto newDivisionDto = (DivisionRepository.Update(newDivisionEntity)).MapTo<DivisionDto>();

            return new UpdateOutput<DivisionDto, long>()
            {
                UpdatedItem = newDivisionDto
            };
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Division divisionEntityForDelete = DivisionRepository.Get(input.EntityId);

            if (divisionEntityForDelete == null)
            {
                throw new UserFriendlyException(String.Format(
                    "There are no Division with Id = {0}. Can not delete it.", input.EntityId));
            }

            DivisionRepository.Delete(divisionEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedItemId = input.EntityId
            };
        }
    }
}
