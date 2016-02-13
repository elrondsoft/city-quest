namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReceivedPointsToStatistics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerGameTaskStatistics", "ReceivedPoints", c => c.Int());
            AddColumn("dbo.PlayerGameStatistics", "ReceivedPoints", c => c.Int());
            AddColumn("dbo.TeamGameStatistics", "ReceivedPoints", c => c.Int());
            AddColumn("dbo.TeamGameTaskStatistics", "ReceivedPoints", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamGameTaskStatistics", "ReceivedPoints");
            DropColumn("dbo.TeamGameStatistics", "ReceivedPoints");
            DropColumn("dbo.PlayerGameStatistics", "ReceivedPoints");
            DropColumn("dbo.PlayerGameTaskStatistics", "ReceivedPoints");
        }
    }
}
