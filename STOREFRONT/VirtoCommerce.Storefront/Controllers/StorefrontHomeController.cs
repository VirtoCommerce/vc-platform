using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "HomeCachingProfile")]
    public class StorefrontHomeController : Controller
    {
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly WorkContext _workContext;
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly ISearchModuleApi _searchApi;

        public StorefrontHomeController(WorkContext context, ICatalogSearchService catalogSearchService, ICatalogModuleApi catalogModuleApi, ISearchModuleApi searchApi)
        {
            _catalogSearchService = catalogSearchService;
            _workContext = context;
            _catalogModuleApi = catalogModuleApi;
            _searchApi = searchApi;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //Load categories for main page (may be removed if it not necessary)
            //var catalogSearchCriteria = new CatalogSearchCriteria()
            //{
            //    CatalogId = _workContext.CurrentStore.Catalog,
            //    ResponseGroup = CatalogSearchResponseGroup.WithCategories,
            //    SortBy = "Priority",
            //    SearchInChildren = false
            //};
        
           // _workContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(catalogSearchCriteria);
            return View("index", _workContext);
        }

       
    }
}
