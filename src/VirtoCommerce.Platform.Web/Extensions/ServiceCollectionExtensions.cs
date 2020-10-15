using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.External;
using VirtoCommerce.Platform.Web.Infrastructure;

namespace VirtoCommerce.Platform.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services, IMvcBuilder mvcBuilder, Action<LocalStorageModuleCatalogOptions> setupAction = null)
        {
            services.AddSingleton(services);

            services.AddSingleton<IModuleInitializer, ModuleInitializer>();
            // Cannot inject IHostingEnvironment to LoadContextAssemblyResolver as IsDevelopment() is an extension method (means static) and cannot be mocked by Moq in tests
            services.AddSingleton<IAssemblyResolver, LoadContextAssemblyResolver>(provider =>
                new LoadContextAssemblyResolver(provider.GetService<ILogger<LoadContextAssemblyResolver>>(), provider.GetService<IWebHostEnvironment>().IsDevelopment()));
            services.AddSingleton<IModuleManager, ModuleManager>();
            services.AddSingleton<ILocalModuleCatalog, LocalStorageModuleCatalog>();
            services.AddSingleton<IModuleCatalog>(provider => provider.GetService<ILocalModuleCatalog>());

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            var providerSnapshot = services.BuildServiceProvider();

            var manager = providerSnapshot.GetRequiredService<IModuleManager>();
            var moduleCatalog = providerSnapshot.GetRequiredService<ILocalModuleCatalog>();

            manager.Run();
            // Ensure all modules are loaded
            foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.State == ModuleState.NotStarted).ToArray())
            {
                manager.LoadModule(module.ModuleName);

                // VP-2190: No need to add parts for modules with laoding errors - it could cause an exception
                if (module.Assembly != null && module.Errors.IsNullOrEmpty())
                {
                    // Register API controller from modules
                    mvcBuilder.AddApplicationPart(module.Assembly);
                }
            }

            services.AddSingleton(moduleCatalog);
            return services;
        }

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
