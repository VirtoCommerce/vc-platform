namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoreIdToSeoKeywords : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SeoUrlKeyword", new[] { "Keyword" });
            AddColumn("dbo.SeoUrlKeyword", "StoreId", c => c.String(maxLength: 128));
            CreateIndex("dbo.SeoUrlKeyword", new[] { "Keyword", "StoreId" }, name: "KeywordStoreId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SeoUrlKeyword", "KeywordStoreId");
            DropColumn("dbo.SeoUrlKeyword", "StoreId");
            CreateIndex("dbo.SeoUrlKeyword", "Keyword");
        }
    }
}
