using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CollectionConverter
    {
        public static Collection ToShopifyModel(this storefrontModel.Catalog.CatalogSearchResult searchResult, storefrontModel.WorkContext workContext)
        {
            var result = new Collection();
            if(searchResult.Category != null)
            {
                result = searchResult.Category.ToShopifyModel(workContext);
            }

            if (searchResult.Products != null)
            {
                result.Products = new StorefrontPagedList<Product>(searchResult.Products.Select(x => x.ToShopifyModel(workContext)),
                                                                   searchResult.Products, searchResult.Products.GetPageUrl);
                result.ProductsCount = searchResult.Products.TotalItemCount;
                result.AllProductsCount = searchResult.Products.TotalItemCount;
            }

            return result;
        }

        public static Collection ToShopifyModel(this storefrontModel.Catalog.Category category, storefrontModel.WorkContext workContext)
        {
            var result = new Collection();
            result.Description = category.Name;
            result.Handle = category.SeoInfo != null ? category.SeoInfo.Slug : category.Id;
            result.Image = category.PrimaryImage != null ? category.PrimaryImage.ToShopifyModel() : null;
            result.Title = category.Name;
            result.Url = "~/category/" + category.Id;
            if (category.SeoInfo != null)
            {
                result.Url = "~/" + category.SeoInfo.Slug;
            }
            return result;
        }

    }
}

