using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class UnsuccessfulPlayerAttemptRepository : CityQuestRepositoryBase<UnsuccessfulPlayerAttempt, long>, IUnsuccessfulPlayerAttemptRepository
    {
        public UnsuccessfulPlayerAttemptRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
