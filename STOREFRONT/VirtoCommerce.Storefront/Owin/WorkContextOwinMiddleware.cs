using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Models;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Populate main work context by required commerce data such as store, user profile, cart etc.
    /// </summary>
    public class WorkContextOwinMiddleware : OwinMiddleware
    {
        private readonly IStoreModuleApi _storeApi;
        private readonly IVirtoCommercePlatformApi _platformApi;

        public WorkContextOwinMiddleware(OwinMiddleware next, IStoreModuleApi storeApi, IVirtoCommercePlatformApi platformApi)
            : base(next)
        {
            _storeApi = storeApi;
            _platformApi = platformApi;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = context.Get<WorkContext>();

            // Initialize common properties: stores, languages, user profile, cart

            workContext.AllStores = _storeApi.StoreModuleGetStores().Select(x => x.ToWebModel()).ToList();

            // Initialize request specific properties: current store, current language

            await Next.Invoke(context);
        }
    }
}
