using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("search")]
    public class CatalogSearchController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _searchService;

        public CatalogSearchController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService searchService)
            : base(workContext, urlBuilder)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// This method called from SeoRoute when url contains slug for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CategoryBrowsing(string categoryId)
        {
            WorkContext.CurrentCatalogSearchCriteria.CategoryId = categoryId;
            WorkContext.CurrentCatalogSearchResult = await _searchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);
            return View("collection", WorkContext);
        }
    }
}
