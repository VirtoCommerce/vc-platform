namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DynamicContentItemProperty", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropIndex("dbo.DynamicContentItemProperty", new[] { "DynamicContentItemId" });
            DropTable("dbo.DynamicContentItemProperty");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.DynamicContentItemProperty", "DynamicContentItemId");
            AddForeignKey("dbo.DynamicContentItemProperty", "DynamicContentItemId", "dbo.DynamicContentItem", "Id", cascadeDelete: true);
        }
    }
}
