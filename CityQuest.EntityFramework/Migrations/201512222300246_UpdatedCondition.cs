namespace CityQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCondition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayerCareers", "Team_Id", "dbo.Teams");
            DropIndex("dbo.PlayerCareers", new[] { "Team_Id" });
            AddColumn("dbo.Conditions", "ValueToPass", c => c.String());
            DropColumn("dbo.PlayerCareers", "Team_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerCareers", "Team_Id", c => c.Long());
            DropColumn("dbo.Conditions", "ValueToPass");
            CreateIndex("dbo.PlayerCareers", "Team_Id");
            AddForeignKey("dbo.PlayerCareers", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
