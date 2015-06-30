namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetAndImagesCascadeDeletion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CatalogImage", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CatalogAsset", "ItemId", "dbo.Item");
            AlterColumn("dbo.CatalogAsset", "Size", c => c.Long(nullable: false));
            AddForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CatalogImage", "ItemId", "dbo.Item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CatalogAsset", "ItemId", "dbo.Item", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CatalogAsset", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CatalogImage", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase");
            AlterColumn("dbo.CatalogAsset", "Size", c => c.Int(nullable: false));
            AddForeignKey("dbo.CatalogAsset", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.CatalogImage", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase", "Id");
        }
    }
}
