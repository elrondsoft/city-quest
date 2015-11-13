using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class ConditionTypeRepository : CityQuestRepositoryBase<ConditionType, long>, IConditionTypeRepository
    {
        public ConditionTypeRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
