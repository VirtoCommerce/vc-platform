using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartShipmentMethodConverter
    {
        public static ShippingMethod ToWebModel(this VirtoCommerceCartModuleWebModelShippingMethod shippingMethod)
        {
            var shippingMethodModel = new ShippingMethod();

            shippingMethodModel.InjectFrom(shippingMethod);

            var currency = new Currency(EnumUtility.SafeParse(shippingMethod.Currency, CurrencyCodes.USD));

            if (shippingMethod.Discounts != null)
            {
                shippingMethodModel.Discounts = shippingMethod.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            shippingMethodModel.Price = new Money(shippingMethod.Price ?? 0, currency.Code);

            return shippingMethodModel;
        }

        public static Shipment ToShipmentModel(this ShippingMethod shippingMethod, Currency currency)
        {
            var shipmentWebModel = new Shipment();

            shipmentWebModel.Currency = currency;
            shipmentWebModel.ShipmentMethodCode = shippingMethod.ShipmentMethodCode;
            shipmentWebModel.ShippingPrice = shippingMethod.Price;
            shipmentWebModel.TaxType = shippingMethod.TaxType;

            return shipmentWebModel;
        }
    }
}