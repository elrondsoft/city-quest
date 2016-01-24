using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class UnsuccessfullPlayerAttemptRepository : CityQuestRepositoryBase<UnsuccessfulPlayerAttempt, long>, IUnsuccessfullPlayerAttemptRepository
    {
        public UnsuccessfullPlayerAttemptRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
