namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Indexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderGroup", "CustomerId");
        }

        public override void Down()
        {
            DropIndex("dbo.OrderGroup", new[] { "CustomerId" });
        }
    }
}
