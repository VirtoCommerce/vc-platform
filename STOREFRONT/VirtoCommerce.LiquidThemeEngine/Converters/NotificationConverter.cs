using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class NotificationConverter
    {
        public static Notification ToShopifyModel(this StorefrontNotification storefrontModel)
        {
            var shopifyModel = new Notification();

            shopifyModel.Message = storefrontModel.Message;
            shopifyModel.Title = storefrontModel.Title;
            shopifyModel.Type = storefrontModel.Type.ToString();

            return shopifyModel;
        }
    }
}