using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.External;

#pragma warning disable VC0014 // Type is obsolete
namespace VirtoCommerce.Platform.Modules
{
    public static class ServiceCollectionExtensions
    {
        [Obsolete("Use ModuleDiscovery class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public static IServiceCollection AddExternalModules(this IServiceCollection services, Action<ExternalModuleCatalogOptions> setupAction = null)
        {
            services.AddSingleton<IExternalModulesClient, ExternalModulesClient>();
            services.AddSingleton<IExternalModuleCatalog, ExternalModuleCatalog>();
            services.AddSingleton<IModuleInstaller, ModuleInstaller>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton<IPlatformRestarter, ProcessPlatformRestarter>();

            return services;
        }
    }
}
