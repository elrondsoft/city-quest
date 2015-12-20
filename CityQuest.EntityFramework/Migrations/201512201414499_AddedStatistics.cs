namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatistics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerGameTaskStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        PlayerCareerId = c.Long(nullable: false),
                        GameTaskStartDateTime = c.DateTime(nullable: false),
                        GameTaskEndDateTime = c.DateTime(nullable: false),
                        GameTaskDurationInTicks = c.Long(nullable: false),
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
                    { "DynamicFilter_PlayerGameTaskStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.GameTasks", t => t.GameTaskId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.PlayerCareers", t => t.PlayerCareerId)
                .Index(t => t.GameTaskId)
                .Index(t => t.PlayerCareerId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.PlayerGameStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        PlayerCareerId = c.Long(nullable: false),
                        GameStartDateTime = c.DateTime(nullable: false),
                        GameEndDateTime = c.DateTime(nullable: false),
                        GameDurationInTicks = c.Long(nullable: false),
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
                    { "DynamicFilter_PlayerGameStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Games", t => t.GameId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.PlayerCareers", t => t.PlayerCareerId)
                .Index(t => t.GameId)
                .Index(t => t.PlayerCareerId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.SuccessfulPlayerAttempts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConditionId = c.Long(nullable: false),
                        PlayerCareerId = c.Long(nullable: false),
                        InputedValue = c.String(),
                        AttemptDateTime = c.DateTime(nullable: false),
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
                    { "DynamicFilter_SuccessfulPlayerAttempt_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conditions", t => t.ConditionId)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.PlayerCareers", t => t.PlayerCareerId)
                .Index(t => t.ConditionId)
                .Index(t => t.PlayerCareerId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.TeamGameStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        TeamId = c.Long(nullable: false),
                        GameStartDateTime = c.DateTime(nullable: false),
                        GameEndDateTime = c.DateTime(nullable: false),
                        GameDurationInTicks = c.Long(nullable: false),
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
                    { "DynamicFilter_TeamGameStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Games", t => t.GameId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.GameId)
                .Index(t => t.TeamId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.TeamGameTaskStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        TeamId = c.Long(nullable: false),
                        GameTaskStartDateTime = c.DateTime(nullable: false),
                        GameTaskEndDateTime = c.DateTime(nullable: false),
                        GameTaskDurationInTicks = c.Long(nullable: false),
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
                    { "DynamicFilter_TeamGameTaskStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.GameTasks", t => t.GameTaskId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.GameTaskId)
                .Index(t => t.TeamId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.UnsuccessfulPlayerAttempts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConditionId = c.Long(nullable: false),
                        PlayerCareerId = c.Long(nullable: false),
                        InputedValue = c.String(),
                        AttemptDateTime = c.DateTime(nullable: false),
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
                    { "DynamicFilter_UnsuccessfulPlayerAttempt_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conditions", t => t.ConditionId)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.PlayerCareers", t => t.PlayerCareerId)
                .Index(t => t.ConditionId)
                .Index(t => t.PlayerCareerId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            AddColumn("dbo.PlayerCareers", "CareerDateStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.PlayerCareers", "CareerDateEnd", c => c.DateTime());
            AddColumn("dbo.PlayerCareers", "Team_Id", c => c.Long());
            CreateIndex("dbo.PlayerCareers", "Team_Id");
            AddForeignKey("dbo.PlayerCareers", "Team_Id", "dbo.Teams", "Id");
            DropColumn("dbo.PlayerCareers", "DateStart");
            DropColumn("dbo.PlayerCareers", "DateEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerCareers", "DateEnd", c => c.DateTime());
            AddColumn("dbo.PlayerCareers", "DateStart", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.PlayerGameTaskStatistics", "PlayerCareerId", "dbo.PlayerCareers");
            DropForeignKey("dbo.UnsuccessfulPlayerAttempts", "PlayerCareerId", "dbo.PlayerCareers");
            DropForeignKey("dbo.UnsuccessfulPlayerAttempts", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.UnsuccessfulPlayerAttempts", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.UnsuccessfulPlayerAttempts", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.UnsuccessfulPlayerAttempts", "ConditionId", "dbo.Conditions");
            DropForeignKey("dbo.TeamGameTaskStatistics", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamGameTaskStatistics", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.TeamGameTaskStatistics", "GameTaskId", "dbo.GameTasks");
            DropForeignKey("dbo.TeamGameTaskStatistics", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.TeamGameTaskStatistics", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.TeamGameStatistics", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamGameStatistics", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.TeamGameStatistics", "GameId", "dbo.Games");
            DropForeignKey("dbo.TeamGameStatistics", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.TeamGameStatistics", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerCareers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.SuccessfulPlayerAttempts", "PlayerCareerId", "dbo.PlayerCareers");
            DropForeignKey("dbo.SuccessfulPlayerAttempts", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.SuccessfulPlayerAttempts", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.SuccessfulPlayerAttempts", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.SuccessfulPlayerAttempts", "ConditionId", "dbo.Conditions");
            DropForeignKey("dbo.PlayerGameStatistics", "PlayerCareerId", "dbo.PlayerCareers");
            DropForeignKey("dbo.PlayerGameStatistics", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerGameStatistics", "GameId", "dbo.Games");
            DropForeignKey("dbo.PlayerGameStatistics", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerGameStatistics", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerGameTaskStatistics", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerGameTaskStatistics", "GameTaskId", "dbo.GameTasks");
            DropForeignKey("dbo.PlayerGameTaskStatistics", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.PlayerGameTaskStatistics", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.UnsuccessfulPlayerAttempts", new[] { "CreatorUserId" });
            DropIndex("dbo.UnsuccessfulPlayerAttempts", new[] { "LastModifierUserId" });
            DropIndex("dbo.UnsuccessfulPlayerAttempts", new[] { "DeleterUserId" });
            DropIndex("dbo.UnsuccessfulPlayerAttempts", new[] { "PlayerCareerId" });
            DropIndex("dbo.UnsuccessfulPlayerAttempts", new[] { "ConditionId" });
            DropIndex("dbo.TeamGameTaskStatistics", new[] { "CreatorUserId" });
            DropIndex("dbo.TeamGameTaskStatistics", new[] { "LastModifierUserId" });
            DropIndex("dbo.TeamGameTaskStatistics", new[] { "DeleterUserId" });
            DropIndex("dbo.TeamGameTaskStatistics", new[] { "TeamId" });
            DropIndex("dbo.TeamGameTaskStatistics", new[] { "GameTaskId" });
            DropIndex("dbo.TeamGameStatistics", new[] { "CreatorUserId" });
            DropIndex("dbo.TeamGameStatistics", new[] { "LastModifierUserId" });
            DropIndex("dbo.TeamGameStatistics", new[] { "DeleterUserId" });
            DropIndex("dbo.TeamGameStatistics", new[] { "TeamId" });
            DropIndex("dbo.TeamGameStatistics", new[] { "GameId" });
            DropIndex("dbo.SuccessfulPlayerAttempts", new[] { "CreatorUserId" });
            DropIndex("dbo.SuccessfulPlayerAttempts", new[] { "LastModifierUserId" });
            DropIndex("dbo.SuccessfulPlayerAttempts", new[] { "DeleterUserId" });
            DropIndex("dbo.SuccessfulPlayerAttempts", new[] { "PlayerCareerId" });
            DropIndex("dbo.SuccessfulPlayerAttempts", new[] { "ConditionId" });
            DropIndex("dbo.PlayerGameStatistics", new[] { "CreatorUserId" });
            DropIndex("dbo.PlayerGameStatistics", new[] { "LastModifierUserId" });
            DropIndex("dbo.PlayerGameStatistics", new[] { "DeleterUserId" });
            DropIndex("dbo.PlayerGameStatistics", new[] { "PlayerCareerId" });
            DropIndex("dbo.PlayerGameStatistics", new[] { "GameId" });
            DropIndex("dbo.PlayerCareers", new[] { "Team_Id" });
            DropIndex("dbo.PlayerGameTaskStatistics", new[] { "CreatorUserId" });
            DropIndex("dbo.PlayerGameTaskStatistics", new[] { "LastModifierUserId" });
            DropIndex("dbo.PlayerGameTaskStatistics", new[] { "DeleterUserId" });
            DropIndex("dbo.PlayerGameTaskStatistics", new[] { "PlayerCareerId" });
            DropIndex("dbo.PlayerGameTaskStatistics", new[] { "GameTaskId" });
            DropColumn("dbo.PlayerCareers", "Team_Id");
            DropColumn("dbo.PlayerCareers", "CareerDateEnd");
            DropColumn("dbo.PlayerCareers", "CareerDateStart");
            DropTable("dbo.UnsuccessfulPlayerAttempts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnsuccessfulPlayerAttempt_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TeamGameTaskStatistics",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TeamGameTaskStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TeamGameStatistics",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TeamGameStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SuccessfulPlayerAttempts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SuccessfulPlayerAttempt_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PlayerGameStatistics",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PlayerGameStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PlayerGameTaskStatistics",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PlayerGameTaskStatistic_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
