using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class TaxLineConverter
    {
        public static TaxLine ToShopifyModel(this Storefront.Model.TaxDetail storefrontModel)
        {
            var shopifyModel = new TaxLine();

            shopifyModel.Price = storefrontModel.Amount.Amount * 100;
            shopifyModel.Title = storefrontModel.Name;

            return shopifyModel;
        }
    }
}