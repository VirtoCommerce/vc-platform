using System;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    // Each test uses a unique base type to avoid static state pollution across tests.
    // AbstractTypeFactory<T> is a static generic class — each T gets its own state.

    // ── Base domain types (one per test scenario) ──────────────────

    // For: TryCreateInstance_NoOverrides_CreatesBaseType
    public class Product
    {
        public string Name { get; set; }
    }

    // For: TryCreateInstance_WithOverride_CreatesDerivedType
    public class CartLineItem
    {
        public string Sku { get; set; }
    }

    public class ConfigurableLineItem : CartLineItem
    {
    }

    // For: TryCreateInstance_AbstractBaseType_NoOverrides_Throws
    public abstract class DiscountPolicy
    {
    }

    // For: TryCreateInstance_AbstractBaseType_WithOverride_CreatesDerivedType
    public abstract class PaymentMethod
    {
        public string Code { get; set; }
    }

    public class CreditCardPayment : PaymentMethod
    {
    }

    // For: TryCreateInstance_RegisteredType_LookupByBaseTypeName
    public class ShoppingCart
    {
        public string Id { get; set; }
    }

    public class CustomShoppingCart : ShoppingCart
    {
    }

    // For: TryCreateInstance_RegisteredType_LookupByDerivedTypeName
    public class Promotion
    {
        public string Id { get; set; }
    }

    public class CouponPromotion : Promotion
    {
    }

    // For: OverrideType_ReplacesRegistration
    public class TaxProvider
    {
        public string Region { get; set; }
    }

    public class FixedTaxProvider : TaxProvider
    {
    }

    public class AvalaraTaxProvider : TaxProvider
    {
    }

    // For: OverrideType_ClearsInheritanceLookupCache
    public class PriceList
    {
        public string Id { get; set; }
    }

    public class CustomPriceList : PriceList
    {
    }

    // For: TryCreateInstance_WithArgs_UsesActivator
    public class ShipmentPackage
    {
        public string TrackingNumber { get; }
        public string Carrier { get; }

        public ShipmentPackage(string trackingNumber, string carrier)
        {
            TrackingNumber = trackingNumber;
            Carrier = carrier;
        }
    }

    // For: WithFactory_TakesPriorityOverAutoCompiledDelegate
    public class Catalog
    {
        public string Id { get; set; }
    }

    public class VirtualCatalog : Catalog
    {
    }

    // For: WithSetupAction_CalledAfterCreation
    public class Store
    {
        public string Id { get; set; }
    }

    // For: TryCreateInstance_WithDefault_ReturnsDefault_WhenTypeNotFound
    public class Category
    {
        public string Name { get; set; }
    }

    // For: FindTypeInfoByName_SecondCall_UsesCache
    public class CustomerOrder
    {
        public string Number { get; set; }
    }

    public class SubscriptionOrder : CustomerOrder
    {
    }

    // For: TryCreateInstance_WithArgs_FactoryTakesPriority
    public class Fulfillment
    {
        public string OperationType { get; set; }
    }

    public class WarehouseFulfillment : Fulfillment
    {
    }

    // For: WithTypeName_UpdatesDictionaryIndex
    public class Warehouse
    {
        public string Id { get; set; }
    }

    public class DropshipWarehouse : Warehouse
    {
    }

    // For: TryCreateInstance_WithArgs_NotPoisonedByPriorParameterlessCall
    public class Address
    {
        public string City { get; set; }

        public Address() { }

        public Address(string city)
        {
            City = city;
        }
    }

    // For: WithFactory_SetToNull_FallsBackToConstructor
    public class Notification
    {
        public string Channel { get; set; }
    }

    public class EmailNotification : Notification
    {
    }

    // For: RegisterType_AfterTryCreateInstance_InvalidatesCache
    public class Shipment
    {
        public string Method { get; set; }
    }

    public class ExpressShipment : Shipment
    {
    }

    // For: FindTypeInfoByName_DeepInheritance_ThreeLevels
    public class Asset
    {
        public string Name { get; set; }
    }

    public class DigitalAsset : Asset
    {
    }

    public class VideoAsset : DigitalAsset
    {
    }

    // For: TryCreateInstance_TypeWithoutParameterlessCtor_Throws
    public class DbConnection
    {
        public string ConnectionString { get; }

        public DbConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }

    // For: TryCreateInstance_WithDefaultAndArgs_UnknownType_ReturnsDefault
    public class AuditEvent
    {
        public string Description { get; set; }
    }

    // For: FindTypeInfoByName_NullTypeName_ReturnsNull
    public class Review
    {
        public string Rating { get; set; }
    }

    // For: FindTypeInfoByName_EmptyTypeName_ReturnsNull
    public class Wishlist
    {
        public string Name { get; set; }
    }

    // For: FindTypeInfoByName_CaseInsensitiveLookup
    public class FulfillmentCenter
    {
        public string Id { get; set; }
    }

    public class DropshipCenter : FulfillmentCenter
    {
    }

    // ── Tests ──────────────────────────────────────────────────────

    [Trait("Category", "Unit")]
    public class AbstractTypeFactoryTests
    {
        [Fact]
        public void TryCreateInstance_NoOverrides_CreatesBaseType()
        {
            var result = AbstractTypeFactory<Product>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.Equal(typeof(Product), result.GetType());
        }

        [Fact]
        public void TryCreateInstance_WithOverride_CreatesDerivedType()
        {
            AbstractTypeFactory<CartLineItem>.RegisterType<ConfigurableLineItem>();

            var result = AbstractTypeFactory<CartLineItem>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<ConfigurableLineItem>(result);
        }

        [Fact]
        public void TryCreateInstance_AbstractBaseType_NoOverrides_Throws()
        {
            Assert.Throws<OperationCanceledException>(() =>
                AbstractTypeFactory<DiscountPolicy>.TryCreateInstance());
        }

        [Fact]
        public void TryCreateInstance_AbstractBaseType_WithOverride_CreatesDerivedType()
        {
            AbstractTypeFactory<PaymentMethod>.RegisterType<CreditCardPayment>();

            var result = AbstractTypeFactory<PaymentMethod>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<CreditCardPayment>(result);
        }

        [Fact]
        public void TryCreateInstance_RegisteredType_LookupByBaseTypeName()
        {
            AbstractTypeFactory<ShoppingCart>.RegisterType<CustomShoppingCart>();

            var result = AbstractTypeFactory<ShoppingCart>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<CustomShoppingCart>(result);
        }

        [Fact]
        public void TryCreateInstance_RegisteredType_LookupByDerivedTypeName()
        {
            AbstractTypeFactory<Promotion>.RegisterType<CouponPromotion>();

            var result = AbstractTypeFactory<Promotion>.TryCreateInstance(nameof(CouponPromotion));

            Assert.NotNull(result);
            Assert.IsType<CouponPromotion>(result);
        }

        [Fact]
        public void OverrideType_ReplacesRegistration()
        {
            AbstractTypeFactory<TaxProvider>.RegisterType<FixedTaxProvider>();

            AbstractTypeFactory<TaxProvider>.OverrideType<FixedTaxProvider, AvalaraTaxProvider>();

            var result = AbstractTypeFactory<TaxProvider>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<AvalaraTaxProvider>(result);
        }

        [Fact]
        public void OverrideType_ClearsInheritanceLookupCache()
        {
            AbstractTypeFactory<PriceList>.RegisterType<PriceList>();

            var first = AbstractTypeFactory<PriceList>.TryCreateInstance();
            Assert.IsType<PriceList>(first);

            AbstractTypeFactory<PriceList>.OverrideType<PriceList, CustomPriceList>();

            var second = AbstractTypeFactory<PriceList>.TryCreateInstance();
            Assert.IsType<CustomPriceList>(second);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_UsesActivator()
        {
            AbstractTypeFactory<ShipmentPackage>.RegisterType<ShipmentPackage>();

            var result = AbstractTypeFactory<ShipmentPackage>.TryCreateInstance(
                nameof(ShipmentPackage), "TRACK-001", "FedEx");

            Assert.NotNull(result);
            Assert.Equal("TRACK-001", result.TrackingNumber);
            Assert.Equal("FedEx", result.Carrier);
        }

        [Fact]
        public void WithFactory_TakesPriorityOverAutoCompiledDelegate()
        {
            var factoryCalled = false;
            AbstractTypeFactory<Catalog>.RegisterType<VirtualCatalog>()
                .WithFactory(() =>
                {
                    factoryCalled = true;
                    return new VirtualCatalog { Id = "virtual" };
                });

            var result = AbstractTypeFactory<Catalog>.TryCreateInstance();

            Assert.True(factoryCalled);
            Assert.IsType<VirtualCatalog>(result);
            Assert.Equal("virtual", result.Id);
        }

        [Fact]
        public void WithSetupAction_CalledAfterCreation()
        {
            AbstractTypeFactory<Store>.RegisterType<Store>()
                .WithSetupAction(x => x.Id = "default-store");

            var result = AbstractTypeFactory<Store>.TryCreateInstance();

            Assert.Equal("default-store", result.Id);
        }

        [Fact]
        public void TryCreateInstance_WithDefault_ReturnsDefault_WhenTypeNotFound()
        {
            var defaultCategory = new Category { Name = "Uncategorized" };

            var result = AbstractTypeFactory<Category>.TryCreateInstance("NonExistentType", defaultCategory);

            Assert.Same(defaultCategory, result);
        }

        [Fact]
        public void FindTypeInfoByName_SecondCall_UsesCache()
        {
            AbstractTypeFactory<CustomerOrder>.RegisterType<SubscriptionOrder>();

            var first = AbstractTypeFactory<CustomerOrder>.FindTypeInfoByName(nameof(CustomerOrder));
            var second = AbstractTypeFactory<CustomerOrder>.FindTypeInfoByName(nameof(CustomerOrder));

            Assert.NotNull(first);
            Assert.Same(first, second);
            Assert.Equal(typeof(SubscriptionOrder), first.Type);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_FactoryTakesPriority()
        {
            AbstractTypeFactory<Fulfillment>.RegisterType<WarehouseFulfillment>()
                .WithFactory(() => new WarehouseFulfillment { OperationType = "Warehouse" });

            var result = AbstractTypeFactory<Fulfillment>.TryCreateInstance(nameof(WarehouseFulfillment), "ignored-arg");

            Assert.IsType<WarehouseFulfillment>(result);
            Assert.Equal("Warehouse", result.OperationType);
        }

        [Fact]
        public void WithTypeName_UpdatesDictionaryIndex()
        {
            AbstractTypeFactory<Warehouse>.RegisterType<DropshipWarehouse>()
                .WithTypeName("DropshipCenter");

            var result = AbstractTypeFactory<Warehouse>.TryCreateInstance("DropshipCenter");

            Assert.NotNull(result);
            Assert.IsType<DropshipWarehouse>(result);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_NotPoisonedByPriorParameterlessCall()
        {
            AbstractTypeFactory<Address>.RegisterType<Address>();

            // Parameterless — triggers auto-compilation of delegate
            var parameterless = AbstractTypeFactory<Address>.TryCreateInstance();
            Assert.NotNull(parameterless);
            Assert.Null(parameterless.City);

            // With args — must use Activator, NOT the cached delegate
            var withArgs = AbstractTypeFactory<Address>.TryCreateInstance(nameof(Address), "Seattle");
            Assert.NotNull(withArgs);
            Assert.Equal("Seattle", withArgs.City);
        }

        // ── Gap coverage tests ─────────────────────────────────────

        [Fact]
        public void WithFactory_SetToNull_FallsBackToConstructor()
        {
            AbstractTypeFactory<Notification>.RegisterType<EmailNotification>()
                .WithFactory(() => new EmailNotification { Channel = "email" });

            var withFactory = AbstractTypeFactory<Notification>.TryCreateInstance();
            Assert.Equal("email", withFactory.Channel);

            // Clear factory — should fall back to parameterless constructor
            AbstractTypeFactory<Notification>.FindTypeInfoByName(nameof(EmailNotification))
                .WithFactory(null);

            var withoutFactory = AbstractTypeFactory<Notification>.TryCreateInstance();
            Assert.NotNull(withoutFactory);
            Assert.Null(withoutFactory.Channel);
        }

        [Fact]
        public void RegisterType_AfterTryCreateInstance_InvalidatesCache()
        {
            // TryCreateInstance with no overrides → caches _defaultFactory
            var first = AbstractTypeFactory<Shipment>.TryCreateInstance();
            Assert.IsType<Shipment>(first);

            // Late registration — must invalidate _defaultFactory
            AbstractTypeFactory<Shipment>.RegisterType<ExpressShipment>();

            var second = AbstractTypeFactory<Shipment>.TryCreateInstance();
            Assert.IsType<ExpressShipment>(second);
        }

        [Fact]
        public void FindTypeInfoByName_DeepInheritance_ThreeLevels()
        {
            // Asset → DigitalAsset → VideoAsset (3-level chain)
            AbstractTypeFactory<Asset>.RegisterType<VideoAsset>();

            var byAsset = AbstractTypeFactory<Asset>.FindTypeInfoByName(nameof(Asset));
            Assert.NotNull(byAsset);
            Assert.Equal(typeof(VideoAsset), byAsset.Type);

            var byDigitalAsset = AbstractTypeFactory<Asset>.FindTypeInfoByName(nameof(DigitalAsset));
            Assert.NotNull(byDigitalAsset);
            Assert.Equal(typeof(VideoAsset), byDigitalAsset.Type);

            var byVideoAsset = AbstractTypeFactory<Asset>.FindTypeInfoByName(nameof(VideoAsset));
            Assert.NotNull(byVideoAsset);
            Assert.Equal(typeof(VideoAsset), byVideoAsset.Type);
        }

        [Fact]
        public void TryCreateInstance_TypeWithoutParameterlessCtor_Throws()
        {
            AbstractTypeFactory<DbConnection>.RegisterType<DbConnection>();

            Assert.Throws<MissingMethodException>(() =>
                AbstractTypeFactory<DbConnection>.TryCreateInstance());
        }

        [Fact]
        public void TryCreateInstance_WithDefaultAndArgs_UnknownType_ReturnsDefault()
        {
            var defaultEvent = new AuditEvent { Description = "default" };

            var result = AbstractTypeFactory<AuditEvent>.TryCreateInstance("UnknownType", defaultEvent, "arg1");

            Assert.Same(defaultEvent, result);
        }

        [Fact]
        public void FindTypeInfoByName_NullTypeName_ReturnsNull()
        {
            AbstractTypeFactory<Review>.RegisterType<Review>();

            var result = AbstractTypeFactory<Review>.FindTypeInfoByName(null);

            Assert.Null(result);
        }

        [Fact]
        public void FindTypeInfoByName_EmptyTypeName_ReturnsNull()
        {
            AbstractTypeFactory<Wishlist>.RegisterType<Wishlist>();

            var result = AbstractTypeFactory<Wishlist>.FindTypeInfoByName(string.Empty);

            Assert.Null(result);
        }

        [Fact]
        public void FindTypeInfoByName_CaseInsensitiveLookup()
        {
            AbstractTypeFactory<FulfillmentCenter>.RegisterType<DropshipCenter>()
                .WithTypeName("DropshipCenter");

            var result = AbstractTypeFactory<FulfillmentCenter>.TryCreateInstance("dropshipcenter");

            Assert.NotNull(result);
            Assert.IsType<DropshipCenter>(result);
        }
    }
}
