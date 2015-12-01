namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGameStatuses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDefault = c.Boolean(nullable: false),
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
                    { "DynamicFilter_GameStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            AddColumn("dbo.Games", "GameStatusId", c => c.Long(nullable: false));
            CreateIndex("dbo.Games", "GameStatusId");
            AddForeignKey("dbo.Games", "GameStatusId", "dbo.GameStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "GameStatusId", "dbo.GameStatus");
            DropForeignKey("dbo.GameStatus", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.GameStatus", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.GameStatus", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.GameStatus", new[] { "CreatorUserId" });
            DropIndex("dbo.GameStatus", new[] { "LastModifierUserId" });
            DropIndex("dbo.GameStatus", new[] { "DeleterUserId" });
            DropIndex("dbo.Games", new[] { "GameStatusId" });
            DropColumn("dbo.Games", "GameStatusId");
            DropTable("dbo.GameStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GameStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
