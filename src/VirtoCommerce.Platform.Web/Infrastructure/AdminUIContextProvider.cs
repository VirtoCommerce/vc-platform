using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Licensing;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Web.Infrastructure;

public class AdminUIContextProvider(
    IOptions<PlatformOptions> platformOptions,
    IOptions<LocalStorageModuleCatalogOptions> localStorageModuleCatalogOptions,
    IOptions<PushNotificationOptions> pushNotificationOptions,
    IOptions<PlatformUIOptions> platformUIOptions,
    LicenseProvider licenseProvider,
    ISettingsManager settingsManager,
    IAdminUIAppService adminUIAppService)
    : IAdminUIContextProvider
{
    public async Task<AdminUIContext> GetContextAsync(CancellationToken cancellationToken = default)
    {
        var context = new AdminUIContext
        {
            PlatformVersion = PlatformVersion.CurrentVersion.ToString(),
            DemoCredentials = platformOptions.Value.DemoCredentials,
            DemoResetTime = GetNextDemoResetTime(platformOptions.Value.DemoResetTime),
            AllowInstallModules = localStorageModuleCatalogOptions.Value.RefreshProbingFolderOnStart,
            ForceWebSockets = pushNotificationOptions.Value.ForceWebSockets,
            EnvironmentBanner = platformUIOptions.Value.EnvironmentBanner ?? new EnvironmentBannerOptions(),
            AdminUIs = adminUIAppService.GetAdminUIs().ToList(),
        };

        var license = licenseProvider.GetLicense();

        if (license != null)
        {
            context.License = license;
            context.SendDiagnosticData = license.ExpirationDate < DateTime.UtcNow ||
                await settingsManager.GetValueAsync<bool>(Setup.SendDiagnosticData);
        }

        return context;
    }

    private static DateTime? GetNextDemoResetTime(string demoResetTime)
    {
        if (string.IsNullOrEmpty(demoResetTime) || !TimeSpan.TryParse(demoResetTime, out var timeSpan))
        {
            return null;
        }

        var now = DateTime.UtcNow;
        var resetTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, DateTimeKind.Utc);

        return resetTime < now ? resetTime.AddDays(1) : resetTime;
    }
}
