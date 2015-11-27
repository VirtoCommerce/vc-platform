using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopifyContextConverter
    {
        public static ShopifyThemeWorkContext ToShopifyModel(this storefrontModel.WorkContext workContext)
        {
            var result = new ShopifyThemeWorkContext();
            result.CurrentPage = 1;
            result.PageDescription = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.MetaDescription : String.Empty;
            result.PageTitle = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.Title : String.Empty;
            result.Shop = workContext.CurrentStore.ToShopifyModel(workContext);
            result.Cart = workContext.CurrentCart.ToShopifyModel(workContext);
            result.Product = workContext.CurrentProduct != null ? workContext.CurrentProduct.ToShopifyModel(workContext) : null;
            result.Customer = workContext.CurrentCustomer.HasAccount ? workContext.CurrentCustomer.ToShopifyModel(workContext) : null;
            result.AllStores = workContext.AllStores.Select(x => x.ToShopifyModel(workContext)).ToArray();

            result.CurrentCurrency = workContext.CurrentCurrency.ToShopifyModel();
            result.CurrentLanguage = workContext.CurrentLanguage.ToShopifyModel();

            if (workContext.CurrentCatalogSearchResult != null)
            {
                result.Collection = workContext.CurrentCatalogSearchResult.ToShopifyModel(workContext);
                if (workContext.CurrentCatalogSearchResult.Categories != null)
                {
                    result.Collections = new Collections(workContext.CurrentCatalogSearchResult.Categories.Select(x => x.ToShopifyModel(workContext)));
                }
            }
            return result;
        }
    }
}
