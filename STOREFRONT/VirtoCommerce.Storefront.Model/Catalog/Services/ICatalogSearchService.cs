using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface ICatalogSearchService
    {
        Task<Product[]> GetProductsAsync(string[] ids, ItemResponseGroup responseGroup);
        Task<Category[]> GetCategoriesAsync(string[] ids, CategoryResponseGroup responseGroup);

        IPagedList<Product> SearchProducts(CatalogSearchCriteria criteria);
        IPagedList<Category> SearchCategories(CatalogSearchCriteria criteria);
        IPagedList<Aggregation> GetAggregations(CatalogSearchCriteria criteria);
    }
}
