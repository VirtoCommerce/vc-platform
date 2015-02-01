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
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        Name = c.String(),
                        SiteId = c.String(),
                        IsAnonymous = c.Boolean(nullable: false),
                        CustomerId = c.String(),
                        CustomerName = c.String(),
                        OrganizationId = c.String(),
                        Currency = c.String(),
                        Coupon = c.String(),
                        LanguageCode = c.String(),
                        TaxIncluded = c.Boolean(nullable: false),
                        IsRecuring = c.Boolean(nullable: false),
                        Note = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HandlingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.cart_Address",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AddressType = c.String(),
                        Organization = c.String(),
                        CountryCode = c.String(),
                        CountryName = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Zip = c.String(),
                        ShoppingCartId = c.String(maxLength: 128),
                        ShipmentId = c.String(),
                        PaymentId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.cart_Payment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OuterId = c.String(),
                        Currency = c.String(),
                        PaymentGatewayCode = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShoppingCartId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId)
                .ForeignKey("dbo.cart_Address", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.cart_Shipment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ShipmentMethodCode = c.String(),
                        WarehouseLocation = c.String(),
                        Currency = c.String(),
                        WeightUnit = c.String(),
                        WeightValue = c.Decimal(precision: 18, scale: 2),
                        VolumetricWeight = c.Decimal(precision: 18, scale: 2),
                        DimensionUnit = c.String(),
                        DimensionHeight = c.Decimal(precision: 18, scale: 2),
                        DimensionLength = c.Decimal(precision: 18, scale: 2),
                        DimensionWidth = c.Decimal(precision: 18, scale: 2),
                        TaxIncluded = c.Boolean(nullable: false),
                        ShippingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShoppingCartId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cart_ShoppingCart", t => t.ShoppingCartId, cascadeDelete: true)
                .ForeignKey("dbo.cart_Address", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.cart_LineItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductId = c.String(),
                        CatalogId = c.String(),
                        CategoryId = c.String(),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        FulfilmentLocationCode = c.String(),
                        ShipmentMethodCode = c.String(),
                        RequiredShipping = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                        IsGift = c.Boolean(nullable: false),
                        Currency = c.String(),
                        LanguageCode = c.String(),
                        Note = c.String(),
                        IsReccuring = c.Boolean(nullable: false),
                        TaxIncluded = c.Boolean(nullable: false),
                        WeightUnit = c.String(),
                        WeightValue = c.Decimal(precision: 18, scale: 2),
                        VolumetricWeight = c.Decimal(precision: 18, scale: 2),
                        DimensionUnit = c.String(),
                        DimensionHeight = c.Decimal(precision: 18, scale: 2),
                        DimensionLength = c.Decimal(precision: 18, scale: 2),
                        DimensionWidth = c.Decimal(precision: 18, scale: 2),
                        ListPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlacedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShoppingCartId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
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
            DropForeignKey("dbo.cart_Shipment", "Id", "dbo.cart_Address");
            DropForeignKey("dbo.cart_Shipment", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropForeignKey("dbo.cart_LineItem", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropForeignKey("dbo.cart_LineItem", "ShipmentId", "dbo.cart_Shipment");
            DropForeignKey("dbo.cart_Payment", "Id", "dbo.cart_Address");
            DropForeignKey("dbo.cart_Payment", "ShoppingCartId", "dbo.cart_ShoppingCart");
            DropIndex("dbo.cart_LineItem", new[] { "ShipmentId" });
            DropIndex("dbo.cart_LineItem", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Shipment", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Shipment", new[] { "Id" });
            DropIndex("dbo.cart_Payment", new[] { "ShoppingCartId" });
            DropIndex("dbo.cart_Payment", new[] { "Id" });
            DropIndex("dbo.cart_Address", new[] { "ShoppingCartId" });
            DropTable("dbo.cart_LineItem");
            DropTable("dbo.cart_Shipment");
            DropTable("dbo.cart_Payment");
            DropTable("dbo.cart_Address");
            DropTable("dbo.cart_ShoppingCart");
        }
    }
}
