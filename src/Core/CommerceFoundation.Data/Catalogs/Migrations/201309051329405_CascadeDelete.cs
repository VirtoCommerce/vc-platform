namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class CascadeDelete : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
			DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
			DropForeignKey("dbo.Category", "CategoryId", "dbo.CategoryBase");
			DropIndex("dbo.Item", new[] { "PropertySetId" });
			DropIndex("dbo.Category", new[] { "PropertySetId" });
			DropIndex("dbo.Category", new[] { "CategoryId" });
			AlterColumn("dbo.Item", "PropertySetId", c => c.String(nullable: false, maxLength: 128));
			AddForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet", "PropertySetId", cascadeDelete: true);
			AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "PropertySetId", cascadeDelete: true);
			AddForeignKey("dbo.Category", "CategoryId", "dbo.CategoryBase", "CategoryId", cascadeDelete: true);
			CreateIndex("dbo.Item", "PropertySetId");
			CreateIndex("dbo.Category", "PropertySetId");
			CreateIndex("dbo.Category", "CategoryId");

            //Trigger for deleting categories
			Sql(@"
CREATE TRIGGER [TR_CategoryDeleteTrigger] ON [dbo].[Category]
FOR DELETE
AS
BEGIN

IF(EXISTS((SELECT CategoryId FROM [deleted])))
	BEGIN

		DECLARE @TempParentCategoryId TABLE
		(
		   CategoryId nvarchar(128)
		);

		INSERT INTO @TempParentCategoryId 
		SELECT CategoryId FROM [dbo].[CategoryBase] 
		WHERE ParentCategoryId IN (SELECT CategoryId FROM [deleted])

		DELETE FROM [dbo].[Category] WHERE CategoryId IN (SELECT CategoryId FROM @TempParentCategoryId)
		DELETE FROM [dbo].[LinkedCategory] WHERE LinkedCategoryId IN (SELECT CategoryId FROM [deleted])
		DELETE FROM [dbo].[LinkedCategory] WHERE CategoryId IN (SELECT CategoryId FROM @TempParentCategoryId)
		DELETE FROM [dbo].[CategoryItemRelation] WHERE CategoryId IN (SELECT CategoryId FROM [deleted])
		DELETE FROM [dbo].[CategoryItemRelation] WHERE CategoryId IN (SELECT CategoryId FROM @TempParentCategoryId)
		DELETE FROM [dbo].[CategoryBase] WHERE ParentCategoryId IN (SELECT CategoryId FROM [deleted])
	END
END");
            //Trigger for deleting Catalog
            Sql(@"CREATE TRIGGER [dbo].[TR_CatalogDeleteTrigger] ON [dbo].[Catalog]
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM [dbo].[Property] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
	DELETE FROM [dbo].[PropertySet] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
	DELETE FROM [dbo].[Catalog] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
END");

            //Trigger for deleting VirtualCatalog
		    Sql(@"CREATE TRIGGER [dbo].[TR_VirtualCatalogDeleteTrigger] ON [dbo].[VirtualCatalog]
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM [dbo].[VirtualCatalog] WHERE CatalogId IN (SELECT CatalogId FROM [deleted])
	DELETE FROM [dbo].[LinkedCategory] WHERE CategoryId IN (SELECT CategoryId FROM [dbo].[CategoryBase] WHERE CatalogId IN (SELECT CatalogId FROM [deleted]))
END");
		}

		public override void Down()
		{
			DropIndex("dbo.Category", new[] { "PropertySetId" });
			DropIndex("dbo.Item", new[] { "PropertySetId" });
			DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
			DropIndex("dbo.Category", new[] { "CategoryId" });
			DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
			DropForeignKey("dbo.Category", "CategoryId", "dbo.CategoryBase");
			AlterColumn("dbo.Item", "PropertySetId", c => c.String(maxLength: 128));
			CreateIndex("dbo.Category", "PropertySetId");
			CreateIndex("dbo.Item", "PropertySetId");
			AddForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet", "PropertySetId");
			CreateIndex("dbo.Category", "CategoryId");
			AddForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet", "PropertySetId");
			AddForeignKey("dbo.Category", "CategoryId", "dbo.CategoryBase", "CategoryId");

			Sql(@"DROP TRIGGER [dbo].[TR_CategoryDeleteTrigger]");
            Sql(@"DROP TRIGGER [dbo].[TR_CatalogDeleteTrigger]");
            Sql(@"DROP TRIGGER [dbo].[TR_VirtualCatalogDeleteTrigger]");
		}
	}
}
