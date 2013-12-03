using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Web.Client.Modules;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VirtoCommerce.Web.ModuleConfig), "Start")]

namespace VirtoCommerce.Web
{
    
    public static class ModuleConfig
    {
        /// <summary>
        /// Loads asp.net modules.
        /// </summary>
        public static void Start()
        {
            if (ConnectionHelper.IsDatabaseInstalled)
            {
                DynamicModuleUtility.RegisterModule(typeof(StoreHttpModule));
                DynamicModuleUtility.RegisterModule(typeof(MarketingHttpModule));
            }
        }
    }
}