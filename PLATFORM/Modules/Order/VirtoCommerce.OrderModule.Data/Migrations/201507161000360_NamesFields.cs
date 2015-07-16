namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamesFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOrder", "CustomerName", c => c.String(maxLength: 255));
            AddColumn("dbo.CustomerOrder", "StoreName", c => c.String(maxLength: 255));
            AddColumn("dbo.CustomerOrder", "OrganizationName", c => c.String(maxLength: 255));
            AddColumn("dbo.CustomerOrder", "EmployeeName", c => c.String(maxLength: 255));
            AddColumn("dbo.OrderPaymentIn", "OrganizationName", c => c.String(maxLength: 255));
            AddColumn("dbo.OrderPaymentIn", "CustomerName", c => c.String(maxLength: 255));
            AddColumn("dbo.OrderShipment", "OrganizationName", c => c.String(maxLength: 255));
            AddColumn("dbo.OrderShipment", "FulfillmentCenterName", c => c.String(maxLength: 255));
            AddColumn("dbo.OrderShipment", "EmployeeName", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderShipment", "EmployeeName");
            DropColumn("dbo.OrderShipment", "FulfillmentCenterName");
            DropColumn("dbo.OrderShipment", "OrganizationName");
            DropColumn("dbo.OrderPaymentIn", "CustomerName");
            DropColumn("dbo.OrderPaymentIn", "OrganizationName");
            DropColumn("dbo.CustomerOrder", "EmployeeName");
            DropColumn("dbo.CustomerOrder", "OrganizationName");
            DropColumn("dbo.CustomerOrder", "StoreName");
            DropColumn("dbo.CustomerOrder", "CustomerName");
        }
    }
}
