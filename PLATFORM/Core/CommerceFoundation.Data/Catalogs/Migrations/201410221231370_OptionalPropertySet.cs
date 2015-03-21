namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalPropertySet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            AlterColumn("dbo.Item", "PropertySetId", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet", "PropertySetId");
            AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "PropertySetId");

            Sql(@"
ALTER TRIGGER [dbo].[TR_CatalogDeleteTrigger] ON [dbo].[Catalog]
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM [dbo].[Item] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
    DELETE [dbo].[Category] FROM [dbo].[Category] c INNER JOIN [dbo].[CategoryBase] b ON b.CategoryId = c.CategoryId WHERE b.CatalogId IN (SELECT CatalogId FROM [deleted])
    DELETE FROM [dbo].[Property] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
    DELETE FROM [dbo].[PropertySet] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
    DELETE FROM [dbo].[Catalog] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
END");
        }
        
        public override void Down()
        {
            Sql(@"
                ALTER TRIGGER [dbo].[TR_CatalogDeleteTrigger] ON [dbo].[Catalog]
                INSTEAD OF DELETE
                AS
                BEGIN
	                DELETE FROM [dbo].[Property] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
	                DELETE FROM [dbo].[PropertySet] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
	                DELETE FROM [dbo].[Catalog] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
                END");

            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
            DropIndex("dbo.Item","IX_PropertySetId");
            AlterColumn("dbo.Item", "PropertySetId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Item", "PropertySetId");
            AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "PropertySetId", cascadeDelete: true);
            AddForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet", "PropertySetId", cascadeDelete: true);
        }
    }
}
