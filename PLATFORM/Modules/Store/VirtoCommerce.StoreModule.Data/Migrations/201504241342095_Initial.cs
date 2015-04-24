namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.vc_Store",
                c => new
                    {
                        StoreId = c.String(nullable: false, maxLength: 128),
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
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreId)
                .ForeignKey("dbo.vc_FulfillmentCenter", t => t.FulfillmentCenterId)
                .ForeignKey("dbo.vc_FulfillmentCenter", t => t.ReturnsFulfillmentCenterId)
                .Index(t => t.FulfillmentCenterId)
                .Index(t => t.ReturnsFulfillmentCenterId);
            
            CreateTable(
                "dbo.vc_StoreCurrency",
                c => new
                    {
                        StoreCurrencyId = c.String(nullable: false, maxLength: 128),
                        CurrencyCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreCurrencyId)
                .ForeignKey("dbo.vc_Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.vc_StoreLanguage",
                c => new
                    {
                        StoreLanguageId = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreLanguageId)
                .ForeignKey("dbo.vc_Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.vc_StorePaymentGateway",
                c => new
                    {
                        StorePaymentGatewayId = c.String(nullable: false, maxLength: 128),
                        PaymentGateway = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StorePaymentGatewayId)
                .ForeignKey("dbo.vc_Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.vc_StoreSetting",
                c => new
                    {
                        StoreSettingId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.StoreSettingId)
                .ForeignKey("dbo.vc_Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.vc_StoreSetting", "StoreId", "dbo.vc_Store");
            DropForeignKey("dbo.vc_Store", "ReturnsFulfillmentCenterId", "dbo.vc_FulfillmentCenter");
            DropForeignKey("dbo.vc_StorePaymentGateway", "StoreId", "dbo.vc_Store");
            DropForeignKey("dbo.vc_StoreLanguage", "StoreId", "dbo.vc_Store");
            DropForeignKey("dbo.vc_Store", "FulfillmentCenterId", "dbo.vc_FulfillmentCenter");
            DropForeignKey("dbo.vc_StoreCurrency", "StoreId", "dbo.vc_Store");
            DropIndex("dbo.vc_StoreSetting", new[] { "StoreId" });
            DropIndex("dbo.vc_StorePaymentGateway", new[] { "StoreId" });
            DropIndex("dbo.vc_StoreLanguage", new[] { "StoreId" });
            DropIndex("dbo.vc_StoreCurrency", new[] { "StoreId" });
            DropIndex("dbo.vc_Store", new[] { "ReturnsFulfillmentCenterId" });
            DropIndex("dbo.vc_Store", new[] { "FulfillmentCenterId" });
            DropTable("dbo.vc_StoreSetting");
            DropTable("dbo.vc_StorePaymentGateway");
            DropTable("dbo.vc_StoreLanguage");
            DropTable("dbo.vc_StoreCurrency");
            DropTable("dbo.vc_Store");
            DropTable("dbo.vc_FulfillmentCenter");
        }
    }
}
