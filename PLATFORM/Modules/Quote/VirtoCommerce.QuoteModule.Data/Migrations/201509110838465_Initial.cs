namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuoteAddress",
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
                        QuoteRequestId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuoteRequest", t => t.QuoteRequestId, cascadeDelete: true)
                .Index(t => t.QuoteRequestId);
            
            CreateTable(
                "dbo.QuoteRequest",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Number = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 64),
                        StoreName = c.String(maxLength: 255),
                        ChannelId = c.String(maxLength: 64),
                        OrganizationId = c.String(maxLength: 64),
                        OrganizationName = c.String(maxLength: 255),
                        IsAnonymous = c.Boolean(nullable: false),
                        CustomerId = c.String(maxLength: 64),
                        CustomerName = c.String(maxLength: 255),
                        EmployeeId = c.String(maxLength: 64),
                        EmployeeName = c.String(maxLength: 255),
                        ExpirationDate = c.DateTime(),
                        ReminderDate = c.DateTime(),
                        EnableNotification = c.Boolean(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(),
                        InnerComment = c.String(),
                        Currency = c.String(nullable: false, maxLength: 3),
                        LanguageCode = c.String(maxLength: 5),
                        Coupon = c.String(maxLength: 64),
                        ShipmentMethodCode = c.String(maxLength: 64),
                        ShipmentMethodOption = c.String(maxLength: 64),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(),
                        CancelReason = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuoteAttachment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(nullable: false, maxLength: 2083),
                        Name = c.String(maxLength: 1024),
                        MimeType = c.String(maxLength: 128),
                        Size = c.Long(nullable: false),
                        QuoteRequestId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuoteRequest", t => t.QuoteRequestId)
                .Index(t => t.QuoteRequestId);
            
            CreateTable(
                "dbo.QuoteItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Currency = c.String(nullable: false, maxLength: 3),
                        BasePrice = c.Decimal(nullable: false, storeType: "money"),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        ProductId = c.String(nullable: false, maxLength: 64),
                        CatalogId = c.String(nullable: false, maxLength: 64),
                        CategoryId = c.String(maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        Comment = c.String(maxLength: 2048),
                        ImageUrl = c.String(maxLength: 1028),
                        QuoteRequestId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuoteRequest", t => t.QuoteRequestId, cascadeDelete: true)
                .Index(t => t.QuoteRequestId);
            
            CreateTable(
                "dbo.QuoteTierPrice",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Quantity = c.Long(nullable: false),
                        QuoteItemId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuoteItem", t => t.QuoteItemId, cascadeDelete: true)
                .Index(t => t.QuoteItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuoteAddress", "QuoteRequestId", "dbo.QuoteRequest");
            DropForeignKey("dbo.QuoteItem", "QuoteRequestId", "dbo.QuoteRequest");
            DropForeignKey("dbo.QuoteTierPrice", "QuoteItemId", "dbo.QuoteItem");
            DropForeignKey("dbo.QuoteAttachment", "QuoteRequestId", "dbo.QuoteRequest");
            DropIndex("dbo.QuoteTierPrice", new[] { "QuoteItemId" });
            DropIndex("dbo.QuoteItem", new[] { "QuoteRequestId" });
            DropIndex("dbo.QuoteAttachment", new[] { "QuoteRequestId" });
            DropIndex("dbo.QuoteAddress", new[] { "QuoteRequestId" });
            DropTable("dbo.QuoteTierPrice");
            DropTable("dbo.QuoteItem");
            DropTable("dbo.QuoteAttachment");
            DropTable("dbo.QuoteRequest");
            DropTable("dbo.QuoteAddress");
        }
    }
}
