using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DynamicContentItem",
                c => new
                    {
                        DynamicContentItemId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        ContentTypeId = c.String(maxLength: 64),
                        IsMultilingual = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DynamicContentItemId);
            
            CreateTable(
                "dbo.DynamicContentItemProperty",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
                        DynamicContentItemId = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.DynamicContentItem", t => t.DynamicContentItemId, cascadeDelete: true)
                .Index(t => t.DynamicContentItemId);
            
            CreateTable(
                "dbo.DynamicContentPlace",
                c => new
                    {
                        DynamicContentPlaceId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DynamicContentPlaceId);
            
            CreateTable(
                "dbo.DynamicContentPublishingGroup",
                c => new
                    {
                        DynamicContentPublishingGroupId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        Priority = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ConditionExpression = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DynamicContentPublishingGroupId);
            
            CreateTable(
                "dbo.PublishingGroupContentItem",
                c => new
                    {
                        PublishingGroupContentItemId = c.String(nullable: false, maxLength: 128),
                        DynamicContentPublishingGroupId = c.String(nullable: false, maxLength: 128),
                        DynamicContentItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PublishingGroupContentItemId)
                .ForeignKey("dbo.DynamicContentItem", t => t.DynamicContentItemId)
                .ForeignKey("dbo.DynamicContentPublishingGroup", t => t.DynamicContentPublishingGroupId, cascadeDelete: true)
                .Index(t => t.DynamicContentItemId)
                .Index(t => t.DynamicContentPublishingGroupId);
            
            CreateTable(
                "dbo.PublishingGroupContentPlace",
                c => new
                    {
                        PublishingGroupContentPlaceId = c.String(nullable: false, maxLength: 128),
                        DynamicContentPublishingGroupId = c.String(nullable: false, maxLength: 128),
                        DynamicContentPlaceId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PublishingGroupContentPlaceId)
                .ForeignKey("dbo.DynamicContentPlace", t => t.DynamicContentPlaceId)
                .ForeignKey("dbo.DynamicContentPublishingGroup", t => t.DynamicContentPublishingGroupId, cascadeDelete: true)
                .Index(t => t.DynamicContentPlaceId)
                .Index(t => t.DynamicContentPublishingGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropForeignKey("dbo.DynamicContentItemProperty", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropIndex("dbo.PublishingGroupContentPlace", new[] { "DynamicContentPublishingGroupId" });
            DropIndex("dbo.PublishingGroupContentPlace", new[] { "DynamicContentPlaceId" });
            DropIndex("dbo.PublishingGroupContentItem", new[] { "DynamicContentPublishingGroupId" });
            DropIndex("dbo.PublishingGroupContentItem", new[] { "DynamicContentItemId" });
            DropIndex("dbo.DynamicContentItemProperty", new[] { "DynamicContentItemId" });
            DropTable("dbo.PublishingGroupContentPlace");
            DropTable("dbo.PublishingGroupContentItem");
            DropTable("dbo.DynamicContentPublishingGroup");
            DropTable("dbo.DynamicContentPlace");
            DropTable("dbo.DynamicContentItemProperty");
            DropTable("dbo.DynamicContentItem");
        }
    }
}
