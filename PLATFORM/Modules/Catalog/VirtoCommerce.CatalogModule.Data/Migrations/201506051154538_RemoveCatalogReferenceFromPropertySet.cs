namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCatalogReferenceFromPropertySet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PropertySet", "Catalog_Id", "dbo.Catalog");
            DropForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet");
            DropIndex("dbo.PropertySet", new[] { "CatalogId" });
            DropIndex("dbo.PropertySet", new[] { "Catalog_Id" });
            AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet", "Id", cascadeDelete: true);
            DropColumn("dbo.PropertySet", "CatalogId");
            DropColumn("dbo.PropertySet", "Catalog_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PropertySet", "Catalog_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PropertySet", "CatalogId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            CreateIndex("dbo.PropertySet", "Catalog_Id");
            CreateIndex("dbo.PropertySet", "CatalogId");
            AddForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet", "Id");
            AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "Id");
            AddForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog", "Id");
            AddForeignKey("dbo.PropertySet", "Catalog_Id", "dbo.Catalog", "Id");
        }
    }
}
