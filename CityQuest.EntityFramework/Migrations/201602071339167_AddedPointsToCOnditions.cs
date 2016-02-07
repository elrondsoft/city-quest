namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPointsToCOnditions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conditions", "Points", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conditions", "Points");
        }
    }
}
