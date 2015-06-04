namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePaymentGatewayCodeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartPayment", "PaymentGatewayCode", c => c.String(maxLength: 64));
            DropColumn("dbo.CartPayment", "GatewayCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartPayment", "GatewayCode", c => c.String(maxLength: 64));
            DropColumn("dbo.CartPayment", "PaymentGatewayCode");
        }
    }
}
