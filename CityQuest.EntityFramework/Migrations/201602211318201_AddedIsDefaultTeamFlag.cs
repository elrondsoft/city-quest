namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsDefaultTeamFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "IsDefault", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "IsDefault");
        }
    }
}
