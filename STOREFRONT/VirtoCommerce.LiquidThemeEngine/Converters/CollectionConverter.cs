using System.Linq;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CollectionConverter
    {

        public static Collection ToShopifyModel(this storefrontModel.Catalog.Category category, storefrontModel.WorkContext workContext)
        {
            var result = new Collection
            {
                Id = category.Id,
                Description = null,
                Handle = category.SeoInfo != null ? category.SeoInfo.Slug : category.Id,
                Title = category.Name,
                Url = category.Url,
                DefaultSortBy = "manual",
            };

            if (category.PrimaryImage != null)
            {
                result.Image = category.PrimaryImage.ToShopifyModel();
            }

            if (category.Products != null)
            {
                result.Products = new MutablePagedList<Product>((pageNumber, pageSize) =>
                {
                    category.Products.Slice(pageNumber, pageSize);
                    return new StaticPagedList<Product>(category.Products.Select(x => x.ToShopifyModel()), category.Products);
                }, category.Products.PageNumber, category.Products.PageSize);
            }

            if (workContext.Aggregations != null)
            {
                result.Tags = new TagCollection(new MutablePagedList<Tag>((pageNumber, pageSize) =>
                {
                    workContext.Aggregations.Slice(pageNumber, pageSize);
                    var tags = workContext.Aggregations.Where(a => a.Items != null)
                                           .SelectMany(a => a.Items.Select(item => item.ToShopifyModel(a.Field, a.Label)));
                    return new StaticPagedList<Tag>(tags, workContext.Aggregations);

                }, workContext.Aggregations.PageNumber, workContext.Aggregations.PageSize));
            }

            if (workContext.CurrentCatalogSearchCriteria.SortBy != null)
            {
                result.SortBy = workContext.CurrentCatalogSearchCriteria.SortBy;
            }

            if (!category.Properties.IsNullOrEmpty())
            {
                result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("properties", category.Properties) });
            }

            return result;
        }
    }
}
