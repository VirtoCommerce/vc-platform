using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShippingMethodConverter
    {
        public static ShippingMethod ToShopifyModel(this Shipment shipment)
        {
            var ret = new ShippingMethod
            {
                Price = shipment.Sum.Amount,
                Title = shipment.ShipmentMethodCode,
                Handle = shipment.ShipmentMethodCode
            };

            return ret;
        }
    }
}
