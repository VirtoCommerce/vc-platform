using VirtoCommerce.Storefront.Converters;
using System.Configuration;
using System.Web.Http;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Common;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    public abstract class ApiControllerBase : ApiController
    {
#pragma warning disable CS3008 // Identifier is not CLS-compliant
        protected readonly ICatalogSearchService _catalogService;
        protected readonly ICatalogSearchService _productService;
        protected readonly IStoreModuleApi _storeApi;
#pragma warning restore CS3008 // Identifier is not CLS-compliant

        protected WorkContext WorkContext { get; private set; }

        public ApiControllerBase(WorkContext workContext, ICatalogSearchService catalogSearchService, ICatalogSearchService productService, IStoreModuleApi storeApi)
        {
            WorkContext = workContext;
            _catalogService = catalogSearchService;
            _productService = productService;
            _storeApi = storeApi;

            InitializeWorkContext();
        }


        protected void InitializeWorkContext()
        {
            if (WorkContext.CurrentStore == null)
            {
                WorkContext.CurrentStore = _storeApi.StoreModuleGetStoreById(ConfigurationManager.AppSettings["DefaultStore"]).ToWebModel();
                WorkContext.CurrentCurrency = WorkContext.CurrentStore.DefaultCurrency;
                WorkContext.CurrentLanguage = WorkContext.CurrentStore.DefaultLanguage;
            }
            if (WorkContext.CurrentCustomer == null)
            {
                WorkContext.CurrentCustomer = new Customer
                {
                    Id = "anonymous-customer-id",
                    UserName = StorefrontConstants.AnonymousUsername,
                    Name = StorefrontConstants.AnonymousUsername
                };
            }
        }
    }
}