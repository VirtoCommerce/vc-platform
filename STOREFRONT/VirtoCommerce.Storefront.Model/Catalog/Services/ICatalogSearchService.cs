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
    /// <summary>
    /// Represent abstraction to search in catalog api (products, categories etc)
    /// </summary>
    public interface ICatalogSearchService
    {
        Task<Product[]> GetProductsAsync(string[] ids, ItemResponseGroup responseGroup);
        Task<Category[]> GetCategoriesAsync(string[] ids, CategoryResponseGroup responseGroup);
        Task<CatalogSearchResult> SearchProductsAsync(CatalogSearchCriteria criteria);
        Task<IPagedList<Category>> SearchCategoriesAsync(CatalogSearchCriteria criteria);

        CatalogSearchResult SearchProducts(CatalogSearchCriteria criteria);
        IPagedList<Category> SearchCategories(CatalogSearchCriteria criteria);
    }
}
