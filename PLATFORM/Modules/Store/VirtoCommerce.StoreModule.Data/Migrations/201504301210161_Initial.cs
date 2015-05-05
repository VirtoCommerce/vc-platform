namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        Url = c.String(maxLength: 256),
                        StoreState = c.Int(nullable: false),
                        TimeZone = c.String(maxLength: 128),
                        Country = c.String(maxLength: 128),
                        Region = c.String(maxLength: 128),
                        DefaultLanguage = c.String(maxLength: 128),
                        DefaultCurrency = c.String(maxLength: 64),
                        Catalog = c.String(nullable: false, maxLength: 128),
                        CreditCardSavePolicy = c.Int(nullable: false),
                        SecureUrl = c.String(maxLength: 128),
                        Email = c.String(maxLength: 128),
                        AdminEmail = c.String(maxLength: 128),
                        DisplayOutOfStock = c.Boolean(nullable: false),
                        FulfillmentCenterId = c.String(maxLength: 128),
                        ReturnsFulfillmentCenterId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreCurrency",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CurrencyCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreLanguage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StorePaymentGateway",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PaymentGateway = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreSetting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreSetting", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StorePaymentGateway", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLanguage", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreCurrency", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreSetting", new[] { "StoreId" });
            DropIndex("dbo.StorePaymentGateway", new[] { "StoreId" });
            DropIndex("dbo.StoreLanguage", new[] { "StoreId" });
            DropIndex("dbo.StoreCurrency", new[] { "StoreId" });
            DropTable("dbo.StoreSetting");
            DropTable("dbo.StorePaymentGateway");
            DropTable("dbo.StoreLanguage");
            DropTable("dbo.StoreCurrency");
            DropTable("dbo.Store");
        }
    }
}
