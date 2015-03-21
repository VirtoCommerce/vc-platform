namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FulfillmentMisspeling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.order_Shipment", "FulfillmentCenterId", c => c.String(maxLength: 64));
            DropColumn("dbo.order_Shipment", "FulfilmentCenterId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.order_Shipment", "FulfilmentCenterId", c => c.String(maxLength: 64));
            DropColumn("dbo.order_Shipment", "FulfillmentCenterId");
        }
    }
}
