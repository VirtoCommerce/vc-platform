using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "CatalogSearchCachingProfile")]
    public class CatalogSearchController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _searchService;
        private readonly IMarketingModuleApi _marketingApi;

        public CatalogSearchController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService searchService,
            IMarketingModuleApi marketingApi)
            : base(workContext, urlBuilder)
        {
            _searchService = searchService;
            _marketingApi = marketingApi;
        }

        /// GET search
        /// This method used for search products by given criteria 
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchProducts()
        {
            //All resulting categories, products and aggregations will be lazy evaluated when view will be rendered. (workContext.Products, workContext.Categories etc) 
            //All data will loaded using by current search criteria taken from query string
           return View("search", WorkContext);
        }

        /// <summary>
        /// GET search/{categoryId}?view=...
        /// This method called from SeoRoute when url contains slug for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CategoryBrowsing(string categoryId, string view)
        {
            if (string.IsNullOrEmpty(view))
            {
                view = "grid";
            }
            WorkContext.CurrentCatalogSearchCriteria.CategoryId = categoryId;
            WorkContext.CurrentCategory = (await _searchService.GetCategoriesAsync(new[] { categoryId }, CategoryResponseGroup.Full)).FirstOrDefault();
            WorkContext.CurrentCategory.Products = WorkContext.Products;

            if (view.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                return View("collection.list", WorkContext);
            }

            return View("collection", WorkContext);
        }
    }
}