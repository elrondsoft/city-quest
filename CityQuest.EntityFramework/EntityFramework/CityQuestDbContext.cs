using Abp.EntityFramework;

namespace CityQuest.EntityFramework
{
    public class CityQuestDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...
        //public virtual IDbSet<User> Users { get; set; }

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
