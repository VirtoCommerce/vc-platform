namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIndexes : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateIndex("Item", new[] { "Code", "CatalogId" }, true);
			CreateIndex("CategoryBase", new[] { "Code", "CatalogId" }, true);
            CreateIndex("TaxCategory", "Name", true);
            CreateIndex("Item", "Discriminator");
            CreateIndex("Item", "LastModified");
			CreateIndex("CategoryItemRelation", "Discriminator");
			CreateIndex("Price", "Discriminator");
        }
        
        /// <summary>
        /// Add down migration (drop indexes) here. Use index column names array as index name doesn't work.
        /// </summary>
        public override void Down()
        {
            DropIndex("Item", new[] { "Code", "CatalogId" });
			DropIndex("CategoryBase", new[] { "Code", "CatalogId" });
            DropIndex("TaxCategory", new[] { "Name" });
            DropIndex("Item", new [] {"Discriminator"});
            DropIndex("Item", new [] {"LastModified"});
			DropIndex("Price", new[] { "Discriminator" });
        }
    }
}
