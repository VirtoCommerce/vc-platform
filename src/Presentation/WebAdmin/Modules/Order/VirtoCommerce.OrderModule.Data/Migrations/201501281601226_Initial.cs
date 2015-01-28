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
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        Number = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(),
                        SourceStoreId = c.String(),
                        TargetStoreId = c.String(),
                        SourceAgentId = c.String(),
                        TargetAgentId = c.String(),
                        Comment = c.String(),
                        Currency = c.String(),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.order_Address",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AddressType = c.Int(nullable: false),
                        Organization = c.String(),
                        CountryCode = c.String(),
                        CountryName = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        CustomerOrder_Id = c.String(maxLength: 128),
                        Shipment_Id = c.String(),
                        PaymentIn_Id = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrder_Id)
                .Index(t => t.CustomerOrder_Id);
            
            CreateTable(
                "dbo.order_PaymentIn",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IncomingDate = c.DateTime(),
                        OuterId = c.String(),
                        Purpose = c.String(),
                        GatewayCode = c.String(),
                        CustomerOrder_Id = c.String(maxLength: 128),
                        Shipment_Id = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        Number = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(),
                        SourceStoreId = c.String(),
                        TargetStoreId = c.String(),
                        SourceAgentId = c.String(),
                        TargetAgentId = c.String(),
                        Comment = c.String(),
                        Currency = c.String(),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrder_Id)
                .ForeignKey("dbo.order_Shipment", t => t.Shipment_Id)
                .ForeignKey("dbo.order_Address", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CustomerOrder_Id)
                .Index(t => t.Shipment_Id);
            
            CreateTable(
                "dbo.order_Shipment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CustomerOrder_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        Number = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(),
                        SourceStoreId = c.String(),
                        TargetStoreId = c.String(),
                        SourceAgentId = c.String(),
                        TargetAgentId = c.String(),
                        Comment = c.String(),
                        Currency = c.String(),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrder_Id, cascadeDelete: true)
                .ForeignKey("dbo.order_Address", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CustomerOrder_Id);
            
            CreateTable(
                "dbo.order_Discount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(),
                        PromotionDescription = c.String(),
                        Currency = c.String(),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CouponCode = c.String(),
                        CouponInvalidDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_LineItem", t => t.Id)
                .ForeignKey("dbo.order_Shipment", t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.order_LineItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Long(nullable: false),
                        ProductId = c.String(),
                        CatalogId = c.String(),
                        CategoryId = c.String(),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        IsGift = c.Boolean(nullable: false),
                        ShippingMethodCode = c.String(),
                        FulfilmentLocationCode = c.String(),
                        CustomerOrder_Id = c.String(maxLength: 128),
                        Shipment_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.order_CustomerOrder", t => t.CustomerOrder_Id)
                .ForeignKey("dbo.order_Shipment", t => t.Shipment_Id)
                .Index(t => t.CustomerOrder_Id)
                .Index(t => t.Shipment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Shipment", "Id", "dbo.order_Address");
            DropForeignKey("dbo.order_PaymentIn", "Id", "dbo.order_Address");
            DropForeignKey("dbo.order_PaymentIn", "Shipment_Id", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_Shipment");
            DropForeignKey("dbo.order_LineItem", "Shipment_Id", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_LineItem");
            DropForeignKey("dbo.order_LineItem", "CustomerOrder_Id", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Shipment", "CustomerOrder_Id", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_PaymentIn", "CustomerOrder_Id", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Address", "CustomerOrder_Id", "dbo.order_CustomerOrder");
            DropIndex("dbo.order_LineItem", new[] { "Shipment_Id" });
            DropIndex("dbo.order_LineItem", new[] { "CustomerOrder_Id" });
            DropIndex("dbo.order_Discount", new[] { "Id" });
            DropIndex("dbo.order_Shipment", new[] { "CustomerOrder_Id" });
            DropIndex("dbo.order_Shipment", new[] { "Id" });
            DropIndex("dbo.order_PaymentIn", new[] { "Shipment_Id" });
            DropIndex("dbo.order_PaymentIn", new[] { "CustomerOrder_Id" });
            DropIndex("dbo.order_PaymentIn", new[] { "Id" });
            DropIndex("dbo.order_Address", new[] { "CustomerOrder_Id" });
            DropTable("dbo.order_LineItem");
            DropTable("dbo.order_Discount");
            DropTable("dbo.order_Shipment");
            DropTable("dbo.order_PaymentIn");
            DropTable("dbo.order_Address");
            DropTable("dbo.order_CustomerOrder");
        }
    }
}
