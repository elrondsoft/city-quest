namespace CityQuest.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderForGameRelatedEntities : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.Tips",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        Name = c.String(),
                        TipText = c.String(),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Tip_IPassivableFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.Conditions", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GameTasks", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Tips", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.Tips", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tips", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tips", "Order");
            DropColumn("dbo.GameTasks", "Order");
            DropColumn("dbo.Conditions", "Order");
            AlterTableAnnotations(
                "dbo.Tips",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameTaskId = c.Long(nullable: false),
                        Name = c.String(),
                        TipText = c.String(),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Tip_IPassivableFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
    }
}
