namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPlayerCareers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropIndex("dbo.Teams", new[] { "CaptainId" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            CreateTable(
                "dbo.PlayerCareers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        TeamId = c.Long(nullable: false),
                        IsCaptain = c.Boolean(nullable: false),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PlayerCareer_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_PlayerCareer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TeamId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            DropColumn("dbo.Teams", "CaptainId");
            DropTable("dbo.TeamUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_Id = c.Long(nullable: false),
                        User_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.User_Id });
            
            AddColumn("dbo.Teams", "CaptainId", c => c.Long(nullable: false));
            DropForeignKey("dbo.PlayerCareers", "UserId", "dbo.Users");
            DropForeignKey("dbo.PlayerCareers", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.PlayerCareers", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerCareers", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerCareers", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.PlayerCareers", new[] { "CreatorUserId" });
            DropIndex("dbo.PlayerCareers", new[] { "LastModifierUserId" });
            DropIndex("dbo.PlayerCareers", new[] { "DeleterUserId" });
            DropIndex("dbo.PlayerCareers", new[] { "TeamId" });
            DropIndex("dbo.PlayerCareers", new[] { "UserId" });
            DropTable("dbo.PlayerCareers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PlayerCareer_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_PlayerCareer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.TeamUsers", "User_Id");
            CreateIndex("dbo.TeamUsers", "Team_Id");
            CreateIndex("dbo.Teams", "CaptainId");
            AddForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Teams", "CaptainId", "dbo.Users", "Id");
        }
    }
}
