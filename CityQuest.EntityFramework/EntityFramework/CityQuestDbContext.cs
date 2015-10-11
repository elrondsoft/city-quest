using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.MainModule.Users;
using System.Data.Entity;

namespace CityQuest.EntityFramework
{
    public class CityQuestDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...
        //
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Game> Games { get; set; }

        public CityQuestDbContext()
            : base("CityQuest")
        {

        }

        public CityQuestDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
