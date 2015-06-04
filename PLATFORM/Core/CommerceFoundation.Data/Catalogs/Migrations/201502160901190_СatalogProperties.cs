namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class СatalogProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog");
            CreateTable(
                "dbo.CatalogPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
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
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            AddColumn("dbo.Catalog", "PropertySetId", c => c.String(maxLength: 128));
            AddColumn("dbo.PropertySet", "Catalog_CatalogId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PropertySet", "Catalog_CatalogId");
            CreateIndex("dbo.Catalog", "PropertySetId");
            AddForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet", "PropertySetId");
            AddForeignKey("dbo.PropertySet", "Catalog_CatalogId", "dbo.Catalog", "CatalogId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertySet", "Catalog_CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.CatalogPropertyValue", "CatalogId", "dbo.Catalog");
            DropIndex("dbo.Catalog", new[] { "PropertySetId" });
            DropIndex("dbo.CatalogPropertyValue", new[] { "CatalogId" });
            DropIndex("dbo.PropertySet", new[] { "Catalog_CatalogId" });
            DropColumn("dbo.PropertySet", "Catalog_CatalogId");
            DropColumn("dbo.Catalog", "PropertySetId");
            DropTable("dbo.CatalogPropertyValue");
            AddForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog", "CatalogId");
        }
    }
}
