namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxTypeToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderShipment", "TaxType", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderLineItem", "TaxType", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderLineItem", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderLineItem", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.OrderLineItem", "CancelReason", c => c.String(maxLength: 2048));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItem", "CancelReason");
            DropColumn("dbo.OrderLineItem", "CancelledDate");
            DropColumn("dbo.OrderLineItem", "IsCancelled");
            DropColumn("dbo.OrderLineItem", "TaxType");
            DropColumn("dbo.OrderShipment", "TaxType");
        }
    }
}
