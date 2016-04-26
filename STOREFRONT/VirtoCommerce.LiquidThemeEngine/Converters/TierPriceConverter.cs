using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class TierPriceConverter
    {
        public static TierPrice ToShopifyModel(this Storefront.Model.TierPrice storefrontModel)
        {
            var shopifyModel = new TierPrice();

            shopifyModel.Price = storefrontModel.Price.Amount * 100;
            shopifyModel.Quantity = storefrontModel.Quantity;

            return shopifyModel;
        }
    }
}