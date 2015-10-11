using Abp.EntityFramework;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.MainModule.Users;
using System.Data.Entity;

namespace CityQuest.EntityFramework
{
    public class CityQuestDbContext : AbpDbContext
    {
        #region DataBase Sets

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Game> Games { get; set; }

        #endregion

        public CityQuestDbContext()
            : base("CityQuest")
        {

        }

        public CityQuestDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Database.CommandTimeout = 3600;
            //this.Database.Log = (r) => Trace.WriteLine(r);
        }
    }
}
