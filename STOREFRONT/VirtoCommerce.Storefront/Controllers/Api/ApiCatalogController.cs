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

        // GET: storefrontapi/catalog/search
        [HttpPost]
        public async Task<ActionResult> SearchProducts(CatalogSearchCriteria searchCriteria)
        {
            var retVal = await _catalogSearchService.SearchAsync(searchCriteria);
            return Json(retVal);
        }

        // GET: storefrontapi/products?productIds=...&respGroup=...
        [HttpGet]
        public async Task<ActionResult> GetProductsByIds(string[] productIds, ItemResponseGroup respGroup = ItemResponseGroup.ItemLarge)
        {
            var retVal = await _catalogSearchService.GetProductsAsync(productIds, respGroup);
            return Json(retVal);
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