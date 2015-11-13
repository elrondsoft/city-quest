using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class TipRepository : CityQuestRepositoryBase<Tip, long>, ITipRepository
    {
        public TipRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
