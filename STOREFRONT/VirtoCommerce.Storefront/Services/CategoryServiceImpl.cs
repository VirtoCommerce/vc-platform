using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Services
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;

        public CategoryServiceImpl(ICatalogModuleApi catalogModuleApi)
        {
            _catalogModuleApi = catalogModuleApi;
        }

        public async Task<Category[]> GetCategoriesByCatalog(string catalogId)
        {
            var result = await _catalogModuleApi.CatalogModuleSearchSearchAsync("WithCategories", null, null, null, null, null, null, null, null, null, null, null, null, null, 0, 100, null);

            return result.Categories.Select(c => c.ToWebModel()).ToArray();
        }

        public async Task<Category> GetCategoryById(string id, string catalogId, string language, string currencyCode)
        {
            var category = await _catalogModuleApi.CatalogModuleCategoriesGetAsync(id);

            var result = await _catalogModuleApi.CatalogModuleSearchSearchAsync("WithCategories", null, null, null, null, null, null, null, null, null, null, null, null, null, 0, 100, null);

            return category.ToWebModel(result.Products.ToArray());
        }
    }
}