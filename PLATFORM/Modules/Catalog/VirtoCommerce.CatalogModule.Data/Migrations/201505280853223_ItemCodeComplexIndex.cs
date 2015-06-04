namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemCodeComplexIndex : DbMigration
    {
        public override void Up()
        {
			DropIndex("dbo.Item", new[] { "Code" }, "IX_Code");
			CreateIndex("dbo.Item", new string[] { "Code", "CatalogId" }, unique: true, name: "IX_Code_CatalogId");
        }
        
        public override void Down()
        {
			DropIndex("dbo.Item", new[] { "Code" }, "IX_Code_CatalogId");
			CreateIndex("dbo.Item", "Code", unique: true, name: "IX_Code");
        }
    }
}
