using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.OwinMiddlewares
{
    /// <summary>
    /// Populate main work context by required commerce data as store, user profile, cart etc.
    /// </summary>
    public class ContextInitializationMiddleware : OwinMiddleware
    {
        private readonly WorkContext _workContext;
        private readonly IStoreModuleApi _storeApi;
        private readonly IVirtoCommercePlatformApi _platformApi;

        public ContextInitializationMiddleware(OwinMiddleware next, WorkContext workContext, IStoreModuleApi storeApi, IVirtoCommercePlatformApi platformApi)
            : base(next)
        {
            _workContext = workContext;
            _storeApi = storeApi;
            _platformApi = platformApi;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = context as WorkContext;
            if (context != null)
            {
                workContext.AllStores = _storeApi.StoreModuleGetStores().Select(x => x.ToWebModel()).ToList();
            }

            await Next.Invoke(context);
        }
    }
}