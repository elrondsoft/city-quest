namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedGameStatuses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameStatus", "NextAllowedStatusNames", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameStatus", "NextAllowedStatusNames");
        }
    }
}
