using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion
{   
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupon",
                c => new
                    {
                        CouponId = c.String(nullable: false, maxLength: 128),
                        Code = c.String(maxLength: 64),
                        CouponSetId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CouponId)
                .ForeignKey("dbo.CouponSet", t => t.CouponSetId)
                .Index(t => t.CouponSetId);
            
            CreateTable(
                "dbo.CouponSet",
                c => new
                    {
                        CouponSetId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CouponSetId);
            
            CreateTable(
                "dbo.PromotionReward",
                c => new
                    {
                        PromotionRewardId = c.String(nullable: false, maxLength: 128),
                        RewardAmountId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountTypeId = c.Int(nullable: false),
                        PromotionId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        CategoryId = c.String(maxLength: 128),
                        ProductId = c.String(maxLength: 128),
                        SkuId = c.String(maxLength: 128),
                        ExcludingCategories = c.String(maxLength: 128),
                        ExcludingProducts = c.String(maxLength: 128),
                        ExcludingSkus = c.String(maxLength: 128),
                        QuantityLimit = c.Decimal(precision: 18, scale: 2),
                        ItemsCountLimit = c.Decimal(precision: 18, scale: 2),
                        ShippingMethodId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PromotionRewardId)
                .ForeignKey("dbo.Promotion", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.SegmentSet",
                c => new
                    {
                        SegmentSetId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SegmentSetId);
            
            CreateTable(
                "dbo.Segment",
                c => new
                    {
                        SegmentId = c.String(nullable: false, maxLength: 128),
                        SegmentSetId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SegmentId)
                .ForeignKey("dbo.SegmentSet", t => t.SegmentSetId)
                .Index(t => t.SegmentSetId);
            
            CreateTable(
                "dbo.Promotion",
                c => new
                    {
                        PromotionId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        Status = c.String(maxLength: 32),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        PredicateSerialized = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        PerCustomerLimit = c.Int(nullable: false),
                        TotalLimit = c.Int(nullable: false),
                        ExclusionTypeId = c.Int(nullable: false),
                        SegmentSetId = c.String(maxLength: 128),
                        CouponId = c.String(maxLength: 128),
                        CouponSetId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        CatalogId = c.String(maxLength: 128),
                        StoreId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PromotionId)
                .ForeignKey("dbo.Coupon", t => t.CouponId)
                .ForeignKey("dbo.CouponSet", t => t.CouponSetId)
                .ForeignKey("dbo.SegmentSet", t => t.SegmentSetId)
                .Index(t => t.CouponId)
                .Index(t => t.CouponSetId)
                .Index(t => t.SegmentSetId);
            
            CreateTable(
                "dbo.PromotionUsage",
                c => new
                    {
                        PromotionUsageId = c.String(nullable: false, maxLength: 128),
                        MemberId = c.String(maxLength: 128),
                        OrderGroupId = c.String(maxLength: 128),
                        CouponCode = c.String(maxLength: 64),
                        Status = c.Int(nullable: false, defaultValue:0),
                        UsageDate = c.DateTime(),
                        PromotionId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PromotionUsageId)
                .ForeignKey("dbo.Promotion", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionUsage", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.Promotion", "SegmentSetId", "dbo.SegmentSet");
            DropForeignKey("dbo.Segment", "SegmentSetId", "dbo.SegmentSet");
            DropForeignKey("dbo.PromotionReward", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.Promotion", "CouponSetId", "dbo.CouponSet");
            DropForeignKey("dbo.Promotion", "CouponId", "dbo.Coupon");
            DropForeignKey("dbo.Coupon", "CouponSetId", "dbo.CouponSet");
            DropIndex("dbo.PromotionUsage", new[] { "PromotionId" });
            DropIndex("dbo.Promotion", new[] { "SegmentSetId" });
            DropIndex("dbo.Segment", new[] { "SegmentSetId" });
            DropIndex("dbo.PromotionReward", new[] { "PromotionId" });
            DropIndex("dbo.Promotion", new[] { "CouponSetId" });
            DropIndex("dbo.Promotion", new[] { "CouponId" });
            DropIndex("dbo.Coupon", new[] { "CouponSetId" });
            DropTable("dbo.PromotionUsage");
            DropTable("dbo.Promotion");
            DropTable("dbo.Segment");
            DropTable("dbo.SegmentSet");
            DropTable("dbo.PromotionReward");
            DropTable("dbo.CouponSet");
            DropTable("dbo.Coupon");
        }
    }
}
