using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;


namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LinkListConverter
    {
        public static Linklist ToShopifyModel(this StorefrontModel.MenuLinkList storefrontModel, Storefront.Model.WorkContext workContext,  IStorefrontUrlBuilder urlBuilder)
        {
            var shopifyModel = new Linklist();

            shopifyModel.Handle = storefrontModel.Name;
            shopifyModel.Id = storefrontModel.Id;
            shopifyModel.Links = storefrontModel.MenuLinks.Select(ml => ml.ToShopfiyModel(workContext, urlBuilder)).ToList();
            shopifyModel.Title = storefrontModel.Name;

            return shopifyModel;
        }

        public static Link ToShopfiyModel(this StorefrontModel.MenuLink storefrontModel, Storefront.Model.WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            var shopifyModel = new Link();

            shopifyModel.Active = storefrontModel.IsActive;
            shopifyModel.Object = "";
            shopifyModel.Title = storefrontModel.Title;
            shopifyModel.Type = "";
            shopifyModel.Url = urlBuilder.ToAppAbsolute(storefrontModel.Url);

            var productLink = storefrontModel as StorefrontModel.ProductMenuLink;
            var categoryLink = storefrontModel as StorefrontModel.CategoryMenuLink;
            if (productLink != null)
            {
                shopifyModel.Type = "product";
                if (productLink.Product != null)
                {
                    shopifyModel.Object = productLink.Product.ToShopifyModel();
                }
            }
            if (categoryLink != null)
            {
                shopifyModel.Type = "collection";
                if (categoryLink.Category != null)
                {
                    shopifyModel.Object = categoryLink.Category.ToShopifyModel(workContext);
                }
            }
            return shopifyModel;
        }
    }
}