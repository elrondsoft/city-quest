using CityQuest.EntityFramework;
using System.Data.Entity.Migrations;

namespace CityQuest.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CityQuestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = CityQuestConsts.CityQuestBDContextKey;
        }

        protected override void Seed(CityQuestDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
