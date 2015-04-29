namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cart_ShoppingCart",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 64),
                        ChannelId = c.String(maxLength: 64),
                        IsAnonymous = c.Boolean(nullable: false),
                        CustomerId = c.String(nullable: false, maxLength: 64),
                        CustomerName = c.String(maxLength: 128),
                        OrganizationId = c.String(maxLength: 64),
                        Currency = c.String(nullable: false, maxLength: 3),
                        Coupon = c.String(maxLength: 64),
                        LanguageCode = c.String(maxLength: 16),
                        TaxIncluded = c.Boolean(nullable: false),
                        IsRecuring = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 2048),
                        Total = c.Decimal(nullable: false, storeType: "money"),
                        SubTotal = c.Decimal(nullable: false, storeType: "money"),
                        ShippingTotal = c.Decimal(nullable: false, storeType: "money"),
                        HandlingTotal = c.Decimal(nullable: false, storeType: "money"),
                        DiscountTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxTotal = c.Decimal(nullable: false, storeType: "money"),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.cart_Address",
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
                        ShoppingCartId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        PaymentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_Payment", t => t.PaymentId)
                .ForeignKey("dbo.cart_Shipment", t => t.ShipmentId)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId)
                .Index(t => t.ShoppingCartId)
                .Index(t => t.ShipmentId)
                .Index(t => t.PaymentId);
            
            CreateTable(
                "dbo.cart_Payment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OuterId = c.String(maxLength: 128),
                        Currency = c.String(nullable: false, maxLength: 64),
                        GatewayCode = c.String(maxLength: 64),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Purpose = c.String(maxLength: 1024),
                        ShoppingCartId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.cart_Shipment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ShipmentMethodCode = c.String(maxLength: 64),
                        FulfilmentCenterId = c.String(maxLength: 64),
                        Currency = c.String(nullable: false, maxLength: 3),
                        WeightUnit = c.String(maxLength: 16),
                        WeightValue = c.Decimal(precision: 18, scale: 2),
                        VolumetricWeight = c.Decimal(precision: 18, scale: 2),
                        DimensionUnit = c.String(maxLength: 16),
                        DimensionHeight = c.Decimal(precision: 18, scale: 2),
                        DimensionLength = c.Decimal(precision: 18, scale: 2),
                        DimensionWidth = c.Decimal(precision: 18, scale: 2),
                        TaxIncluded = c.Boolean(nullable: false),
                        ShippingPrice = c.Decimal(nullable: false, storeType: "money"),
                        DiscountTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxTotal = c.Decimal(nullable: false, storeType: "money"),
                        ShoppingCartId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.cart_LineItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Currency = c.String(nullable: false, maxLength: 3),
                        ProductId = c.String(nullable: false, maxLength: 64),
                        CatalogId = c.String(nullable: false, maxLength: 64),
                        CategoryId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        Quantity = c.Int(nullable: false),
                        FulfilmentLocationCode = c.String(maxLength: 64),
                        ShipmentMethodCode = c.String(maxLength: 64),
                        RequiredShipping = c.Boolean(nullable: false),
                        ImageUrl = c.String(maxLength: 1028),
                        IsGift = c.Boolean(nullable: false),
                        LanguageCode = c.String(maxLength: 16),
                        Comment = c.String(maxLength: 2048),
                        IsReccuring = c.Boolean(nullable: false),
                        TaxIncluded = c.Boolean(nullable: false),
                        WeightUnit = c.String(maxLength: 16),
                        WeightValue = c.Decimal(precision: 18, scale: 2),
                        VolumetricWeight = c.Decimal(precision: 18, scale: 2),
                        DimensionUnit = c.String(maxLength: 16),
                        DimensionHeight = c.Decimal(precision: 18, scale: 2),
                        DimensionLength = c.Decimal(precision: 18, scale: 2),
                        DimensionWidth = c.Decimal(precision: 18, scale: 2),
                        ListPrice = c.Decimal(nullable: false, storeType: "money"),
                        SalePrice = c.Decimal(nullable: false, storeType: "money"),
                        PlacedPrice = c.Decimal(nullable: false, storeType: "money"),
                        ExtendedPrice = c.Decimal(nullable: false, storeType: "money"),
                        DiscountTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxTotal = c.Decimal(nullable: false, storeType: "money"),
                        ShoppingCartId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_Shipment", t => t.ShipmentId)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId)
                .Index(t => t.ShoppingCartId)
                .Index(t => t.ShipmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.cart_Address", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropForeignKey("dbo.cart_Address", "ShipmentId", "dbo.cart_Shipment");
            DropForeignKey("dbo.cart_Shipment", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropForeignKey("dbo.cart_LineItem", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropForeignKey("dbo.cart_LineItem", "ShipmentId", "dbo.cart_Shipment");
            DropForeignKey("dbo.cart_Address", "PaymentId", "dbo.cart_Payment");
            DropForeignKey("dbo.cart_Payment", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropIndex("dbo.cart_LineItem", new[] { "ShipmentId" });
            DropIndex("dbo.cart_LineItem", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Shipment", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Payment", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Address", new[] { "PaymentId" });
            DropIndex("dbo.cart_Address", new[] { "ShipmentId" });
            DropIndex("dbo.cart_Address", new[] { "ShoppingCartId" });
            DropTable("dbo.cart_LineItem");
            DropTable("dbo.cart_Shipment");
            DropTable("dbo.cart_Payment");
            DropTable("dbo.cart_Address");
            DropTable("dbo.cart_ShoppingCart");
        }
    }
}
