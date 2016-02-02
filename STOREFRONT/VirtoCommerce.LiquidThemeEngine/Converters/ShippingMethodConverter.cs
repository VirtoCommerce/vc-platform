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
                Price = shipment.Sum.Amount * 100,
                Title = shipment.ShipmentMethodCode,
                Handle = shipment.ShipmentMethodCode
            };

            return ret;
        }

        public static ShippingMethod ToShopifyModel(this Storefront.Model.ShippingMethod storefrontModel)
        {
            var shopifyModel = new ShippingMethod();

            shopifyModel.Handle = storefrontModel.ShipmentMethodCode;
            shopifyModel.Price = storefrontModel.Price.Amount;
            shopifyModel.TaxType = storefrontModel.TaxType;
            shopifyModel.Title = storefrontModel.Name;

            return shopifyModel;
        }
    }
}