namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTeamRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvitedUserId = c.Long(nullable: false),
                        TeamId = c.Long(nullable: false),
                        InvitedUserResponse = c.Boolean(),
                        InvitedUserResponseDateTime = c.DateTime(),
                        DeclinedByInviter = c.Boolean(),
                        DeclinedByInviterDateTime = c.DateTime(),
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
                    { "DynamicFilter_TeamRequest_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.InvitedUserId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.InvitedUserId)
                .Index(t => t.TeamId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamRequests", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamRequests", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.TeamRequests", "InvitedUserId", "dbo.Users");
            DropForeignKey("dbo.TeamRequests", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.TeamRequests", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.TeamRequests", new[] { "CreatorUserId" });
            DropIndex("dbo.TeamRequests", new[] { "LastModifierUserId" });
            DropIndex("dbo.TeamRequests", new[] { "DeleterUserId" });
            DropIndex("dbo.TeamRequests", new[] { "TeamId" });
            DropIndex("dbo.TeamRequests", new[] { "InvitedUserId" });
            DropTable("dbo.TeamRequests",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TeamRequest_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
