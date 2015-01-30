namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertiesChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.order_CustomerOrder", "CustomerId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "SiteId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "OrganizationId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "EmployeeId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "OrganizationId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "CustomerId", c => c.String());
            AddColumn("dbo.order_Shipment", "OrganizationId", c => c.String());
            AddColumn("dbo.order_Shipment", "FulfilmentCenterId", c => c.String());
            AddColumn("dbo.order_Shipment", "EmployeeId", c => c.String());
            DropColumn("dbo.order_CustomerOrder", "SourceStoreId");
            DropColumn("dbo.order_CustomerOrder", "TargetStoreId");
            DropColumn("dbo.order_CustomerOrder", "SourceAgentId");
            DropColumn("dbo.order_CustomerOrder", "TargetAgentId");
            DropColumn("dbo.order_PaymentIn", "SourceStoreId");
            DropColumn("dbo.order_PaymentIn", "TargetStoreId");
            DropColumn("dbo.order_PaymentIn", "SourceAgentId");
            DropColumn("dbo.order_PaymentIn", "TargetAgentId");
            DropColumn("dbo.order_Shipment", "SourceStoreId");
            DropColumn("dbo.order_Shipment", "TargetStoreId");
            DropColumn("dbo.order_Shipment", "SourceAgentId");
            DropColumn("dbo.order_Shipment", "TargetAgentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.order_Shipment", "TargetAgentId", c => c.String());
            AddColumn("dbo.order_Shipment", "SourceAgentId", c => c.String());
            AddColumn("dbo.order_Shipment", "TargetStoreId", c => c.String());
            AddColumn("dbo.order_Shipment", "SourceStoreId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "TargetAgentId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "SourceAgentId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "TargetStoreId", c => c.String());
            AddColumn("dbo.order_PaymentIn", "SourceStoreId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "TargetAgentId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "SourceAgentId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "TargetStoreId", c => c.String());
            AddColumn("dbo.order_CustomerOrder", "SourceStoreId", c => c.String());
            DropColumn("dbo.order_Shipment", "EmployeeId");
            DropColumn("dbo.order_Shipment", "FulfilmentCenterId");
            DropColumn("dbo.order_Shipment", "OrganizationId");
            DropColumn("dbo.order_PaymentIn", "CustomerId");
            DropColumn("dbo.order_PaymentIn", "OrganizationId");
            DropColumn("dbo.order_CustomerOrder", "EmployeeId");
            DropColumn("dbo.order_CustomerOrder", "OrganizationId");
            DropColumn("dbo.order_CustomerOrder", "SiteId");
            DropColumn("dbo.order_CustomerOrder", "CustomerId");
        }
    }
}
