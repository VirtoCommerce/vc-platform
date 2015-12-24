using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LoginFormConverter
    {
        public static Form ToShopifyModel(this StorefrontModel.Login storefrontModel)
        {
            var shopifyModel = new Form();

            shopifyModel.PasswordNeeded = true;

            return shopifyModel;
        }
    }
}