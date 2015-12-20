using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class GameStatusRepository : CityQuestRepositoryBase<GameStatus, long>, IGameStatusRepository
    {
        public GameStatusRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
