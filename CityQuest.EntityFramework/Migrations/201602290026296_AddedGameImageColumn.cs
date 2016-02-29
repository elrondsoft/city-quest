namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGameImageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "GameImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "GameImageName");
        }
    }
}
