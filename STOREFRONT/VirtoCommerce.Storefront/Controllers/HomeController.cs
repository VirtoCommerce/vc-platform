using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "HomeCachingProfile")]
    public class HomeController : Controller
    {
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly WorkContext _workContext;
        public HomeController(WorkContext context, ICatalogSearchService catalogSearchService)
        {
            _catalogSearchService = catalogSearchService;
            _workContext = context;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            //Load categories for main page (may be removed if it not necessary)
            var catalogSearchCriteria = new CatalogSearchCriteria
            {
                CatalogId = _workContext.CurrentStore.Catalog,
                ResponseGroup = CatalogSearchResponseGroup.WithCategories
            };
            _workContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(catalogSearchCriteria);
            return View("index", _workContext);
        }
             
    }
}
