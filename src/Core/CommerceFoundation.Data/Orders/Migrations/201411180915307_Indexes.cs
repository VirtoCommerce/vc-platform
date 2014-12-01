namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Indexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderGroup", "CustomerId");
            CreateIndex("dbo.OrderGroup", "StoreId");
            CreateIndex("dbo.OrderGroup", "Discriminator");
        }

        public override void Down()
        {
            DropIndex("dbo.OrderGroup", new[] { "Discriminator" });
            DropIndex("dbo.OrderGroup", new[] { "StoreId" });
            DropIndex("dbo.OrderGroup", new[] { "CustomerId" });
        }
    }
}
