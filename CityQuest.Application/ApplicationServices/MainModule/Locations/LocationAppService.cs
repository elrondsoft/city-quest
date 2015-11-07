using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityQuest.ApplicationServices.MainModule.Locations.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using Abp.Domain.Uow;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Mapping;


namespace CityQuest.ApplicationServices.MainModule.Locations
{
    public class LocationAppService : ILocationAppService
    {
        #region Injected Dependencies
        private IUnitOfWorkManager UowManager { get; set; }
        private ILocationRepository LocationRepository { get; set; }
        #endregion

        public LocationAppService(IUnitOfWorkManager uowManager, ILocationRepository locationRepository)
        {
            UowManager = uowManager;
            LocationRepository = locationRepository;
        }

        public RetrieveAllOutput<LocationDto, long> RetriveAll(RetrieveAllLocationsInput input)
        {

            IList<Location> locationEntities = LocationRepository.GetAll().ToList();

            IList<LocationDto> result = locationEntities.MapIList<Location, LocationDto>();
            
            return new RetrieveAllOutput<LocationDto, long>()
            {
                RetrievedEntities = result,
            };
        }
    }
}
