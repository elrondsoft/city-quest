using System.Data.Entity.Migrations;

namespace CityQuest.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CityQuest.EntityFramework.CityQuestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CityQuest";
        }

        protected override void Seed(CityQuest.EntityFramework.CityQuestDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
