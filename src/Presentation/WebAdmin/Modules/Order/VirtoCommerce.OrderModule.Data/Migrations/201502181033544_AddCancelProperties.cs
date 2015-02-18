namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.order_CustomerOrder", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.order_CustomerOrder", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.order_CustomerOrder", "CancelReason", c => c.String(maxLength: 2048));
            AddColumn("dbo.order_PaymentIn", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.order_PaymentIn", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.order_PaymentIn", "CancelReason", c => c.String(maxLength: 2048));
            AddColumn("dbo.order_Shipment", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.order_Shipment", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.order_Shipment", "CancelReason", c => c.String(maxLength: 2048));
        }
        
        public override void Down()
        {
            DropColumn("dbo.order_Shipment", "CancelReason");
            DropColumn("dbo.order_Shipment", "CancelledDate");
            DropColumn("dbo.order_Shipment", "IsCancelled");
            DropColumn("dbo.order_PaymentIn", "CancelReason");
            DropColumn("dbo.order_PaymentIn", "CancelledDate");
            DropColumn("dbo.order_PaymentIn", "IsCancelled");
            DropColumn("dbo.order_CustomerOrder", "CancelReason");
            DropColumn("dbo.order_CustomerOrder", "CancelledDate");
            DropColumn("dbo.order_CustomerOrder", "IsCancelled");
        }
    }
}
