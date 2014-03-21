namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTotals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderForm", "LineItemDiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderForm", "ShipmentDiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderGroup", "DiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderGroup", "FormDiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderGroup", "LineItemDiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderGroup", "ShipmentDiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderGroup", "ShipmentDiscountTotal");
            DropColumn("dbo.OrderGroup", "LineItemDiscountTotal");
            DropColumn("dbo.OrderGroup", "FormDiscountTotal");
            DropColumn("dbo.OrderGroup", "DiscountTotal");
            DropColumn("dbo.OrderForm", "ShipmentDiscountAmount");
            DropColumn("dbo.OrderForm", "LineItemDiscountAmount");
        }
    }
}
