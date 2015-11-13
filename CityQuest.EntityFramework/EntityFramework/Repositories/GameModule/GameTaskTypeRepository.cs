using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class GameTaskTypeRepository : CityQuestRepositoryBase<GameTaskType, long>, IGameTaskTypeRepository
    {
        public GameTaskTypeRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
