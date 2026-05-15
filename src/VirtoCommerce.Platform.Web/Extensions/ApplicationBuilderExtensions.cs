using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using VirtoCommerce.Platform.Core.Common;
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
        /// Register settings declared in each installed module's
        /// <c>module.manifest</c> &lt;settings&gt; element. For each module:
        /// <list type="number">
        ///   <item>Every entry in <see cref="Core.Modularity.ManifestModuleInfo.Settings"/> is
        ///   registered globally via
        ///   <see cref="ISettingsRegistrar.RegisterSettings"/> — surfaces
        ///   under <c>/api/platform/settings/v2/global/*</c>.</item>
        ///   <item>Entries whose <see cref="SettingDescriptor.Tenant"/> is
        ///   non-empty (set by the manifest's
        ///   <c>&lt;setting tenant="…"&gt;</c> attribute, e.g.
        ///   <c>tenant="UserProfile"</c>) are also registered via
        ///   <see cref="ISettingsRegistrar.RegisterSettingsForType"/>
        ///   under that tenant-type name — for <c>UserProfile</c> this
        ///   surfaces them additionally under
        ///   <c>/api/platform/settings/v2/me/*</c>.</item>
        /// </list>
        /// Mirrors the platform's own dual-registration pattern in
        /// <see cref="UsePlatformSettings"/> for
        /// <c>PlatformConstants.Settings.UserProfile.AllSettings</c>.
        /// </summary>
        public static IApplicationBuilder UseSettingsFromModuleManifests(this IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            var moduleService = appBuilder.ApplicationServices.GetRequiredService<Core.Modularity.IModuleService>();
            foreach (var module in moduleService.GetInstalledModules())
            {
                if (module.Settings.IsNullOrEmpty())
                {
                    continue;
                }

                // 1. Register every manifest-declared setting globally.
                settingsRegistrar.RegisterSettings(module.Settings, module.Id);

                // 2. Register the tenant-scoped subset additionally
                var tenantSettingsGroups = module.Settings
                    .Where(x => !string.IsNullOrWhiteSpace(x.Tenant))
                    .GroupBy(x => x.Tenant);
                foreach (var tenantSettings in tenantSettingsGroups)
                {
                    settingsRegistrar.RegisterSettingsForType(tenantSettings.ToList(), tenantSettings.Key);
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
