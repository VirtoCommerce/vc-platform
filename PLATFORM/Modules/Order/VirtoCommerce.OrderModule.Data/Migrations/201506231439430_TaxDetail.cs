namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderTaxDetail",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Name = c.String(maxLength: 1024),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        LineItemId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.OrderLineItem", t => t.LineItemId, cascadeDelete: true)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId)
                .Index(t => t.LineItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderTaxDetail", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderTaxDetail", "LineItemId", "dbo.OrderLineItem");
            DropForeignKey("dbo.OrderTaxDetail", "CustomerOrderId", "dbo.CustomerOrder");
            DropIndex("dbo.OrderTaxDetail", new[] { "LineItemId" });
            DropIndex("dbo.OrderTaxDetail", new[] { "ShipmentId" });
            DropIndex("dbo.OrderTaxDetail", new[] { "CustomerOrderId" });
            DropTable("dbo.OrderTaxDetail");
        }
    }
}
