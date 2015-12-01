using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopifyContextConverter
    {
        public static ShopifyThemeWorkContext ToShopifyModel(this storefrontModel.WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new ShopifyThemeWorkContext();

            result.CurrentPage = 1;
            result.CountryOptionTags = string.Join("\r\n", workContext.Countries.OrderBy(c => c.Name).Select(c => c.ToOptionTag()));
            result.PageDescription = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.MetaDescription : string.Empty;
            result.PageTitle = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.Title : string.Empty;
            result.Shop = workContext.CurrentStore.ToShopifyModel(workContext);
            result.Cart = workContext.CurrentCart.ToShopifyModel(workContext);
            result.Product = workContext.CurrentProduct != null ? workContext.CurrentProduct.ToShopifyModel(workContext) : null;
            result.Customer = workContext.CurrentCustomer.HasAccount ? workContext.CurrentCustomer.ToShopifyModel(workContext, urlBuilder) : null;
            result.AllStores = workContext.AllStores.Select(x => x.ToShopifyModel(workContext)).ToArray();

            result.CurrentCurrency = workContext.CurrentCurrency.ToShopifyModel();
            result.CurrentLanguage = workContext.CurrentLanguage.ToShopifyModel();

            if (workContext.CurrentProduct != null && workContext.CurrentProduct.Category != null)
            {
                result.Collection = workContext.CurrentProduct.Category.ToShopifyModel(workContext);
            }

            var searchResult = workContext.CurrentCatalogSearchResult;
            if (searchResult != null)
            {
                result.Collection = searchResult.ToShopifyModel(workContext);

                if (searchResult.Categories != null)
                {
                    result.Collections = new Collections(searchResult.Categories.Select(x => x.ToShopifyModel(workContext)));
                }
            }

            return result;
        }
    }
}
