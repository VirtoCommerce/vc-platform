using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.CQRS.Messages;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Model.Taxes;

namespace VirtoCommerce.Foundation.Orders.Factories
{
	public class OrderEntityFactory : FactoryBase, IOrderEntityFactory
	{
		public OrderEntityFactory()
		{
			RegisterStorageType(typeof(ShoppingCart), "ShoppingCart");
			RegisterStorageType(typeof(Order), "Order");
			RegisterStorageType(typeof(LineItem), "LineItem");
			RegisterStorageType(typeof(OrderForm), "OrderForm");
			RegisterStorageType(typeof(OrderFormPropertyValue), "OrderFormPropertyValue");
			RegisterStorageType(typeof(OrderFormDiscount), "OrderFormDiscount");
			RegisterStorageType(typeof(LineItemDiscount), "LineItemDiscount");
			RegisterStorageType(typeof(ShipmentDiscount), "ShipmentDiscount");
			RegisterStorageType(typeof(OrderAddress), "OrderAddress");
			RegisterStorageType(typeof(GiftCartPayment), "GiftCartPayment");
			RegisterStorageType(typeof(ExchangePayment), "ExchangePayment");
			RegisterStorageType(typeof(CreditCardPayment), "CreditCardPayment");
			RegisterStorageType(typeof(CashCardPayment), "CashCardPayment");
			RegisterStorageType(typeof(InvoicePayment), "InvoicePayment");
			RegisterStorageType(typeof(OtherPayment), "OtherPayment");
			RegisterStorageType(typeof(Shipment), "Shipment");
			RegisterStorageType(typeof(ShipmentItem), "ShipmentItem");
			RegisterStorageType(typeof(LineItemOption), "LineItemOption");
            RegisterStorageType(typeof(ShipmentOption), "ShipmentOption");
			RegisterStorageType(typeof(RmaRequest), "RmaRequest");
			RegisterStorageType(typeof(RmaReturnItem), "RmaReturnItem");
			RegisterStorageType(typeof(RmaLineItem), "RmaLineItem");

			RegisterStorageType(typeof(ShippingOption), "ShippingOption");
			RegisterStorageType(typeof(ShippingMethod), "ShippingMethod");
			RegisterStorageType(typeof(ShippingMethodLanguage), "ShippingMethodLanguage");
			RegisterStorageType(typeof(ShippingMethodCase), "ShippingMethodCase");
			RegisterStorageType(typeof(ShippingPackage), "ShippingPackage");
			RegisterStorageType(typeof(ShippingGatewayPropertyValue), "ShippingGatewayPropertyValue");

			// gateways
			RegisterStorageType(typeof(ShippingGateway), "ShippingGateway");
			RegisterStorageType(typeof(PaymentGateway), "PaymentGateway");
			RegisterStorageType(typeof(GatewayProperty), "GatewayProperty");
			RegisterStorageType(typeof(GatewayPropertyDictionaryValue), "GatewayPropertyDictionaryValue");

			RegisterStorageType(typeof(Tax), "Tax");
			RegisterStorageType(typeof(TaxLanguage), "TaxLanguage");
			RegisterStorageType(typeof(TaxValue), "TaxValue");

			RegisterStorageType(typeof(Jurisdiction), "Jurisdiction");
			RegisterStorageType(typeof(JurisdictionGroup), "JurisdictionGroup");
			RegisterStorageType(typeof(JurisdictionRelation), "JurisdictionRelation");
			RegisterStorageType(typeof(ShippingMethodJurisdictionGroup), "ShippingMethodJurisdictionGroup");

			RegisterStorageType(typeof(PaymentMethod), "PaymentMethod");
			RegisterStorageType(typeof(PaymentMethodShippingMethod), "PaymentMethodShippingMethod");
			RegisterStorageType(typeof(PaymentMethodLanguage), "PaymentMethodLanguage");
			RegisterStorageType(typeof(PaymentMethodPropertyValue), "PaymentMethodPropertyValue");


			RegisterStorageType(typeof(Country), "Country");
			RegisterStorageType(typeof(Region), "Region");

			RegisterStorageType(typeof(SaveOrderGroupChangesMessage), "SaveOrderGroupChangesMesssage");

			RegisterStorageType(typeof(Picklist), "Picklist");
		}
	}
}

