namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentDiscountAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderShipment", "DiscountAmount", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderShipment", "DiscountAmount");
        }
    }
}
