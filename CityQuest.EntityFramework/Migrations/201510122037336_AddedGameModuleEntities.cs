namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGameModuleEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        ConditionTypeId = c.Long(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConditionTypes", t => t.ConditionTypeId)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.GameTasks", t => t.GameTaskId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.GameTaskId)
                .Index(t => t.ConditionTypeId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.ConditionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDefault = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ConditionType_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                        EmailConfirmationCode = c.String(),
                        LastLoginTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Keys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        OwnerUserId = c.Long(),
                        KeyValue = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .ForeignKey("dbo.Users", t => t.OwnerUserId)
                .Index(t => t.GameId)
                .Index(t => t.OwnerUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
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
                    { "DynamicFilter_Game_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.GameTasks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        GameTaskTypeId = c.Long(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        TaskText = c.String(),
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
                    { "DynamicFilter_GameTask_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Games", t => t.GameId)
                .ForeignKey("dbo.GameTaskTypes", t => t.GameTaskTypeId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.GameId)
                .Index(t => t.GameTaskTypeId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.GameTaskTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDefault = c.Boolean(nullable: false),
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
                    { "DynamicFilter_GameTaskType_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Tips",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        Name = c.String(),
                        TipText = c.String(),
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
                    { "DynamicFilter_Tip_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.GameTasks", t => t.GameTaskId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.GameTaskId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CaptainId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Slogan = c.String(),
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
                    { "DynamicFilter_Team_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CaptainId)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.CaptainId)
                .Index(t => t.DivisionId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDefault = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Division_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_Id = c.Long(nullable: false),
                        User_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.User_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conditions", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Conditions", "GameTaskId", "dbo.GameTasks");
            DropForeignKey("dbo.Conditions", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Conditions", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Conditions", "ConditionTypeId", "dbo.ConditionTypes");
            DropForeignKey("dbo.ConditionTypes", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.ConditionTypes", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.ConditionTypes", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Teams", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Divisions", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Divisions", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Divisions", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Users");
            DropForeignKey("dbo.Users", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Keys", "OwnerUserId", "dbo.Users");
            DropForeignKey("dbo.Keys", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Keys", "GameId", "dbo.Games");
            DropForeignKey("dbo.Games", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Tips", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Tips", "GameTaskId", "dbo.GameTasks");
            DropForeignKey("dbo.Tips", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Tips", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.GameTasks", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.GameTasks", "GameTaskTypeId", "dbo.GameTaskTypes");
            DropForeignKey("dbo.GameTaskTypes", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.GameTaskTypes", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.GameTaskTypes", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.GameTasks", "GameId", "dbo.Games");
            DropForeignKey("dbo.GameTasks", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.GameTasks", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Games", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Games", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Keys", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Keys", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.Divisions", new[] { "CreatorUserId" });
            DropIndex("dbo.Divisions", new[] { "LastModifierUserId" });
            DropIndex("dbo.Divisions", new[] { "DeleterUserId" });
            DropIndex("dbo.Teams", new[] { "CreatorUserId" });
            DropIndex("dbo.Teams", new[] { "LastModifierUserId" });
            DropIndex("dbo.Teams", new[] { "DeleterUserId" });
            DropIndex("dbo.Teams", new[] { "DivisionId" });
            DropIndex("dbo.Teams", new[] { "CaptainId" });
            DropIndex("dbo.Tips", new[] { "CreatorUserId" });
            DropIndex("dbo.Tips", new[] { "LastModifierUserId" });
            DropIndex("dbo.Tips", new[] { "DeleterUserId" });
            DropIndex("dbo.Tips", new[] { "GameTaskId" });
            DropIndex("dbo.GameTaskTypes", new[] { "CreatorUserId" });
            DropIndex("dbo.GameTaskTypes", new[] { "LastModifierUserId" });
            DropIndex("dbo.GameTaskTypes", new[] { "DeleterUserId" });
            DropIndex("dbo.GameTasks", new[] { "CreatorUserId" });
            DropIndex("dbo.GameTasks", new[] { "LastModifierUserId" });
            DropIndex("dbo.GameTasks", new[] { "DeleterUserId" });
            DropIndex("dbo.GameTasks", new[] { "GameTaskTypeId" });
            DropIndex("dbo.GameTasks", new[] { "GameId" });
            DropIndex("dbo.Games", new[] { "CreatorUserId" });
            DropIndex("dbo.Games", new[] { "LastModifierUserId" });
            DropIndex("dbo.Games", new[] { "DeleterUserId" });
            DropIndex("dbo.Keys", new[] { "CreatorUserId" });
            DropIndex("dbo.Keys", new[] { "LastModifierUserId" });
            DropIndex("dbo.Keys", new[] { "DeleterUserId" });
            DropIndex("dbo.Keys", new[] { "OwnerUserId" });
            DropIndex("dbo.Keys", new[] { "GameId" });
            DropIndex("dbo.Users", new[] { "CreatorUserId" });
            DropIndex("dbo.Users", new[] { "LastModifierUserId" });
            DropIndex("dbo.Users", new[] { "DeleterUserId" });
            DropIndex("dbo.ConditionTypes", new[] { "CreatorUserId" });
            DropIndex("dbo.ConditionTypes", new[] { "LastModifierUserId" });
            DropIndex("dbo.ConditionTypes", new[] { "DeleterUserId" });
            DropIndex("dbo.Conditions", new[] { "CreatorUserId" });
            DropIndex("dbo.Conditions", new[] { "LastModifierUserId" });
            DropIndex("dbo.Conditions", new[] { "DeleterUserId" });
            DropIndex("dbo.Conditions", new[] { "ConditionTypeId" });
            DropIndex("dbo.Conditions", new[] { "GameTaskId" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.Divisions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Division_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Teams",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Team_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Tips",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tip_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GameTaskTypes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GameTaskType_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GameTasks",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GameTask_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Games",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Game_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Keys");
            DropTable("dbo.Users");
            DropTable("dbo.ConditionTypes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConditionType_IPassivableFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Conditions");
        }
    }
}
