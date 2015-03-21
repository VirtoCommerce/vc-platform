namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditorialReviewLocale_SeoTriggers : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Property", new[] { "Name", "CatalogId" }, true);
            AddColumn("dbo.Property", "TargetType", c => c.String(maxLength: 128));
            AddColumn("dbo.EditorialReview", "Locale", c => c.String(maxLength: 64));

            Sql(@"
                ALTER TRIGGER [TR_CategoryDeleteTrigger] ON [dbo].[Category]
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
                        IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SeoUrlKeyword'))
	                    BEGIN
		                    DELETE FROM [dbo].[SeoUrlKeyword] WHERE KeywordType = 0 AND KeywordValue IN (SELECT CategoryId FROM [deleted])
	                    END
	                END
                END");

            Sql(@"
                CREATE TRIGGER [dbo].[TR_ItemDeleteTrigger] ON [dbo].[Item] 
                FOR DELETE AS
                BEGIN
	                IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SeoUrlKeyword'))
	                BEGIN
		                DELETE FROM [dbo].[SeoUrlKeyword] WHERE KeywordType = 1 AND KeywordValue IN (SELECT ItemId FROM [deleted])
	                END
                END");
        }
        
        public override void Down()
        {
            Sql(@"
                ALTER TRIGGER [TR_CategoryDeleteTrigger] ON [dbo].[Category]
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

            DropColumn("dbo.EditorialReview", "Locale");
            DropColumn("dbo.Property", "TargetType");
            DropIndex("dbo.Property", new[] { "Name", "CatalogId" });
        }
    }
}
