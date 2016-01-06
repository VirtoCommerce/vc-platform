using VirtoCommerce.Storefront.Converters;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Common;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [RoutePrefix("api/search")]
    public class ApiSearchController : ApiController
    {
        private readonly WorkContext _workContext;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ICatalogSearchService _productService;
        private readonly IStoreModuleApi _storeApi;
        public ApiSearchController(WorkContext workContext, ICatalogSearchService catalogSearchService, ICatalogSearchService productService, IStoreModuleApi storeApi)
        {
            _workContext = workContext;
            _catalogSearchService = catalogSearchService;
            _productService = productService;
            _storeApi = storeApi;
        }

        //Load categories for main page
        [Route("categories")]
        public async Task<CatalogSearchResult> GetCurrentCategories()
        {
            InitializeWorkContext();

            _workContext.CurrentCatalogSearchCriteria = new CatalogSearchCriteria
            {
                CatalogId = _workContext.CurrentStore.Catalog,
                ResponseGroup = CatalogSearchResponseGroup.WithCategories
            };

            _workContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(_workContext.CurrentCatalogSearchCriteria);
            return _workContext.CurrentCatalogSearchResult;
        }

        //Load products for category
        [Route("products")]
        public async Task<CatalogSearchResult> GetCategoryProducts([FromUri] ApiWorkContext apiWorkContext)
        {
            InitializeWorkContext();

            _workContext.CurrentCatalogSearchCriteria = new CatalogSearchCriteria
            {
                CatalogId = apiWorkContext.CatalogId,
                CategoryId = apiWorkContext.CategoryId
            };

            _workContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(_workContext.CurrentCatalogSearchCriteria);
            return _workContext.CurrentCatalogSearchResult;
        }

        //Load product details
        [Route("products/{itemId}")]
        public async Task<Product> GetProduct([FromUri] ApiWorkContext apiWorkContext)
        {
            InitializeWorkContext();
            _workContext.CurrentProduct = await _productService.GetProductAsync(apiWorkContext.ItemId, Model.Catalog.ItemResponseGroup.ItemLarge);
            return _workContext.CurrentProduct;
        }
        

        private void InitializeWorkContext()
        {
            if (_workContext.CurrentStore == null)
            {
                _workContext.CurrentStore = _storeApi.StoreModuleGetStoreById(ConfigurationManager.AppSettings["DefaultStore"]).ToWebModel();
                _workContext.CurrentCurrency = _workContext.CurrentStore.DefaultCurrency;
                _workContext.CurrentLanguage = _workContext.CurrentStore.DefaultLanguage;
            }
            if (_workContext.CurrentCustomer == null)
            {
                _workContext.CurrentCustomer = new Customer
                {
                    Id = "anonymous-customer-id",
                    UserName = StorefrontConstants.AnonymousUsername,
                    Name = StorefrontConstants.AnonymousUsername
                };
            }
        }
    }
}