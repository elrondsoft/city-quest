namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDefaultToRoleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "IsDefault");
        }
    }
}
