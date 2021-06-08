using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
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
            settingsRegistrar.RegisterSettingsForType(UserProfile.AllSettings, typeof(UserProfile).Name);

            var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();

            var sendDiagnosticData = settingsManager.GetValue(Setup.SendDiagnosticData.Name, (bool)Setup.SendDiagnosticData.DefaultValue);
            if (!sendDiagnosticData)
            {
                var licenseProvider = appBuilder.ApplicationServices.GetRequiredService<LicenseProvider>();
                var license = licenseProvider.GetLicenseAsync().GetAwaiter().GetResult();

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
        public static IApplicationBuilder WithDistributedLock(this IApplicationBuilder app, Action payload)
        {
            var redisConnMultiplexer = app.ApplicationServices.GetService<IConnectionMultiplexer>();

            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();

            if (redisConnMultiplexer != null)
            {
                var distributedLockWait = app.ApplicationServices.GetRequiredService<IOptions<LocalStorageModuleCatalogOptions>>().Value.DistributedLockWait;

                // Try to acquire distributed lock
                using (var redlockFactory = RedLockFactory.Create(new RedLockMultiplexer[] { new RedLockMultiplexer(redisConnMultiplexer) }))
                using (var redLock = redlockFactory.CreateLock(nameof(WithDistributedLock),
                    TimeSpan.FromSeconds(120 + distributedLockWait) /* Successfully acquired lock expiration time */,
                    TimeSpan.FromSeconds(distributedLockWait) /* Total time to wait until the lock is available */,
                    TimeSpan.FromSeconds(3) /* The span to acquire the lock in retries */))
                {
                    if (redLock.IsAcquired)
                    {
                        logger.LogInformation("Distributed lock acquired");
                        payload();
                    }
                    else
                    {
                        // Lock not acquired even after migrationDistributedLockOptions.Wait
                        throw new PlatformException($"Can't apply migrations. It seems another platform instance still applies migrations. Consider to increase MigrationDistributedLockOptions.Wait timeout.");
                    }
                }
            }
            else
            {
                // One-instance configuration, no Redis, just run
                logger.Log(LogLevel.Information, "Distributed lock not acquired, Redis ConnectionMultiplexer is null (No Redis connection ?)");
                payload();
            }

            return app;
        }
    }

}
