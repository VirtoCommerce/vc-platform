namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentTotalsFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartShipment", "Total", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartShipment", "Total");
        }
    }
}
