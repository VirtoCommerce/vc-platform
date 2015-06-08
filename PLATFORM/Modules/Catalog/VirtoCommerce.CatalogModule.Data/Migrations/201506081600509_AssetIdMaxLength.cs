namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetIdMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemAsset", "AssetId", c => c.String(nullable: false, maxLength: 2083));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ItemAsset", "AssetId", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
