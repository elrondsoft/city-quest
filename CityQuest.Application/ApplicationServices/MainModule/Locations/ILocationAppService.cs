using Abp.Application.Services;
using CityQuest.ApplicationServices.MainModule.Locations.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Locations
{
    public interface ILocationAppService : IApplicationService
    {
        RetrieveAllOutput<LocationDto, long> RetriveAll(RetrieveAllLocationsInput input);
    }
}
