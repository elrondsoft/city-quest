using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class GameTaskRepository : CityQuestRepositoryBase<GameTask, long>, IGameTaskRepository
    {
        public GameTaskRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
