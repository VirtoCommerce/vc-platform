using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Licensing;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePlatformSettings(this IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(AllSettings, "Platform");
            settingsRegistrar.RegisterSettingsForType(UserProfile.AllSettings, nameof(UserProfile));

            var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();

            var sendDiagnosticData = settingsManager.GetValue<bool>(Setup.SendDiagnosticData);
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

        /// <summary>
        /// Register settings declared in each installed module's module.manifest, settings element. 
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSettingsFromModuleManifests(this IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            var moduleService = appBuilder.ApplicationServices.GetRequiredService<Core.Modularity.IModuleService>();
            foreach (var module in moduleService.GetInstalledModules())
            {
                if (module.Settings.Count > 0)
                {
                    settingsRegistrar.RegisterSettings(module.Settings, module.Id);
                }
            }

            return appBuilder;
        }

        /// <summary>
        /// Run specified payload in sync between several instances
        /// </summary>
        /// <param name="app"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static IApplicationBuilder ExecuteSynchronized(this IApplicationBuilder app, Action payload)
        {
            var distributedLockProvider = app.ApplicationServices.GetRequiredService<IInternalDistributedLockService>();
            distributedLockProvider.ExecuteSynchronized(nameof(Startup), _ => payload());
            return app;
        }

        public static IApplicationBuilder UseModulesAndAppsFiles(this IApplicationBuilder app)
        {
            var localModules = ModuleBootstrapper.Instance.GetInstalledModules();

            foreach (var module in localModules)
            {
                // Step 1. Enables static file serving for module
                // Disable FileSystemWatcher to prevent directory handle lock (allows module update/uninstall at runtime)
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(module.FullPhysicalPath) { UsePollingFileWatcher = true, UseActivePolling = false },
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
                        FileProvider = new PhysicalFileProvider(appPath) { UsePollingFileWatcher = true, UseActivePolling = false },
                        RequestPath = new PathString($"/apps/{moduleApp.Id}")
                    });

                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(appPath) { UsePollingFileWatcher = true, UseActivePolling = false },
                        RequestPath = new PathString($"/apps/{moduleApp.Id}")
                    });
                }
            }

            return app;
        }
    }

}
