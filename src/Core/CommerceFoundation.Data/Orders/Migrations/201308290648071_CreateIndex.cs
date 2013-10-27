namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIndex : DbMigration
    {
		public override void Up()
		{
			CreateIndex("OrderGroup", "Discriminator");
			CreateIndex("OrderGroup", "Created");
			CreateIndex("OrderGroup", "StoreId");
		}

		public override void Down()
		{
			DropIndex("OrderGroup", new[] { "Discriminator" });
			DropIndex("OrderGroup", new[] { "Created" });
			DropIndex("OrderGroup", new[] { "StoreId" });
		}
    }
}
