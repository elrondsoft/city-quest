using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.EntityFramework.Repositories.GameModule
{
    public class TeamRepository : CityQuestRepositoryBase<Team, long>, ITeamRepository
    {
        public TeamRepository(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}

