namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.order_CustomerOrder",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 64),
                        ChannelId = c.String(maxLength: 64),
                        OrganizationId = c.String(maxLength: 64),
                        EmployeeId = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.order_Address",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AddressType = c.String(maxLength: 32),
                        Organization = c.String(maxLength: 64),
                        CountryCode = c.String(nullable: false, maxLength: 3),
                        CountryName = c.String(nullable: false, maxLength: 64),
                        City = c.String(nullable: false, maxLength: 128),
                        PostalCode = c.String(maxLength: 64),
                        Line1 = c.String(maxLength: 2048),
                        Line2 = c.String(maxLength: 2048),
                        FirstName = c.String(nullable: false, maxLength: 64),
                        LastName = c.String(nullable: false, maxLength: 64),
                        Phone = c.String(maxLength: 64),
                        Email = c.String(maxLength: 64),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        PaymentInId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.order_PaymentIn", t => t.PaymentInId)
                .ForeignKey("dbo.order_Shipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId)
                .Index(t => t.PaymentInId);
            
            CreateTable(
                "dbo.order_PaymentIn",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.String(maxLength: 64),
                        CustomerId = c.String(nullable: false, maxLength: 64),
                        IncomingDate = c.DateTime(),
                        OuterId = c.String(maxLength: 128),
                        Purpose = c.String(maxLength: 1024),
                        GatewayCode = c.String(maxLength: 64),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.order_Shipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.order_Shipment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.String(maxLength: 64),
                        FulfilmentCenterId = c.String(maxLength: 64),
                        EmployeeId = c.String(maxLength: 64),
                        ShipmentMethodCode = c.String(maxLength: 64),
                        WeightUnit = c.String(maxLength: 16),
                        WeightValue = c.Decimal(precision: 18, scale: 2),
                        VolumetricWeight = c.Decimal(precision: 18, scale: 2),
                        DimensionUnit = c.String(maxLength: 16),
                        DimensionHeight = c.Decimal(precision: 18, scale: 2),
                        DimensionLength = c.Decimal(precision: 18, scale: 2),
                        DimensionWidth = c.Decimal(precision: 18, scale: 2),
                        CustomerOrderId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrderId, cascadeDelete: true)
                .Index(t => t.CustomerOrderId);
            
            CreateTable(
                "dbo.order_Discount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(maxLength: 64),
                        PromotionDescription = c.String(maxLength: 1024),
                        Currency = c.String(nullable: false, maxLength: 3),
                        DiscountAmount = c.Decimal(nullable: false, storeType: "money"),
                        CouponCode = c.String(maxLength: 64),
                        CouponInvalidDescription = c.String(maxLength: 1024),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        LineItemId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.order_LineItem", t => t.LineItemId)
                .ForeignKey("dbo.order_Shipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "dbo.order_LineItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                        Currency = c.String(nullable: false, maxLength: 3),
                        BasePrice = c.Decimal(nullable: false, storeType: "money"),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        DiscountAmount = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.String(nullable: false, maxLength: 64),
                        CatalogId = c.String(nullable: false, maxLength: 64),
                        CategoryId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        Comment = c.String(maxLength: 2048),
                        IsReccuring = c.Boolean(nullable: false),
                        ImageUrl = c.String(maxLength: 1028),
                        IsGift = c.Boolean(nullable: false),
                        ShippingMethodCode = c.String(maxLength: 64),
                        FulfilmentLocationCode = c.String(maxLength: 64),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.order_Shipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.order_Address", "ShipmentId", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Address", "PaymentInId", "dbo.order_PaymentIn");
            DropForeignKey("dbo.order_PaymentIn", "ShipmentId", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Discount", "ShipmentId", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Discount", "LineItemId", "dbo.order_LineItem");
            DropForeignKey("dbo.order_LineItem", "ShipmentId", "dbo.order_Shipment");
            DropForeignKey("dbo.order_LineItem", "CustomerOrderId", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Discount", "CustomerOrderId", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Shipment", "CustomerOrderId", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_PaymentIn", "CustomerOrderId", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Address", "CustomerOrderId", "dbo.order_CustomerOrder");
            DropIndex("dbo.order_LineItem", new[] { "ShipmentId" });
            DropIndex("dbo.order_LineItem", new[] { "CustomerOrderId" });
            DropIndex("dbo.order_Discount", new[] { "LineItemId" });
            DropIndex("dbo.order_Discount", new[] { "ShipmentId" });
            DropIndex("dbo.order_Discount", new[] { "CustomerOrderId" });
            DropIndex("dbo.order_Shipment", new[] { "CustomerOrderId" });
            DropIndex("dbo.order_PaymentIn", new[] { "ShipmentId" });
            DropIndex("dbo.order_PaymentIn", new[] { "CustomerOrderId" });
            DropIndex("dbo.order_Address", new[] { "PaymentInId" });
            DropIndex("dbo.order_Address", new[] { "ShipmentId" });
            DropIndex("dbo.order_Address", new[] { "CustomerOrderId" });
            DropTable("dbo.order_LineItem");
            DropTable("dbo.order_Discount");
            DropTable("dbo.order_Shipment");
            DropTable("dbo.order_PaymentIn");
            DropTable("dbo.order_Address");
            DropTable("dbo.order_CustomerOrder");
        }
    }
}
