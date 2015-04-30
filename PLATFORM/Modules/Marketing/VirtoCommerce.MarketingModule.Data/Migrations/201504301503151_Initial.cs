namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promotion",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(maxLength: 128),
                        CatalogId = c.String(maxLength: 128),
                        CouponCode = c.String(maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 1024),
                        IsActive = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        IsExclusive = c.Boolean(nullable: false),
                        PredicateSerialized = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        RewardsSerialized = c.String(),
                        PerCustomerLimit = c.Int(nullable: false),
                        TotalLimit = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Coupon",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(maxLength: 64),
                        PromotionId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.PromotionUsage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MemberId = c.String(maxLength: 128),
                        MemberName = c.String(maxLength: 256),
                        OrderId = c.String(maxLength: 128),
                        OrderNumber = c.String(maxLength: 128),
                        CouponCode = c.String(maxLength: 64),
                        UsageDate = c.DateTime(),
                        PromotionId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.DynamicContentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        ContentTypeId = c.String(maxLength: 64),
                        IsMultilingual = c.Boolean(nullable: false),
                        FolderId = c.String(maxLength: 128),
                        ImageUrl = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentFolder", t => t.FolderId)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.DynamicContentFolder",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        ImageUrl = c.String(maxLength: 2048),
                        ParentFolderId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentFolder", t => t.ParentFolderId)
                .Index(t => t.ParentFolderId);
            
            CreateTable(
                "dbo.DynamicContentPlace",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        ImageUrl = c.String(maxLength: 2048),
                        FolderId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentFolder", t => t.FolderId)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.DynamicContentItemProperty",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        KeyValue = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        DynamicContentItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentItem", t => t.DynamicContentItemId, cascadeDelete: true)
                .Index(t => t.DynamicContentItemId);
            
            CreateTable(
                "dbo.DynamicContentPublishingGroup",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        Priority = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ConditionExpression = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PublishingGroupContentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DynamicContentPublishingGroupId = c.String(maxLength: 128),
                        DynamicContentItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentItem", t => t.DynamicContentItemId)
                .ForeignKey("dbo.DynamicContentPublishingGroup", t => t.DynamicContentPublishingGroupId)
                .Index(t => t.DynamicContentPublishingGroupId)
                .Index(t => t.DynamicContentItemId);
            
            CreateTable(
                "dbo.PublishingGroupContentPlace",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DynamicContentPublishingGroupId = c.String(maxLength: 128),
                        DynamicContentPlaceId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicContentPlace", t => t.DynamicContentPlaceId)
                .ForeignKey("dbo.DynamicContentPublishingGroup", t => t.DynamicContentPublishingGroupId)
                .Index(t => t.DynamicContentPublishingGroupId)
                .Index(t => t.DynamicContentPlaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropForeignKey("dbo.DynamicContentItemProperty", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropForeignKey("dbo.DynamicContentItem", "FolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.DynamicContentFolder", "ParentFolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.DynamicContentPlace", "FolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.PromotionUsage", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.Coupon", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.PublishingGroupContentPlace", new[] { "DynamicContentPlaceId" });
            DropIndex("dbo.PublishingGroupContentPlace", new[] { "DynamicContentPublishingGroupId" });
            DropIndex("dbo.PublishingGroupContentItem", new[] { "DynamicContentItemId" });
            DropIndex("dbo.PublishingGroupContentItem", new[] { "DynamicContentPublishingGroupId" });
            DropIndex("dbo.DynamicContentItemProperty", new[] { "DynamicContentItemId" });
            DropIndex("dbo.DynamicContentPlace", new[] { "FolderId" });
            DropIndex("dbo.DynamicContentFolder", new[] { "ParentFolderId" });
            DropIndex("dbo.DynamicContentItem", new[] { "FolderId" });
            DropIndex("dbo.PromotionUsage", new[] { "PromotionId" });
            DropIndex("dbo.Coupon", new[] { "PromotionId" });
            DropTable("dbo.PublishingGroupContentPlace");
            DropTable("dbo.PublishingGroupContentItem");
            DropTable("dbo.DynamicContentPublishingGroup");
            DropTable("dbo.DynamicContentItemProperty");
            DropTable("dbo.DynamicContentPlace");
            DropTable("dbo.DynamicContentFolder");
            DropTable("dbo.DynamicContentItem");
            DropTable("dbo.PromotionUsage");
            DropTable("dbo.Coupon");
            DropTable("dbo.Promotion");
        }
    }
}
