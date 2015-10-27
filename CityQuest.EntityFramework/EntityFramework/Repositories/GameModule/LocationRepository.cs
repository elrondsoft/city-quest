using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class LocationRepository : CityQuestRepositoryBase<Location, long>, ILocationRepository
    {
        public LocationRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
