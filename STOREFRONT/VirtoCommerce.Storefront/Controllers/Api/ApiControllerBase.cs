using System.Web.Http;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    public abstract class ApiControllerBase : ApiController
    {
#pragma warning disable CS3008 // Identifier is not CLS-compliant
        protected readonly ICatalogSearchService _catalogSearchService;
#pragma warning restore CS3008 // Identifier is not CLS-compliant

        protected WorkContext WorkContext { get; private set; }

        public ApiControllerBase(WorkContext workContext, ICatalogSearchService catalogSearchService)
        {
            WorkContext = workContext;
            _catalogSearchService = catalogSearchService;            
        }
    }
}