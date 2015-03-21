using Microsoft.Practices.Unity;
using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
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
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data.Orders
{
    public class DSOrderClient : DSClientBase, IOrderRepository, ICountryRepository, IPaymentMethodRepository, IShippingRepository, ITaxRepository, IFulfillmentRepository
    {
        [InjectionConstructor]
        public DSOrderClient(IOrderEntityFactory orderEntityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
            : base(connFactory.GetConnectionString(OrderConfiguration.Instance.Connection.DataServiceUri), orderEntityFactory, tokenInjector)
        {
        }

        public DSOrderClient(Uri serviceUri, IOrderEntityFactory orderEntityFactory, ISecurityTokenInjector tokenInjector)
            : base(serviceUri, orderEntityFactory, tokenInjector)
        {
        }

        #region IOrderRepository Members

        public IQueryable<Order> Orders
        {
            get { return GetAsQueryable<OrderGroup>().OfType<Order>(); }
        }

        public IQueryable<ShoppingCart> ShoppingCarts
        {
            get { return GetAsQueryable<OrderGroup>().OfType<ShoppingCart>(); }
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
            get { return GetAsQueryable<Gateway>().OfType<PaymentGateway>(); }
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
            get { return GetAsQueryable<Gateway>().OfType<ShippingGateway>(); }
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
