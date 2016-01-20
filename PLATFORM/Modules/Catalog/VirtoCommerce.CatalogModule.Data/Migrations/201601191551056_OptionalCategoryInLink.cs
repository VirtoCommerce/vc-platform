namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalCategoryInLink : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CategoryItemRelation", new[] { "CategoryId" });
            AlterColumn("dbo.CategoryItemRelation", "CategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CategoryItemRelation", "CategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CategoryItemRelation", new[] { "CategoryId" });
            AlterColumn("dbo.CategoryItemRelation", "CategoryId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.CategoryItemRelation", "CategoryId");
        }
    }
}
