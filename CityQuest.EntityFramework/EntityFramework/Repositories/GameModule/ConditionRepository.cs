using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class ConditionRepository : CityQuestRepositoryBase<Condition, long>, IConditionRepository
    {
        public ConditionRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
