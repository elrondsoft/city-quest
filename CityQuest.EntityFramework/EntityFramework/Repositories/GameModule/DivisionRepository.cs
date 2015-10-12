using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Divisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class DivisionRepository : CityQuestRepositoryBase<Division, long>, IDivisionRepository
    {
        public DivisionRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
