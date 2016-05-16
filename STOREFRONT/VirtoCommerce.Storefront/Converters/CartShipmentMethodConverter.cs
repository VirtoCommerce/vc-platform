using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartShipmentMethodConverter
    {
        public static ShippingMethod ToWebModel(this VirtoCommerceCartModuleWebModelShippingMethod shippingMethod, Currency currency)
        {
            var shippingMethodModel = new ShippingMethod(currency);
            shippingMethodModel.InjectFrom(shippingMethod);       
            shippingMethodModel.Price = new Money(shippingMethod.Price ?? 0, currency);
            return shippingMethodModel;
        }

        public static Shipment ToShipmentModel(this ShippingMethod shippingMethod, Currency currency)
        {
            var shipmentWebModel = new Shipment(currency);

            shipmentWebModel.ShipmentMethodCode = shippingMethod.ShipmentMethodCode;
            shipmentWebModel.ShippingPrice = shippingMethod.Price;
            shipmentWebModel.TaxType = shippingMethod.TaxType;

            return shipmentWebModel;
        }
    }
}