using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.Common;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiCatalogController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _catalogSearchService;
        public ApiCatalogController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService catalogSearchService)
            : base(workContext, urlBuilder)
        {
            _catalogSearchService = catalogSearchService;
        }

        // storefrontapi/catalog/search
        [HttpPost]
        public async Task<ActionResult> SearchProducts(CatalogSearchCriteria searchCriteria)
        {
            var retVal = await _catalogSearchService.SearchProductsAsync(searchCriteria);
            return Json(new
            {
                 Products = retVal.Products,
                 Aggregations = retVal.Aggregations,
                 MetaData = retVal.Products.GetMetaData()
            });
        }

        // storefrontapi/products?productIds=...&respGroup=...
        [HttpGet]
        public async Task<ActionResult> GetProductsByIds(string[] productIds, ItemResponseGroup respGroup = ItemResponseGroup.ItemLarge)
        {
            var retVal = await _catalogSearchService.GetProductsAsync(productIds, respGroup);
            return Json(retVal);
        }

        // storefrontapi/categories/search
        [HttpPost]
        public async Task<ActionResult> SearchCategories(CatalogSearchCriteria searchCriteria)
        {
            var retVal = await _catalogSearchService.SearchCategoriesAsync(searchCriteria);
            return Json(new
            {
                Categories = retVal,
                MetaData = retVal.GetMetaData()
            });
        }

        // GET: storefrontapi/categories
        [HttpGet]
        public async Task<ActionResult> GetCategoriesByIds(string[] categoryIds, CategoryResponseGroup respGroup = CategoryResponseGroup.Full)
        {
            var retVal = await _catalogSearchService.GetCategoriesAsync(categoryIds, respGroup);
            return Json(retVal);
        }
      
    }
}