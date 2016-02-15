namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCatalogIdAndParentIdIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Item", new[] { "CatalogId" });
            DropIndex("dbo.Item", new[] { "ParentId" });
            CreateIndex("dbo.Item", new[] { "CatalogId", "ParentId" }, name: "CatalogIdAndParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Item", "CatalogIdAndParentId");
            CreateIndex("dbo.Item", "ParentId");
            CreateIndex("dbo.Item", "CatalogId");
        }
    }
}
