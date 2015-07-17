namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatesStatusChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderPaymentIn", "AuthorizedDate", c => c.DateTime());
            AddColumn("dbo.OrderPaymentIn", "CapturedDate", c => c.DateTime());
            AddColumn("dbo.OrderPaymentIn", "VoidedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderPaymentIn", "VoidedDate");
            DropColumn("dbo.OrderPaymentIn", "CapturedDate");
            DropColumn("dbo.OrderPaymentIn", "AuthorizedDate");
        }
    }
}
