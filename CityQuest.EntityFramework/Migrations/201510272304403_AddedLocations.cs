namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ShortName = c.String(),
                        DisplayName = c.String(),
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
                    { "DynamicFilter_Location_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
            DropForeignKey("dbo.Locations", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "DeleterUserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.Locations", new[] { "CreatorUserId" });
            DropIndex("dbo.Locations", new[] { "LastModifierUserId" });
            DropIndex("dbo.Locations", new[] { "DeleterUserId" });
            DropTable("dbo.Locations",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Location_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
