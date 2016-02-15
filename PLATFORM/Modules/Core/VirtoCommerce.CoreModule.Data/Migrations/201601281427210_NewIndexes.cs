namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SeoUrlKeyword", "Keyword");
            CreateIndex("dbo.SeoUrlKeyword", new[] { "ObjectId", "ObjectType" }, name: "ObjectIdAndObjectType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SeoUrlKeyword", "ObjectIdAndObjectType");
            DropIndex("dbo.SeoUrlKeyword", new[] { "Keyword" });
        }
    }
}
