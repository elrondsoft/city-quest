using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class KeyRepository : CityQuestRepositoryBase<Key, long>, IKeyRepository
    {
        public KeyRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
