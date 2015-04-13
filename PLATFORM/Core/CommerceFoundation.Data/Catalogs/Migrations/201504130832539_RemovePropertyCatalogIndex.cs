namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePropertyCatalogIndex : DbMigration
    {
        public override void Up()
        {
			DropIndex("dbo.Property", "IX_Name_CatalogId");
        }
        
        public override void Down()
        {
        }
    }
}
