using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class GameRepository : CityQuestRepositoryBase<Game, long>, IGameRepository
    {
        public GameRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
