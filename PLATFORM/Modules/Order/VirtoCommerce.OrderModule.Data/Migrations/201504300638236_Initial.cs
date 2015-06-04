namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOrder",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 64),
                        ChannelId = c.String(maxLength: 64),
                        OrganizationId = c.String(maxLength: 64),
                        EmployeeId = c.String(maxLength: 64),
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(),
                        CancelReason = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderAddress",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AddressType = c.String(maxLength: 32),
                        Organization = c.String(maxLength: 64),
                        CountryCode = c.String(maxLength: 3),
                        CountryName = c.String(nullable: false, maxLength: 64),
                        City = c.String(nullable: false, maxLength: 128),
                        PostalCode = c.String(maxLength: 64),
                        Line1 = c.String(maxLength: 2048),
                        Line2 = c.String(maxLength: 2048),
                        RegionId = c.String(maxLength: 128),
                        RegionName = c.String(maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 64),
                        LastName = c.String(nullable: false, maxLength: 64),
                        Phone = c.String(maxLength: 64),
                        Email = c.String(maxLength: 64),
                        CustomerOrderId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        PaymentInId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.OrderPaymentIn", t => t.PaymentInId)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId)
                .Index(t => t.PaymentInId);
            
            CreateTable(
                "dbo.OrderPaymentIn",
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
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(),
                        CancelReason = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.OrderShipment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.String(maxLength: 64),
                        FulfillmentCenterId = c.String(maxLength: 64),
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
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(),
                        CancelReason = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId, cascadeDelete: true)
                .Index(t => t.CustomerOrderId);
            
            CreateTable(
                "dbo.OrderDiscount",
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
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.OrderLineItem", t => t.LineItemId)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "dbo.OrderLineItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShipmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderAddress", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderAddress", "PaymentInId", "dbo.OrderPaymentIn");
            DropForeignKey("dbo.OrderPaymentIn", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderDiscount", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderDiscount", "LineItemId", "dbo.OrderLineItem");
            DropForeignKey("dbo.OrderLineItem", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderLineItem", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.OrderDiscount", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.OrderShipment", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.OrderPaymentIn", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.OrderAddress", "CustomerOrderId", "dbo.CustomerOrder");
            DropIndex("dbo.OrderLineItem", new[] { "ShipmentId" });
            DropIndex("dbo.OrderLineItem", new[] { "CustomerOrderId" });
            DropIndex("dbo.OrderDiscount", new[] { "LineItemId" });
            DropIndex("dbo.OrderDiscount", new[] { "ShipmentId" });
            DropIndex("dbo.OrderDiscount", new[] { "CustomerOrderId" });
            DropIndex("dbo.OrderShipment", new[] { "CustomerOrderId" });
            DropIndex("dbo.OrderPaymentIn", new[] { "ShipmentId" });
            DropIndex("dbo.OrderPaymentIn", new[] { "CustomerOrderId" });
            DropIndex("dbo.OrderAddress", new[] { "PaymentInId" });
            DropIndex("dbo.OrderAddress", new[] { "ShipmentId" });
            DropIndex("dbo.OrderAddress", new[] { "CustomerOrderId" });
            DropTable("dbo.OrderLineItem");
            DropTable("dbo.OrderDiscount");
            DropTable("dbo.OrderShipment");
            DropTable("dbo.OrderPaymentIn");
            DropTable("dbo.OrderAddress");
            DropTable("dbo.CustomerOrder");
        }
    }
}
