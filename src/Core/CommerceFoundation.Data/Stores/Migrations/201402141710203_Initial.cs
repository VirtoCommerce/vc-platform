namespace VirtoCommerce.Foundation.Data.Stores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FulfillmentCenter",
                c => new
                    {
                        FulfillmentCenterId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        MaxReleasesPerPickBatch = c.Int(nullable: false),
                        PickDelay = c.Int(nullable: false),
                        DaytimePhoneNumber = c.String(nullable: false, maxLength: 32),
                        Line1 = c.String(nullable: false, maxLength: 128),
                        Line2 = c.String(maxLength: 128),
                        City = c.String(nullable: false, maxLength: 128),
                        StateProvince = c.String(maxLength: 128),
                        CountryCode = c.String(nullable: false, maxLength: 64),
                        CountryName = c.String(nullable: false, maxLength: 128),
                        PostalCode = c.String(nullable: false, maxLength: 32),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FulfillmentCenterId);
            
            CreateTable(
                "dbo.Store",
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreId)
                .ForeignKey("dbo.FulfillmentCenter", t => t.FulfillmentCenterId)
                .ForeignKey("dbo.FulfillmentCenter", t => t.ReturnsFulfillmentCenterId)
                .Index(t => t.FulfillmentCenterId)
                .Index(t => t.ReturnsFulfillmentCenterId);
            
            CreateTable(
                "dbo.StoreCardType",
                c => new
                    {
                        StoreCardTypeId = c.String(nullable: false, maxLength: 128),
                        CardType = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreCardTypeId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreCurrency",
                c => new
                    {
                        StoreCurrencyId = c.String(nullable: false, maxLength: 128),
                        CurrencyCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreCurrencyId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreLanguage",
                c => new
                    {
                        StoreLanguageId = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreLanguageId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreLinkedStore",
                c => new
                    {
                        StoreLinkedStoreId = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LinkedStoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreLinkedStoreId)
                .ForeignKey("dbo.Store", t => t.LinkedStoreId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.LinkedStoreId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StorePaymentGateway",
                c => new
                    {
                        StorePaymentGatewayId = c.String(nullable: false, maxLength: 128),
                        PaymentGateway = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StorePaymentGatewayId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreSetting",
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreSettingId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreTaxCode",
                c => new
                    {
                        StoreTaxCodeId = c.String(nullable: false, maxLength: 128),
                        TaxCode = c.String(nullable: false, maxLength: 32),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreTaxCodeId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreTaxJurisdiction",
                c => new
                    {
                        StoreTaxJurisdictionId = c.String(nullable: false, maxLength: 128),
                        TaxJurisdiction = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreTaxJurisdictionId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreTaxJurisdiction", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreTaxCode", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreSetting", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store", "ReturnsFulfillmentCenterId", "dbo.FulfillmentCenter");
            DropForeignKey("dbo.StorePaymentGateway", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLinkedStore", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLinkedStore", "LinkedStoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLanguage", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store", "FulfillmentCenterId", "dbo.FulfillmentCenter");
            DropForeignKey("dbo.StoreCurrency", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreCardType", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreTaxJurisdiction", new[] { "StoreId" });
            DropIndex("dbo.StoreTaxCode", new[] { "StoreId" });
            DropIndex("dbo.StoreSetting", new[] { "StoreId" });
            DropIndex("dbo.Store", new[] { "ReturnsFulfillmentCenterId" });
            DropIndex("dbo.StorePaymentGateway", new[] { "StoreId" });
            DropIndex("dbo.StoreLinkedStore", new[] { "StoreId" });
            DropIndex("dbo.StoreLinkedStore", new[] { "LinkedStoreId" });
            DropIndex("dbo.StoreLanguage", new[] { "StoreId" });
            DropIndex("dbo.Store", new[] { "FulfillmentCenterId" });
            DropIndex("dbo.StoreCurrency", new[] { "StoreId" });
            DropIndex("dbo.StoreCardType", new[] { "StoreId" });
            DropTable("dbo.StoreTaxJurisdiction");
            DropTable("dbo.StoreTaxCode");
            DropTable("dbo.StoreSetting");
            DropTable("dbo.StorePaymentGateway");
            DropTable("dbo.StoreLinkedStore");
            DropTable("dbo.StoreLanguage");
            DropTable("dbo.StoreCurrency");
            DropTable("dbo.StoreCardType");
            DropTable("dbo.Store");
            DropTable("dbo.FulfillmentCenter");
        }
    }
}
