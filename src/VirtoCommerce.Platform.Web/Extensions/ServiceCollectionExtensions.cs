using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExternalModules(this IServiceCollection services, Action<ExternalModuleCatalogOptions> setupAction = null)
        {
#pragma warning disable VC0014 // Type or member is obsolete
            services.AddSingleton<IExternalModulesClient, ExternalModulesClient>();
            services.AddSingleton<IExternalModuleCatalog, ExternalModuleCatalog>();
            services.AddSingleton<IModuleInstaller, ModuleInstaller>();
#pragma warning restore VC0014 // Type or member is obsolete

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton<IPlatformRestarter, ProcessPlatformRestarter>();
            services.AddSingleton<IModuleManagementService, ModuleManagementService>();
            services.AddSingleton<IModuleService, ModuleService>();

            return services;
        }
    }
}
