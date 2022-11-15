using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.Web.Licensing;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static readonly Dictionary<string, StringValues> CustomHeaders = new()
        {
            { "X-Frame-Options", new StringValues("SAMEORIGIN") }
        };
        
        public static IApplicationBuilder UsePlatformSettings(this IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(AllSettings, "Platform");
            settingsRegistrar.RegisterSettingsForType(UserProfile.AllSettings, typeof(UserProfile).Name);

            var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();

            var sendDiagnosticData = settingsManager.GetValue(Setup.SendDiagnosticData.Name, (bool)Setup.SendDiagnosticData.DefaultValue);
            if (!sendDiagnosticData)
            {
                var licenseProvider = appBuilder.ApplicationServices.GetRequiredService<LicenseProvider>();
                var license = licenseProvider.GetLicense();

                if (license == null || license.ExpirationDate < DateTime.UtcNow)
                {
                    settingsManager.SetValue(Setup.SendDiagnosticData.Name, true);
                }
            }

            return appBuilder;
        }

        public static IApplicationBuilder UseModules(this IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var moduleManager = serviceScope.ServiceProvider.GetRequiredService<IModuleManager>();
                var modules = GetInstalledModules(serviceScope.ServiceProvider);
                foreach (var module in modules)
                {
                    moduleManager.PostInitializeModule(module, appBuilder);
                }
            }
            return appBuilder;
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                context.Response.Headers.AddRange(CustomHeaders);
                await next();
            });

            return appBuilder;
        }

        private static IEnumerable<ManifestModuleInfo> GetInstalledModules(IServiceProvider serviceProvider)
        {
            var moduleCatalog = serviceProvider.GetRequiredService<ILocalModuleCatalog>();
            var allModules = moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                          .Where(x => x.State == ModuleState.Initialized && !x.Errors.Any())
                                          .ToArray();

            return moduleCatalog.CompleteListWithDependencies(allModules)
                .OfType<ManifestModuleInfo>()
                .Where(x => x.State == ModuleState.Initialized && !x.Errors.Any())
                .ToArray();
        }

        /// <summary>
        /// Run specified payload in sync between several instances
        /// </summary>
        /// <param name="app"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static IApplicationBuilder ExecuteSynchronized(this IApplicationBuilder app, Action payload)
        {
            var distributedLockProvider = app.ApplicationServices.GetRequiredService<IDistributedLockProvider>();
            distributedLockProvider.ExecuteSynchronized(nameof(Startup), (x) => payload());
            return app;
        }

        public static IApplicationBuilder UseModulesAndAppsFiles(this IApplicationBuilder app)
        {
            var localModules = app.ApplicationServices.GetRequiredService<ILocalModuleCatalog>().Modules;

            foreach (var module in localModules.OfType<ManifestModuleInfo>())
            {
                // Step 1. Enables static file serving for module
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(module.FullPhysicalPath),
                    RequestPath = new PathString($"/modules/$({module.ModuleName})")
                });

                // Step 2. Enables static file serving for apps
                foreach (var moduleApp in module.Apps)
                {
                    var appPath = string.IsNullOrEmpty(moduleApp.ContentPath) ?
                        Path.Combine(module.FullPhysicalPath, "Content", moduleApp.Id) :
                        Path.Combine(module.FullPhysicalPath, moduleApp.ContentPath);

                    if (!Directory.Exists(appPath))
                    {
                        throw new ModuleInitializeException($"The '{appPath}' directory doesn't exist for {module.ModuleName}");
                    }

                    app.UseDefaultFiles(new DefaultFilesOptions()
                    {
                        FileProvider = new PhysicalFileProvider(appPath),
                        RequestPath = new PathString($"/apps/{moduleApp.Id}")
                    });

                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(appPath),
                        RequestPath = new PathString($"/apps/{moduleApp.Id}")
                    });
                }
            }

            return app;
        }
    }

}
