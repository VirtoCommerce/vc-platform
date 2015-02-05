using Microsoft.Practices.Unity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Data.Orders
{
    /// <summary>
    /// Implements Order,Country, Payment, Shipping, Tax and Fulfillment repositories using Entity Framework.
    /// </summary>
    public class EFOrderRepository : EFRepositoryBase, IOrderRepository, ICountryRepository, IPaymentMethodRepository, IShippingRepository, ITaxRepository, IFulfillmentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EFOrderRepository"/> class.
        /// </summary>
        public EFOrderRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFOrderRepository"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public EFOrderRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFOrderRepository>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFOrderRepository"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        /// <param name="interceptors">The interceptors.</param>
        [InjectionConstructor]
        public EFOrderRepository(IOrderEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : this(OrderConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
        {
        }

        public EFOrderRepository(string connectionStringName, IOrderEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(connectionStringName, entityFactory, interceptors: interceptors)
        {
            Database.SetInitializer(new ValidateDatabaseInitializer<EFOrderRepository>());

            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            TPHMaping(modelBuilder);

            MapEntity<LineItem>(modelBuilder, toTable: "LineItem");
            MapEntity<OrderForm>(modelBuilder, toTable: "OrderForm");
            MapEntity<Shipment>(modelBuilder, toTable: "Shipment");
            MapEntity<ShipmentItem>(modelBuilder, toTable: "ShipmentItem");
            MapEntity<ShipmentOption>(modelBuilder, toTable: "ShipmentOption");
            MapEntity<LineItemOption>(modelBuilder, toTable: "LineItemOption");
            MapEntity<RmaRequest>(modelBuilder, toTable: "RmaRequest");
            MapEntity<RmaReturnItem>(modelBuilder, toTable: "RmaReturnItem");
            MapEntity<RmaLineItem>(modelBuilder, toTable: "RmaLineItem");
            MapEntity<OrderAddress>(modelBuilder, toTable: "OrderAddress");
            MapEntity<OrderFormDiscount>(modelBuilder, toTable: "OrderFormDiscount");
            MapEntity<LineItemDiscount>(modelBuilder, toTable: "LineItemDiscount");
            MapEntity<ShipmentDiscount>(modelBuilder, toTable: "ShipmentDiscount");

            MapEntity<GatewayProperty>(modelBuilder, toTable: "GatewayProperty");
            MapEntity<GatewayPropertyDictionaryValue>(modelBuilder, toTable: "GatewayPropertyDictionaryValue");

            MapEntity<ShippingOption>(modelBuilder, toTable: "ShippingOption");
            MapEntity<ShippingMethod>(modelBuilder, toTable: "ShippingMethod");
            MapEntity<ShippingMethodLanguage>(modelBuilder, toTable: "ShippingMethodLanguage");

            MapEntity<ShippingMethodCase>(modelBuilder, toTable: "ShippingMethodCase");
            MapEntity<ShippingPackage>(modelBuilder, toTable: "ShippingPackage");

            MapEntity<Tax>(modelBuilder, toTable: "Tax");
            MapEntity<TaxLanguage>(modelBuilder, toTable: "TaxLanguage");
            MapEntity<TaxValue>(modelBuilder, toTable: "TaxValue");

            MapEntity<Jurisdiction>(modelBuilder, toTable: "Jurisdiction");
            MapEntity<JurisdictionGroup>(modelBuilder, toTable: "JurisdictionGroup");
            MapEntity<JurisdictionRelation>(modelBuilder, toTable: "JurisdictionRelation");

            MapEntity<PaymentMethod>(modelBuilder, toTable: "PaymentMethod");
            MapEntity<PaymentMethodShippingMethod>(modelBuilder, toTable: "PaymentMethodShippingMethod");
            MapEntity<PaymentMethodLanguage>(modelBuilder, toTable: "PaymentMethodLanguage");
            MapEntity<PaymentMethodPropertyValue>(modelBuilder, toTable: "PaymentMethodPropertyValue");

            MapEntity<Country>(modelBuilder, toTable: "Country");
            MapEntity<Region>(modelBuilder, toTable: "Region");
            MapEntity<Picklist>(modelBuilder, toTable: "Picklist");

            #region Line Item
            modelBuilder.Entity<ShipmentItem>().HasRequired(s => s.LineItem).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<LineItemDiscount>().HasRequired(o => o.LineItem).WithMany(o => o.Discounts);
            modelBuilder.Entity<LineItemOption>().HasRequired(o => o.LineItem).WithMany(o => o.Options);
            modelBuilder.Entity<RmaLineItem>().HasRequired(o => o.LineItem).WithMany().WillCascadeOnDelete(false);
            #endregion

            #region Order
            modelBuilder.Entity<RmaRequest>().HasRequired(o => o.Order).WithMany(o => o.RmaRequests);
            modelBuilder.Entity<OrderAddress>().HasRequired(o => o.OrderGroup).WithMany(o => o.OrderAddresses);
            modelBuilder.Entity<OrderForm>().HasRequired(o => o.OrderGroup).WithMany(o => o.OrderForms);
            #endregion

            #region OrderForm
            modelBuilder.Entity<LineItem>().HasRequired(o => o.OrderForm).WithMany(o => o.LineItems);
            modelBuilder.Entity<Shipment>().HasRequired(o => o.OrderForm).WithMany(o => o.Shipments);
            modelBuilder.Entity<OrderFormDiscount>().HasRequired(o => o.OrderForm).WithMany(o => o.Discounts);
            modelBuilder.Entity<Payment>().HasRequired(o => o.OrderForm).WithMany(o => o.Payments);
            #endregion

            #region Shipment
            modelBuilder.Entity<ShipmentItem>().HasRequired(o => o.Shipment).WithMany(o => o.ShipmentItems);
            modelBuilder.Entity<ShipmentDiscount>().HasRequired(o => o.Shipment).WithMany(o => o.Discounts);
            modelBuilder.Entity<ShipmentOption>().HasRequired(o => o.Shipment).WithMany(o => o.Options);
            #endregion

            modelBuilder.Entity<RmaLineItem>().HasRequired(o => o.RmaReturnItem).WithMany(o => o.RmaLineItems);

            modelBuilder.Entity<Region>().HasRequired(o => o.Country).WithMany(o => o.Regions);

            modelBuilder.Entity<GatewayProperty>().HasRequired(o => o.Gateway).WithMany(o => o.GatewayProperties);
            modelBuilder.Entity<GatewayPropertyDictionaryValue>().HasRequired(o => o.GatewayProperty).WithMany(o => o.GatewayPropertyDictionaryValues);

            #region Jurisdiction
            modelBuilder.Entity<JurisdictionRelation>().HasRequired(o => o.JurisdictionGroup).WithMany(o => o.JurisdictionRelations);
            modelBuilder.Entity<JurisdictionRelation>().HasRequired(o => o.Jurisdiction).WithMany();
            #endregion

            #region PaymentMethod
            modelBuilder.Entity<PaymentMethodLanguage>().HasRequired(o => o.PaymentMethod).WithMany(o => o.PaymentMethodLanguages);
            modelBuilder.Entity<PaymentMethodPropertyValue>().HasRequired(o => o.PaymentMethod).WithMany(o => o.PaymentMethodPropertyValues);
            modelBuilder.Entity<PaymentMethodShippingMethod>().HasRequired(o => o.PaymentMethod).WithMany(o => o.PaymentMethodShippingMethods);
            modelBuilder.Entity<PaymentMethodShippingMethod>().HasRequired(o => o.ShippingMethod).WithMany(o => o.PaymentMethodShippingMethods);
            #endregion

            #region ShippingMethod
            modelBuilder.Entity<ShippingGatewayPropertyValue>().HasRequired(o => o.ShippingOption).WithMany(o => o.ShippingGatewayPropertyValues);
            modelBuilder.Entity<ShippingMethodCase>().HasRequired(o => o.ShippingMethod).WithMany(o => o.ShippingMethodCases);
            modelBuilder.Entity<ShippingMethodJurisdictionGroup>().HasRequired(o => o.ShippingMethod).WithMany(o => o.ShippingMethodJurisdictionGroups);
            modelBuilder.Entity<ShippingMethodJurisdictionGroup>().HasRequired(o => o.JurisdictionGroup).WithMany(o => o.ShippingMethodJurisdictionGroups);
            modelBuilder.Entity<ShippingMethodLanguage>().HasRequired(o => o.ShippingMethod).WithMany(o => o.ShippingMethodLanguages);
            modelBuilder.Entity<ShippingPackage>().HasRequired(o => o.ShippingOption).WithMany(o => o.ShippingPackages);
            #endregion

            #region Taxes
            modelBuilder.Entity<TaxLanguage>().HasRequired(o => o.Tax).WithMany(o => o.TaxLanguages);
            modelBuilder.Entity<TaxValue>().HasRequired(o => o.Tax).WithMany(o => o.TaxValues);
            modelBuilder.Entity<TaxValue>().HasRequired(o => o.JurisdictionGroup).WithMany();
            #endregion

            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// TPCs the mapping.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private void TPCMaping(DbModelBuilder modelBuilder)
        {
            #region OrderGroup
            modelBuilder.Entity<Order>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("Order");
            });

            modelBuilder.Entity<ShoppingCart>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("ShoppingCart");
            });
            #endregion

            #region Payments
            modelBuilder.Entity<GiftCartPayment>().Map(entity =>
                {
                    entity.MapInheritedProperties();
                    entity.ToTable("GiftCartPayment");
                    //entity.Requires("Descriminator").HasValue("GiftCartPayment");
                });
            modelBuilder.Entity<ExchangePayment>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("ExchangePayment");
                //entity.Requires("Descriminator").HasValue("GiftCartPayment");
            });

            modelBuilder.Entity<CreditCardPayment>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("CreditCardPayment");
                //entity.Requires("Descriminator").HasValue("GiftCartPayment");
            });

            modelBuilder.Entity<CashCardPayment>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("CashCardPayment");
                //entity.Requires("Descriminator").HasValue("GiftCartPayment");
            });

            modelBuilder.Entity<InvoicePayment>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("InvoicePayment");
                //entity.Requires("Descriminator").HasValue("GiftCartPayment");
            });

            modelBuilder.Entity<OtherPayment>().Map(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("OtherPayment");
                //entity.Requires("Descriminator").HasValue("GiftCartPayment");
            });
            #endregion

            #region Discounts
            modelBuilder.Entity<OrderFormDiscount>().Map(entity =>
            {
                entity.ToTable("OrderFormDiscount");
                entity.MapInheritedProperties();
                //entity.Requires("Descriminator").HasValue("OrderFormDiscount");
            });
            modelBuilder.Entity<LineItemDiscount>().Map<LineItemDiscount>(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("LineItemDiscount");
                // entity.Requires("Descriminator").HasValue("LineItemDiscount");
            });
            modelBuilder.Entity<ShipmentDiscount>().Map<ShipmentDiscount>(entity =>
            {
                entity.MapInheritedProperties();
                entity.ToTable("ShipmentDiscount");
                //entity.Requires("Descriminator").HasValue("ShipmentDiscount");
            });
            #endregion
        }

        /// <summary>
        /// TPHs the mapping.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private void TPHMaping(DbModelBuilder modelBuilder)
        {
            #region OrderGroup
            MapEntity<OrderGroup>(modelBuilder, toTable: "OrderGroup");
            MapEntity<Order>(modelBuilder, toTable: "OrderGroup", discriminatorValue: "Order");
            MapEntity<ShoppingCart>(modelBuilder, toTable: "OrderGroup", discriminatorValue: "ShoppingCart");

            // add indexes
            modelBuilder.Entity<OrderGroup>().Property(t => t.CustomerId).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            modelBuilder.Entity<OrderGroup>().Property(t => t.StoreId).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            #endregion

            #region Gateway
            MapEntity<Gateway>(modelBuilder, toTable: "Gateway");
            MapEntity<PaymentGateway>(modelBuilder, toTable: "Gateway", discriminatorValue: "PaymentGateway");
            MapEntity<ShippingGateway>(modelBuilder, toTable: "Gateway", discriminatorValue: "ShippingGateway");
            #endregion

            #region Payments
            MapEntity<Payment>(modelBuilder, toTable: "Payment");
            MapEntity<GiftCartPayment>(modelBuilder, toTable: "Payment", discriminatorValue: "GiftCartPayment");
            MapEntity<ExchangePayment>(modelBuilder, toTable: "Payment", discriminatorValue: "ExchangePayment");
            MapEntity<CashCardPayment>(modelBuilder, toTable: "Payment", discriminatorValue: "CashCardPayment");
            MapEntity<InvoicePayment>(modelBuilder, toTable: "Payment", discriminatorValue: "InvoicePayment");
            MapEntity<OtherPayment>(modelBuilder, toTable: "Payment", discriminatorValue: "OtherPayment");
            MapEntity<CreditCardPayment>(modelBuilder, toTable: "Payment", discriminatorValue: "CreditCardPayment");
            #endregion
        }

        /// <summary>
        /// TPTs the mapping.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private void TPTMaping(DbModelBuilder modelBuilder)
        {
            #region OrderGroup
            modelBuilder.Entity<OrderGroup>().Map(entity =>
            {
                entity.ToTable("OrderGroup");
            })
            .Map<Order>(entity =>
            {
                entity.ToTable("Order");
            })
            .Map<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");
            });
            #endregion

            #region Payments
            modelBuilder.Entity<Payment>().Map(entity =>
            {
                entity.ToTable("Payment");
            })
            .Map<ExchangePayment>(entity =>
            {
                entity.ToTable("ExchangePayment");
            })
                //.Map<CreditCardPayment>(entity =>
                //{
                //    entity.ToTable("CreditCardPayment");
                //})
            .Map<CashCardPayment>(entity =>
            {
                entity.ToTable("CashCardPayment");
            })
            .Map<InvoicePayment>(entity =>
            {
                entity.ToTable("InvoicePayment");
            })
            .Map<OtherPayment>(entity =>
            {
                entity.ToTable("OtherPayment");
            });
            #endregion


            #region Discounts
            modelBuilder.Entity<Discount>().Map(entity =>
                {
                    entity.ToTable("Discount");
                })
                .Map<OrderFormDiscount>(entity =>
                {
                    entity.ToTable("OrderFormDiscount");
                })
                .Map<LineItemDiscount>(entity =>
                {
                    entity.ToTable("LineItemDiscount");
                })
                .Map<ShipmentDiscount>(entity =>
                {
                    entity.ToTable("ShipmentDiscount");
                });
            #endregion
        }

        #region IOrderRepository Members
        public IQueryable<LineItemDiscount> LineItemDiscounts
        {
            get { return GetAsQueryable<LineItemDiscount>(); }
        }

        public IQueryable<Order> Orders
        {
            get { return GetAsQueryable<Order>(); }
        }

        public IQueryable<ShoppingCart> ShoppingCarts
        {
            get { return GetAsQueryable<ShoppingCart>(); }
        }

        public IQueryable<Shipment> Shipments
        {
            get { return GetAsQueryable<Shipment>(); }
        }

        public IQueryable<RmaRequest> RmaRequests
        {
            get { return GetAsQueryable<RmaRequest>(); }
        }

        public IQueryable<LineItem> LineItems
        {
            get { return GetAsQueryable<LineItem>(); }
        }

        public IQueryable<OrderAddress> OrderAddresses
        {
            get { return GetAsQueryable<OrderAddress>(); }
        }

        public IQueryable<Payment> Payments
        {
            get { return GetAsQueryable<Payment>(); }
        }

        public IQueryable<Jurisdiction> Jurisdictions
        {
            get { return GetAsQueryable<Jurisdiction>(); }
        }

        public IQueryable<JurisdictionGroup> JurisdictionGroups
        {
            get { return GetAsQueryable<JurisdictionGroup>(); }
        }



        #endregion

        #region ICountryRepository Members

        public IQueryable<Country> Countries
        {
            get { return GetAsQueryable<Country>(); }
        }

        #endregion

        #region IPaymentMethodRepository Members

        public IQueryable<PaymentMethod> PaymentMethods
        {
            get { return GetAsQueryable<PaymentMethod>(); }
        }

        public IQueryable<PaymentGateway> PaymentGateways
        {
            get { return GetAsQueryable<PaymentGateway>(); }
        }

        public IQueryable<PaymentMethodShippingMethod> PaymentMethodShippingMethods
        {
            get { return GetAsQueryable<PaymentMethodShippingMethod>(); }
        }

        public IQueryable<PaymentMethodLanguage> PaymentMethodLanguages
        {
            get { return GetAsQueryable<PaymentMethodLanguage>(); }
        }

        public IQueryable<PaymentMethodPropertyValue> PaymentPropertyValues
        {
            get { return GetAsQueryable<PaymentMethodPropertyValue>(); }
        }

        #endregion

        #region IShippingRepository Members

        public IQueryable<ShippingOption> ShippingOptions
        {
            get { return GetAsQueryable<ShippingOption>(); }
        }

        public IQueryable<ShippingGateway> ShippingGateways
        {
            get { return GetAsQueryable<ShippingGateway>(); }
        }

        public IQueryable<ShippingMethod> ShippingMethods
        {
            get { return GetAsQueryable<ShippingMethod>(); }
        }

        public IQueryable<ShippingMethodLanguage> ShippingMethodLanguages
        {
            get { return GetAsQueryable<ShippingMethodLanguage>(); }
        }

        public IQueryable<ShippingMethodJurisdictionGroup> ShippingMethodJurisdictionGroups
        {
            get { return GetAsQueryable<ShippingMethodJurisdictionGroup>(); }
        }

        public IQueryable<ShippingPackage> ShippingPackages
        {
            get { return GetAsQueryable<ShippingPackage>(); }
        }

        #endregion

        #region ITaxRepository Members

        public IQueryable<Tax> Taxes
        {
            get { return GetAsQueryable<Tax>(); }
        }

        #endregion

        #region IFulfillmentRepository Members

        public IQueryable<Picklist> Picklists
        {
            get { return GetAsQueryable<Picklist>(); }
        }

        #endregion

    }
}
