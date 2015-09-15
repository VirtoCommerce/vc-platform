namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkuAndShipmentOption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartShipment", "ShipmentMethodOption", c => c.String(maxLength: 64));
            AddColumn("dbo.CartLineItem", "Sku", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartLineItem", "Sku");
            DropColumn("dbo.CartShipment", "ShipmentMethodOption");
        }
    }
}
