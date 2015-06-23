namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeoRenaming : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeoUrlKeyword", "ObjectId", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.SeoUrlKeyword", "ObjectType", c => c.String(nullable: false, maxLength: 64));

			Sql("Update SeoUrlKeyword SET ObjectId = KeywordValue");
			Sql("Update SeoUrlKeyword SET ObjectType = N'CatalogProduct' where  KeywordType = 1");
			Sql("Update SeoUrlKeyword SET ObjectType = N'Category' where  KeywordType = 0");
			Sql("Update SeoUrlKeyword SET ObjectType = N'Store' where  KeywordType = 2");

            DropColumn("dbo.SeoUrlKeyword", "KeywordValue");
            DropColumn("dbo.SeoUrlKeyword", "KeywordType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SeoUrlKeyword", "KeywordType", c => c.Int(nullable: false));
            AddColumn("dbo.SeoUrlKeyword", "KeywordValue", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.SeoUrlKeyword", "ObjectType");
            DropColumn("dbo.SeoUrlKeyword", "ObjectId");
        }
    }
}
