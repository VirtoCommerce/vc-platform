using System;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CollectionConverter
    {
        public static Search ToShopifyModel(this storefrontModel.Catalog.CatalogSearchResult searchResult, storefrontModel.WorkContext workContext)
        {
            var result = new Search();

            /*
            if (searchResult.Category != null)
            {
                result = searchResult.Category.ToShopifyModel(workContext);
            }

            if (searchResult.Products != null)
            {
                result.Products = new StorefrontPagedList<Product>(searchResult.Products.Select(x => x.ToShopifyModel(workContext)), searchResult.Products, searchResult.Products.GetPageUrl);
                result.ProductsCount = searchResult.Products.TotalItemCount;
                result.AllProductsCount = searchResult.Products.TotalItemCount;
            }

            if (searchResult.Aggregations != null)
            {
                var tags = searchResult.Aggregations
                    .Where(a => a.Items != null)
                    .SelectMany(a => a.Items.Select(item => item.ToShopifyModel(a.Field, a.Label)))
                    .ToList();

                result.Tags = new TagCollection(tags);
            }

            result.DefaultSortBy = "manual";
            if (workContext.CurrentCatalogSearchCriteria != null)
            {
                result.SortBy = workContext.CurrentCatalogSearchCriteria.SortBy;
            }
            */

            return result;
        }

        public static Collection ToShopifyModel(this storefrontModel.Catalog.Category category, storefrontModel.WorkContext workContext)
        {
            var result = new Collection
            {
                Id = category.Id,
                Description = null,
                Handle = category.SeoInfo != null ? category.SeoInfo.Slug : category.Id,
                Title = category.Name,
                Url = "~/category/" + category.Id
            };
            if(category.PrimaryImage != null)
            {
                result.Image = category.PrimaryImage.ToShopifyModel();
            }

            if (category.SeoInfo != null)
            {
                result.Url = "~/" + category.SeoInfo.Slug;
            }

            return result;
        }
    }
}

