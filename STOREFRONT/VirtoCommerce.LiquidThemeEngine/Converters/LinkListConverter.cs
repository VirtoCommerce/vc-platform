using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;


namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LinkListConverter
    {
        public static Linklist ToShopifyModel(this StorefrontModel.MenuLinkList storefrontModel)
        {
            var shopifyModel = new Linklist();

            shopifyModel.Handle = storefrontModel.Name;
            shopifyModel.Id = storefrontModel.Id;
            shopifyModel.Links = storefrontModel.MenuLinks.Select(ml => ml.ToShopfiyModel()).ToList();
            shopifyModel.Title = storefrontModel.Name;

            return shopifyModel;
        }

        public static Link ToShopfiyModel(this StorefrontModel.MenuLink storefrontModel)
        {
            var shopifyModel = new Link();

            shopifyModel.Active = storefrontModel.IsActive;
            shopifyModel.Object = "";
            shopifyModel.Title = storefrontModel.Title;
            shopifyModel.Type = "";
            shopifyModel.Url = storefrontModel.Url;

            return shopifyModel;
        }
    }
}