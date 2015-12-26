using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.Locations.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestPolicy.GameModule.Locations;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Locations
{
    [Abp.Authorization.AbpAuthorize]
    public class LocationAppService : ILocationAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private ILocationRepository LocationRepository { get; set; }
        private ILocationPolicy LocationPolicy { get; set; }

        #endregion

        #region ctors

        public LocationAppService(IUnitOfWorkManager uowManager,
            ILocationRepository locationRepository,
            ILocationPolicy locationPolicy)
        {
            UowManager = uowManager;
            LocationRepository = locationRepository;
            LocationPolicy = locationPolicy;
        }

        #endregion

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllPagedResultOutput<LocationDto, long> RetrieveAllPagedResult(RetrieveAllLocationsPagedResultInput input)
        {
            LocationRepository.Includes.Add(r => r.LastModifierUser);
            LocationRepository.Includes.Add(r => r.CreatorUser);

            
            IQueryable<Location> locationsQuery = LocationPolicy.CanRetrieveManyEntities(
                LocationRepository.GetAll()
                .WhereIf(!input.LocationIds.IsNullOrEmpty(), r => input.LocationIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .WhereIf(input.DateStart != null, r => (r.CreationTime > input.DateStart))
                .WhereIf(input.DateEnd != null, r => (r.CreationTime < input.DateEnd)));

            int totalCount = locationsQuery.Count();
            IReadOnlyList<LocationDto> locationDtos = locationsQuery
                .OrderBy(r => r.DisplayName)
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToList().MapIList<Location, LocationDto>().ToList();

            LocationRepository.Includes.Clear();

            return new RetrieveAllPagedResultOutput<LocationDto, long>()
            {
                Items = locationDtos,
                TotalCount = totalCount
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllLocationsLikeComboBoxesOutput RetrieveAllLocationsLikeComboBoxes(RetrieveAllLocationsLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> locationsLikeComboBoxes = LocationPolicy.CanRetrieveManyEntities(
                LocationRepository.GetAll()).ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.DisplayName))
                .OrderBy(r => r.DisplayText).ToList();

            return new RetrieveAllLocationsLikeComboBoxesOutput()
            {
                Items = locationsLikeComboBoxes
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveAllOutput<LocationDto, long> RetrieveAll(RetrieveAllLocationInput input)
        {
            LocationRepository.Includes.Add(r => r.LastModifierUser);
            LocationRepository.Includes.Add(r => r.CreatorUser);

            IList<Location> locationEntities = LocationPolicy.CanRetrieveManyEntities(
                LocationRepository.GetAll()
                .WhereIf(!input.LocationIds.IsNullOrEmpty(), r => input.LocationIds.Contains(r.Id))
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower())))
                // .WhereIf(input.LocationIds, r => input.LocationIds)
                .OrderBy(r => r.DisplayName)
                .ToList();

            IList<LocationDto> result = locationEntities.MapIList<Location, LocationDto>();

            LocationRepository.Includes.Clear();

            return new RetrieveAllOutput<LocationDto, long>()
            {
                RetrievedEntities = result
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public RetrieveOutput<LocationDto, long> Retrieve(RetrieveLocationInput input)
        {
            LocationRepository.Includes.Add(r => r.LastModifierUser);
            LocationRepository.Includes.Add(r => r.CreatorUser);

            IList<Location> locationEntities = LocationRepository.GetAll()
                .WhereIf(input.Id != null, r => r.Id == input.Id)
                .WhereIf(!String.IsNullOrEmpty(input.Name), r => r.Name.ToLower().Contains(input.Name.ToLower()))
                .ToList();

            if (locationEntities.Count != 1)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Location\"");

            if (!LocationPolicy.CanRetrieveEntity(locationEntities.Single()))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionRetrieveDenied, "\"Location\"");

            LocationDto locationEntity = locationEntities.Single().MapTo<LocationDto>();

            LocationRepository.Includes.Clear();

            return new RetrieveOutput<LocationDto, long>()
            {
                RetrievedEntity = locationEntity
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public CreateOutput<LocationDto, long> Create(CreateInput<LocationDto, long> input)
        {
            Location newLocationEntity = input.Entity.MapTo<Location>();

            if (!LocationPolicy.CanCreateEntity(newLocationEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"Location\"");

            LocationRepository.Includes.Add(r => r.LastModifierUser);
            LocationRepository.Includes.Add(r => r.CreatorUser);

            LocationDto newLocationDto = (LocationRepository.Insert(newLocationEntity)).MapTo<LocationDto>();

            LocationRepository.Includes.Clear();

            return new CreateOutput<LocationDto, long>()
            {
                CreatedEntity = newLocationDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public UpdateOutput<LocationDto, long> Update(UpdateInput<LocationDto, long> input)
        {
            Location newLocationEntity = input.Entity.MapTo<Location>();

            if (newLocationEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Location\"");

            if (!LocationPolicy.CanUpdateEntity(newLocationEntity))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionUpdateDenied, "\"Location\"");

            LocationRepository.Includes.Add(r => r.LastModifierUser);
            LocationRepository.Includes.Add(r => r.CreatorUser);

            LocationRepository.Update(newLocationEntity);
            LocationDto newLocationDto = (LocationRepository.Get(newLocationEntity.Id)).MapTo<LocationDto>();

            LocationRepository.Includes.Clear();

            return new UpdateOutput<LocationDto, long>()
            {
                UpdatedEntity = newLocationDto
            };
        }

        [Abp.Authorization.AbpAuthorize]
        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            Location locationEntityForDelete = LocationRepository.Get(input.EntityId);

            if (locationEntityForDelete == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Location\"");

            if (!LocationPolicy.CanDeleteEntity(locationEntityForDelete))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionDeleteDenied, "\"Location\"");

            LocationRepository.Delete(locationEntityForDelete);

            return new DeleteOutput<long>()
            {
                DeletedEntityId = input.EntityId
            };
        }
    }
}
