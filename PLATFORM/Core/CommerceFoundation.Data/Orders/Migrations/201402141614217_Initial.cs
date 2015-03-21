using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderAddress",
                c => new
                {
                    OrderAddressId = c.String(nullable: false, maxLength: 128),
                    OrderGroupId = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 512),
                    FirstName = c.String(nullable: false, maxLength: 128),
                    LastName = c.String(nullable: false, maxLength: 128),
                    Organization = c.String(maxLength: 128),
                    Line1 = c.String(nullable: false, maxLength: 128),
                    Line2 = c.String(maxLength: 128),
                    City = c.String(nullable: false, maxLength: 128),
                    StateProvince = c.String(maxLength: 128),
                    CountryCode = c.String(nullable: false, maxLength: 128),
                    CountryName = c.String(maxLength: 64),
                    PostalCode = c.String(nullable: false, maxLength: 32),
                    RegionId = c.String(maxLength: 32),
                    RegionName = c.String(maxLength: 64),
                    DaytimePhoneNumber = c.String(nullable: false, maxLength: 32),
                    EveningPhoneNumber = c.String(maxLength: 32),
                    FaxNumber = c.String(maxLength: 32),
                    Email = c.String(nullable: false, maxLength: 256),
                    LastModified = c.DateTime(),
                    Created = c.DateTime(),
                    Discriminator = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.OrderAddressId)
                .ForeignKey("dbo.OrderGroup", t => t.OrderGroupId, cascadeDelete: true)
                .Index(t => t.OrderGroupId);
            
            CreateTable(
                "dbo.OrderForm",
                c => new
                    {
                        OrderFormId = c.String(nullable: false, maxLength: 128),
                        OrderGroupId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        BillingAddressId = c.String(maxLength: 128),
                        Status = c.String(maxLength: 64),
                        ShippingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HandlingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderFormId)
                .ForeignKey("dbo.OrderGroup", t => t.OrderGroupId, cascadeDelete: true)
                .Index(t => t.OrderGroupId);
            
            CreateTable(
                "dbo.OrderFormDiscount",
                c => new
                    {
                        DiscountId = c.String(nullable: false, maxLength: 128),
                        OrderFormId = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(maxLength: 128),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountCode = c.String(maxLength: 128),
                        DiscountName = c.String(maxLength: 128),
                        DisplayMessage = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("dbo.OrderForm", t => t.OrderFormId, cascadeDelete: true)
                .Index(t => t.OrderFormId);
            
            CreateTable(
                "dbo.LineItem",
                c => new
                    {
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        OrderFormId = c.String(nullable: false, maxLength: 128),
                        Catalog = c.String(maxLength: 128),
                        CatalogCategory = c.String(maxLength: 128),
                        ShippingMethodId = c.String(maxLength: 128),
                        CatalogOutline = c.String(),
                        CatalogItemId = c.String(nullable: false, maxLength: 128),
                        ParentCatalogItemId = c.String(maxLength: 128),
                        CatalogItemCode = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlacedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ListPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineItemDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InStockQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreorderQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BackorderQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingAddressId = c.String(maxLength: 128),
                        ShippingMethodName = c.String(maxLength: 128),
                        ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 512),
                        Comment = c.String(maxLength: 512),
                        DisplayName = c.String(maxLength: 1024),
                        AllowBackorders = c.Boolean(nullable: false),
                        AllowPreorders = c.Boolean(nullable: false),
                        InventoryStatus = c.String(maxLength: 128),
                        Status = c.String(maxLength: 128),
                        FulfillmentCenterId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LineItemId)
                .ForeignKey("dbo.OrderForm", t => t.OrderFormId, cascadeDelete: true)
                .Index(t => t.OrderFormId);
            
            CreateTable(
                "dbo.LineItemDiscount",
                c => new
                    {
                        DiscountId = c.String(nullable: false, maxLength: 128),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(maxLength: 128),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountCode = c.String(maxLength: 128),
                        DiscountName = c.String(maxLength: 128),
                        DisplayMessage = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("dbo.LineItem", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "dbo.LineItemOption",
                c => new
                    {
                        LineItemOptionId = c.String(nullable: false, maxLength: 128),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        OptionName = c.String(nullable: false, maxLength: 64),
                        OptionValue = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LineItemOptionId)
                .ForeignKey("dbo.LineItem", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "dbo.OrderFormPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
                        OrderFormId = c.String(maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.OrderForm", t => t.OrderFormId)
                .Index(t => t.OrderFormId);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        PaymentId = c.String(nullable: false, maxLength: 128),
                        OrderFormId = c.String(nullable: false, maxLength: 128),
                        BillingAddressId = c.String(maxLength: 128),
                        ContractId = c.String(maxLength: 128),
                        PaymentMethodId = c.String(maxLength: 128),
                        PaymentMethodName = c.String(maxLength: 128),
                        ValidationCode = c.String(maxLength: 128),
                        AuthorizationCode = c.String(maxLength: 128),
                        StatusCode = c.String(maxLength: 128),
                        StatusDesc = c.String(maxLength: 128),
                        PaymentTypeId = c.Int(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(maxLength: 64),
                        TransactionType = c.String(maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        CashCardNumber = c.String(maxLength: 64),
                        CashCardSecurityCode = c.String(maxLength: 32),
                        CreditCardCustomerName = c.String(maxLength: 128),
                        CreditCardType = c.String(maxLength: 32),
                        CreditCardNumber = c.String(maxLength: 64),
                        CreditCardSecurityCode = c.String(maxLength: 32),
                        CreditCardExpirationMonth = c.Int(),
                        CreditCardExpirationYear = c.Int(),
                        CardType = c.String(maxLength: 32),
                        GiftCardNumber = c.String(maxLength: 64),
                        GiftCardSecurityCode = c.String(maxLength: 32),
                        ExpirationMonth = c.Int(),
                        ExpirationYear = c.Int(),
                        InvoiceNumber = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.OrderForm", t => t.OrderFormId, cascadeDelete: true)
                .Index(t => t.OrderFormId);
            
            CreateTable(
                "dbo.Shipment",
                c => new
                    {
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        ShippingMethodId = c.String(maxLength: 128),
                        ShippingMethodName = c.String(maxLength: 128),
                        ShippingAddressId = c.String(maxLength: 128),
                        FulfillmentCenterId = c.String(maxLength: 128),
                        ShipmentTrackingNumber = c.String(maxLength: 128),
                        ItemSubtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalBeforeTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShipmentTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(maxLength: 64),
                        OrderFormId = c.String(nullable: false, maxLength: 128),
                        PicklistId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShipmentId)
                .ForeignKey("dbo.OrderForm", t => t.OrderFormId, cascadeDelete: true)
                .ForeignKey("dbo.Picklist", t => t.PicklistId)
                .Index(t => t.OrderFormId)
                .Index(t => t.PicklistId);
            
            CreateTable(
                "dbo.ShipmentDiscount",
                c => new
                    {
                        DiscountId = c.String(nullable: false, maxLength: 128),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(maxLength: 128),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountCode = c.String(maxLength: 128),
                        DiscountName = c.String(maxLength: 128),
                        DisplayMessage = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("dbo.Shipment", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.Picklist",
                c => new
                    {
                        PicklistId = c.String(nullable: false, maxLength: 128),
                        FulfillmentCenterId = c.String(nullable: false, maxLength: 128),
                        MemberId = c.String(maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PicklistId);
            
            CreateTable(
                "dbo.ShipmentItem",
                c => new
                    {
                        ShipmentItemId = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShipmentItemId)
                .ForeignKey("dbo.LineItem", t => t.LineItemId)
                .ForeignKey("dbo.Shipment", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.LineItemId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.OrderGroup",
                c => new
                    {
                        OrderGroupId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.String(maxLength: 128),
                        CustomerId = c.String(maxLength: 128),
                        CustomerName = c.String(maxLength: 128),
                        StoreId = c.String(maxLength: 128),
                        AddressId = c.String(maxLength: 128),
                        ShippingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HandlingTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingCurrency = c.String(maxLength: 32),
                        Status = c.String(maxLength: 32),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        TrackingNumber = c.String(maxLength: 128),
                        ParentOrderId = c.String(),
                        ExpirationDate = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderGroupId)
                .Index(t => t.Discriminator)
                .Index(t => t.Created)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.RmaRequest",
                c => new
                    {
                        RmaRequestId = c.String(nullable: false, maxLength: 128),
                        AuthorizationCode = c.String(maxLength: 128),
                        Notes = c.String(),
                        AgentId = c.String(maxLength: 128),
                        ReturnAddressId = c.String(maxLength: 128),
                        ReturnFromAddressId = c.String(maxLength: 128),
                        Comment = c.String(maxLength: 528),
                        Status = c.String(maxLength: 64),
                        ItemSubtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalBeforeTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LessReStockingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReturnTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefundAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPhysicalReturnRequired = c.Boolean(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 128),
                        ExchangeOrderId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RmaRequestId)
                .ForeignKey("dbo.OrderGroup", t => t.ExchangeOrderId)
                .ForeignKey("dbo.OrderGroup", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.ExchangeOrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.RmaReturnItem",
                c => new
                    {
                        RmaReturnItemId = c.String(nullable: false, maxLength: 128),
                        ReturnReason = c.String(nullable: false, maxLength: 128),
                        ItemState = c.String(nullable: false, maxLength: 128),
                        ReturnCondition = c.String(maxLength: 128),
                        ReturnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RmaRequestId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RmaReturnItemId)
                .ForeignKey("dbo.RmaRequest", t => t.RmaRequestId)
                .Index(t => t.RmaRequestId);
            
            CreateTable(
                "dbo.RmaLineItem",
                c => new
                    {
                        RmaLineItemId = c.String(nullable: false, maxLength: 128),
                        ReturnQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        RmaReturnItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RmaLineItemId)
                .ForeignKey("dbo.LineItem", t => t.LineItemId)
                .ForeignKey("dbo.RmaReturnItem", t => t.RmaReturnItemId, cascadeDelete: true)
                .Index(t => t.LineItemId)
                .Index(t => t.RmaReturnItemId);
            
            CreateTable(
                "dbo.GatewayProperty",
                c => new
                    {
                        GatewayPropertyId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 64),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        IsRequired = c.Boolean(nullable: false),
                        ValueType = c.Int(nullable: false),
                        GatewayId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GatewayPropertyId)
                .ForeignKey("dbo.Gateway", t => t.GatewayId, cascadeDelete: true)
                .Index(t => t.GatewayId);
            
            CreateTable(
                "dbo.GatewayPropertyDictionaryValue",
                c => new
                    {
                        GatewayPropertyDictionaryValueId = c.String(nullable: false, maxLength: 64),
                        Value = c.String(nullable: false, maxLength: 64),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        GatewayPropertyId = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GatewayPropertyDictionaryValueId)
                .ForeignKey("dbo.GatewayProperty", t => t.GatewayPropertyId, cascadeDelete: true)
                .Index(t => t.GatewayPropertyId);
            
            CreateTable(
                "dbo.Gateway",
                c => new
                    {
                        GatewayId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        ClassType = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        SupportsRecurring = c.Boolean(),
                        SupportedTransactionTypes = c.Int(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GatewayId);
            
            CreateTable(
                "dbo.ShippingOption",
                c => new
                    {
                        ShippingOptionId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 64),
                        Description = c.String(maxLength: 128),
                        ShippingGatewayId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShippingOptionId)
                .ForeignKey("dbo.Gateway", t => t.ShippingGatewayId)
                .Index(t => t.ShippingGatewayId);
            
            CreateTable(
                "dbo.ShippingGatewayPropertyValue",
                c => new
                    {
                        ShippingGatewayPropertyValueId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        BooleanValue = c.Boolean(nullable: false),
                        ShippingOptionId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.ShippingGatewayPropertyValueId)
                .ForeignKey("dbo.ShippingOption", t => t.ShippingOptionId, cascadeDelete: true)
                .Index(t => t.ShippingOptionId);
            
            CreateTable(
                "dbo.ShippingMethod",
                c => new
                    {
                        ShippingMethodId = c.String(nullable: false, maxLength: 128),
                        ShippingOptionId = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(maxLength: 128),
                        DisplayName = c.String(maxLength: 128),
                        RestrictPaymentMethods = c.String(maxLength: 256),
                        RestrictJurisdictionGroups = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShippingMethodId)
                .ForeignKey("dbo.ShippingOption", t => t.ShippingOptionId)
                .Index(t => t.ShippingOptionId);
            
            CreateTable(
                "dbo.PaymentMethodShippingMethod",
                c => new
                    {
                        PaymentMethodShippingMethodId = c.String(nullable: false, maxLength: 64),
                        PaymentMethodId = c.String(nullable: false, maxLength: 128),
                        ShippingMethodId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentMethodShippingMethodId)
                .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethodId, cascadeDelete: true)
                .ForeignKey("dbo.ShippingMethod", t => t.ShippingMethodId, cascadeDelete: true)
                .Index(t => t.PaymentMethodId)
                .Index(t => t.ShippingMethodId);
            
            CreateTable(
                "dbo.PaymentMethod",
                c => new
                    {
                        PaymentMethodId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        RestrictShippingMethods = c.Boolean(nullable: false),
                        PaymentGatewayId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentMethodId)
                .ForeignKey("dbo.Gateway", t => t.PaymentGatewayId)
                .Index(t => t.PaymentGatewayId);
            
            CreateTable(
                "dbo.PaymentMethodLanguage",
                c => new
                    {
                        PaymentMethodLanguageId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        PaymentMethodId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentMethodLanguageId)
                .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethodId, cascadeDelete: true)
                .Index(t => t.PaymentMethodId);
            
            CreateTable(
                "dbo.PaymentMethodPropertyValue",
                c => new
                    {
                        PaymentMethodPropertyValueId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        BooleanValue = c.Boolean(nullable: false),
                        PaymentMethodId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentMethodPropertyValueId)
                .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethodId, cascadeDelete: true)
                .Index(t => t.PaymentMethodId);
            
            CreateTable(
                "dbo.ShippingMethodCase",
                c => new
                    {
                        ShippingMethodCaseId = c.String(nullable: false, maxLength: 128),
                        ShippingMethodId = c.String(nullable: false, maxLength: 128),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Charge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JurisdictionGroup = c.String(maxLength: 128),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShippingMethodCaseId)
                .ForeignKey("dbo.ShippingMethod", t => t.ShippingMethodId, cascadeDelete: true)
                .Index(t => t.ShippingMethodId);
            
            CreateTable(
                "dbo.ShippingMethodJurisdictionGroup",
                c => new
                    {
                        ShippingMethodJurisdictionGroupId = c.String(nullable: false, maxLength: 64),
                        ShippingMethodId = c.String(nullable: false, maxLength: 128),
                        JurisdictionGroupId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.ShippingMethodJurisdictionGroupId)
                .ForeignKey("dbo.JurisdictionGroup", t => t.JurisdictionGroupId, cascadeDelete: true)
                .ForeignKey("dbo.ShippingMethod", t => t.ShippingMethodId, cascadeDelete: true)
                .Index(t => t.JurisdictionGroupId)
                .Index(t => t.ShippingMethodId);
            
            CreateTable(
                "dbo.JurisdictionGroup",
                c => new
                    {
                        JurisdictionGroupId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        JurisdictionType = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JurisdictionGroupId);
            
            CreateTable(
                "dbo.JurisdictionRelation",
                c => new
                    {
                        JurisdictionRelationId = c.String(nullable: false, maxLength: 64),
                        JurisdictionId = c.String(nullable: false, maxLength: 128),
                        JurisdictionGroupId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JurisdictionRelationId)
                .ForeignKey("dbo.Jurisdiction", t => t.JurisdictionId, cascadeDelete: true)
                .ForeignKey("dbo.JurisdictionGroup", t => t.JurisdictionGroupId, cascadeDelete: true)
                .Index(t => t.JurisdictionId)
                .Index(t => t.JurisdictionGroupId);
            
            CreateTable(
                "dbo.Jurisdiction",
                c => new
                    {
                        JurisdictionId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(maxLength: 128),
                        StateProvinceCode = c.String(maxLength: 32),
                        CountryCode = c.String(nullable: false, maxLength: 64),
                        JurisdictionType = c.Int(nullable: false),
                        ZipPostalCodeStart = c.String(maxLength: 64),
                        ZipPostalCodeEnd = c.String(maxLength: 64),
                        City = c.String(maxLength: 64),
                        District = c.String(maxLength: 64),
                        County = c.String(maxLength: 64),
                        GeoCode = c.String(maxLength: 256),
                        Code = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JurisdictionId);
            
            CreateTable(
                "dbo.ShippingMethodLanguage",
                c => new
                    {
                        ShippingMethodLanguageId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        ShippingMethodId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShippingMethodLanguageId)
                .ForeignKey("dbo.ShippingMethod", t => t.ShippingMethodId, cascadeDelete: true)
                .Index(t => t.ShippingMethodId);
            
            CreateTable(
                "dbo.ShippingPackage",
                c => new
                    {
                        ShippingPackageId = c.String(nullable: false, maxLength: 128),
                        MappedPackagingId = c.String(nullable: false, maxLength: 128),
                        ShippingOptionPackaging = c.String(nullable: false, maxLength: 128),
                        ShippingOptionId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShippingPackageId)
                .ForeignKey("dbo.ShippingOption", t => t.ShippingOptionId, cascadeDelete: true)
                .Index(t => t.ShippingOptionId);
            
            CreateTable(
                "dbo.Tax",
                c => new
                    {
                        TaxId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        TaxType = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TaxId);
            
            CreateTable(
                "dbo.TaxLanguage",
                c => new
                    {
                        TaxLanguageId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        TaxId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TaxLanguageId)
                .ForeignKey("dbo.Tax", t => t.TaxId, cascadeDelete: true)
                .Index(t => t.TaxId);
            
            CreateTable(
                "dbo.TaxValue",
                c => new
                    {
                        TaxValueId = c.String(nullable: false, maxLength: 128),
                        TaxId = c.String(nullable: false, maxLength: 128),
                        JurisdictionGroupId = c.String(nullable: false, maxLength: 128),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxCategory = c.String(maxLength: 128),
                        AffectiveDate = c.DateTime(),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TaxValueId)
                .ForeignKey("dbo.JurisdictionGroup", t => t.JurisdictionGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Tax", t => t.TaxId, cascadeDelete: true)
                .Index(t => t.JurisdictionGroupId)
                .Index(t => t.TaxId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.String(nullable: false, maxLength: 128),
                        IsVisible = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 256),
                        DisplayName = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        RegionId = c.String(nullable: false, maxLength: 128),
                        CountryId = c.String(nullable: false, maxLength: 128),
                        IsVisible = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 256),
                        DisplayName = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RegionId)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Region", "CountryId", "dbo.Country");
            DropForeignKey("dbo.TaxValue", "TaxId", "dbo.Tax");
            DropForeignKey("dbo.TaxValue", "JurisdictionGroupId", "dbo.JurisdictionGroup");
            DropForeignKey("dbo.TaxLanguage", "TaxId", "dbo.Tax");
            DropForeignKey("dbo.ShippingPackage", "ShippingOptionId", "dbo.ShippingOption");
            DropForeignKey("dbo.ShippingMethod", "ShippingOptionId", "dbo.ShippingOption");
            DropForeignKey("dbo.ShippingMethodLanguage", "ShippingMethodId", "dbo.ShippingMethod");
            DropForeignKey("dbo.ShippingMethodJurisdictionGroup", "ShippingMethodId", "dbo.ShippingMethod");
            DropForeignKey("dbo.ShippingMethodJurisdictionGroup", "JurisdictionGroupId", "dbo.JurisdictionGroup");
            DropForeignKey("dbo.JurisdictionRelation", "JurisdictionGroupId", "dbo.JurisdictionGroup");
            DropForeignKey("dbo.JurisdictionRelation", "JurisdictionId", "dbo.Jurisdiction");
            DropForeignKey("dbo.ShippingMethodCase", "ShippingMethodId", "dbo.ShippingMethod");
            DropForeignKey("dbo.PaymentMethodShippingMethod", "ShippingMethodId", "dbo.ShippingMethod");
            DropForeignKey("dbo.PaymentMethodShippingMethod", "PaymentMethodId", "dbo.PaymentMethod");
            DropForeignKey("dbo.PaymentMethodPropertyValue", "PaymentMethodId", "dbo.PaymentMethod");
            DropForeignKey("dbo.PaymentMethodLanguage", "PaymentMethodId", "dbo.PaymentMethod");
            DropForeignKey("dbo.PaymentMethod", "PaymentGatewayId", "dbo.Gateway");
            DropForeignKey("dbo.ShippingGatewayPropertyValue", "ShippingOptionId", "dbo.ShippingOption");
            DropForeignKey("dbo.ShippingOption", "ShippingGatewayId", "dbo.Gateway");
            DropForeignKey("dbo.GatewayPropertyDictionaryValue", "GatewayPropertyId", "dbo.GatewayProperty");
            DropForeignKey("dbo.GatewayProperty", "GatewayId", "dbo.Gateway");
            DropForeignKey("dbo.RmaReturnItem", "RmaRequestId", "dbo.RmaRequest");
            DropForeignKey("dbo.RmaLineItem", "RmaReturnItemId", "dbo.RmaReturnItem");
            DropForeignKey("dbo.RmaLineItem", "LineItemId", "dbo.LineItem");
            DropForeignKey("dbo.RmaRequest", "OrderId", "dbo.OrderGroup");
            DropForeignKey("dbo.RmaRequest", "ExchangeOrderId", "dbo.OrderGroup");
            DropForeignKey("dbo.ShipmentItem", "ShipmentId", "dbo.Shipment");
            DropForeignKey("dbo.ShipmentItem", "LineItemId", "dbo.LineItem");
            DropForeignKey("dbo.Shipment", "PicklistId", "dbo.Picklist");
            DropForeignKey("dbo.Shipment", "OrderFormId", "dbo.OrderForm");
            DropForeignKey("dbo.ShipmentDiscount", "ShipmentId", "dbo.Shipment");
            DropForeignKey("dbo.Payment", "OrderFormId", "dbo.OrderForm");
            DropForeignKey("dbo.OrderForm", "OrderGroupId", "dbo.OrderGroup");
            DropForeignKey("dbo.OrderFormPropertyValue", "OrderFormId", "dbo.OrderForm");
            DropForeignKey("dbo.LineItem", "OrderFormId", "dbo.OrderForm");
            DropForeignKey("dbo.LineItemOption", "LineItemId", "dbo.LineItem");
            DropForeignKey("dbo.LineItemDiscount", "LineItemId", "dbo.LineItem");
            DropForeignKey("dbo.OrderFormDiscount", "OrderFormId", "dbo.OrderForm");
            DropForeignKey("dbo.OrderAddress", "OrderGroupId", "dbo.OrderGroup");
            DropIndex("dbo.Region", new[] { "CountryId" });
            DropIndex("dbo.TaxValue", new[] { "TaxId" });
            DropIndex("dbo.TaxValue", new[] { "JurisdictionGroupId" });
            DropIndex("dbo.TaxLanguage", new[] { "TaxId" });
            DropIndex("dbo.ShippingPackage", new[] { "ShippingOptionId" });
            DropIndex("dbo.ShippingMethod", new[] { "ShippingOptionId" });
            DropIndex("dbo.ShippingMethodLanguage", new[] { "ShippingMethodId" });
            DropIndex("dbo.ShippingMethodJurisdictionGroup", new[] { "ShippingMethodId" });
            DropIndex("dbo.ShippingMethodJurisdictionGroup", new[] { "JurisdictionGroupId" });
            DropIndex("dbo.JurisdictionRelation", new[] { "JurisdictionGroupId" });
            DropIndex("dbo.JurisdictionRelation", new[] { "JurisdictionId" });
            DropIndex("dbo.ShippingMethodCase", new[] { "ShippingMethodId" });
            DropIndex("dbo.PaymentMethodShippingMethod", new[] { "ShippingMethodId" });
            DropIndex("dbo.PaymentMethodShippingMethod", new[] { "PaymentMethodId" });
            DropIndex("dbo.PaymentMethodPropertyValue", new[] { "PaymentMethodId" });
            DropIndex("dbo.PaymentMethodLanguage", new[] { "PaymentMethodId" });
            DropIndex("dbo.PaymentMethod", new[] { "PaymentGatewayId" });
            DropIndex("dbo.ShippingGatewayPropertyValue", new[] { "ShippingOptionId" });
            DropIndex("dbo.ShippingOption", new[] { "ShippingGatewayId" });
            DropIndex("dbo.GatewayPropertyDictionaryValue", new[] { "GatewayPropertyId" });
            DropIndex("dbo.GatewayProperty", new[] { "GatewayId" });
            DropIndex("dbo.RmaReturnItem", new[] { "RmaRequestId" });
            DropIndex("dbo.RmaLineItem", new[] { "RmaReturnItemId" });
            DropIndex("dbo.RmaLineItem", new[] { "LineItemId" });
            DropIndex("dbo.RmaRequest", new[] { "OrderId" });
            DropIndex("dbo.RmaRequest", new[] { "ExchangeOrderId" });
            DropIndex("dbo.ShipmentItem", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentItem", new[] { "LineItemId" });
            DropIndex("dbo.Shipment", new[] { "PicklistId" });
            DropIndex("dbo.Shipment", new[] { "OrderFormId" });
            DropIndex("dbo.ShipmentDiscount", new[] { "ShipmentId" });
            DropIndex("dbo.Payment", new[] { "OrderFormId" });
            DropIndex("dbo.OrderForm", new[] { "OrderGroupId" });
            DropIndex("dbo.OrderFormPropertyValue", new[] { "OrderFormId" });
            DropIndex("dbo.LineItem", new[] { "OrderFormId" });
            DropIndex("dbo.LineItemOption", new[] { "LineItemId" });
            DropIndex("dbo.LineItemDiscount", new[] { "LineItemId" });
            DropIndex("dbo.OrderFormDiscount", new[] { "OrderFormId" });
            DropIndex("dbo.OrderAddress", new[] { "OrderGroupId" });
            DropIndex("dbo.OrderGroup", new[] { "Discriminator" });
            DropIndex("dbo.OrderGroup", new[] { "Created" });
            DropIndex("dbo.OrderGroup", new[] { "StoreId" });
            DropTable("dbo.Region");
            DropTable("dbo.Country");
            DropTable("dbo.TaxValue");
            DropTable("dbo.TaxLanguage");
            DropTable("dbo.Tax");
            DropTable("dbo.ShippingPackage");
            DropTable("dbo.ShippingMethodLanguage");
            DropTable("dbo.Jurisdiction");
            DropTable("dbo.JurisdictionRelation");
            DropTable("dbo.JurisdictionGroup");
            DropTable("dbo.ShippingMethodJurisdictionGroup");
            DropTable("dbo.ShippingMethodCase");
            DropTable("dbo.PaymentMethodPropertyValue");
            DropTable("dbo.PaymentMethodLanguage");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.PaymentMethodShippingMethod");
            DropTable("dbo.ShippingMethod");
            DropTable("dbo.ShippingGatewayPropertyValue");
            DropTable("dbo.ShippingOption");
            DropTable("dbo.Gateway");
            DropTable("dbo.GatewayPropertyDictionaryValue");
            DropTable("dbo.GatewayProperty");
            DropTable("dbo.RmaLineItem");
            DropTable("dbo.RmaReturnItem");
            DropTable("dbo.RmaRequest");
            DropTable("dbo.OrderGroup");
            DropTable("dbo.ShipmentItem");
            DropTable("dbo.Picklist");
            DropTable("dbo.ShipmentDiscount");
            DropTable("dbo.Shipment");
            DropTable("dbo.Payment");
            DropTable("dbo.OrderFormPropertyValue");
            DropTable("dbo.LineItemOption");
            DropTable("dbo.LineItemDiscount");
            DropTable("dbo.LineItem");
            DropTable("dbo.OrderFormDiscount");
            DropTable("dbo.OrderForm");
            DropTable("dbo.OrderAddress");
        }
    }
}
