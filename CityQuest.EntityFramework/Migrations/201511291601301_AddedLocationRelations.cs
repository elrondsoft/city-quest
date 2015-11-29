namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocationRelations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LocationId", c => c.Long());
            AddColumn("dbo.Games", "LocationId", c => c.Long(nullable: false));
            AddColumn("dbo.Games", "StartDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Users", "LocationId");
            CreateIndex("dbo.Games", "LocationId");
            AddForeignKey("dbo.Games", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.Users", "LocationId", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Games", "LocationId", "dbo.Locations");
            DropIndex("dbo.Games", new[] { "LocationId" });
            DropIndex("dbo.Users", new[] { "LocationId" });
            DropColumn("dbo.Games", "StartDate");
            DropColumn("dbo.Games", "LocationId");
            DropColumn("dbo.Users", "LocationId");
        }
    }
}
