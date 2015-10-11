namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserGameEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
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
                    { "DynamicFilter_Game_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
                    { "DynamicFilter_User_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Games", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Games", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "CreatorUserId" });
            DropIndex("dbo.Users", new[] { "LastModifierUserId" });
            DropIndex("dbo.Users", new[] { "DeleterUserId" });
            DropIndex("dbo.Games", new[] { "CreatorUserId" });
            DropIndex("dbo.Games", new[] { "LastModifierUserId" });
            DropIndex("dbo.Games", new[] { "DeleterUserId" });
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Games",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Game_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
