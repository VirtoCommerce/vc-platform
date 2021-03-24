using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Home;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlatformOptions _platformOptions;
        private readonly PushNotificationOptions _pushNotificationOptions;
        private readonly WebAnalyticsOptions _webAnalyticsOptions;
        private readonly LocalStorageModuleCatalogOptions _localStorageModuleCatalogOptions;
        private readonly LicenseProvider _licenseProvider;
        private readonly ISettingsManager _settingsManager;

        public HomeController(IOptions<PlatformOptions> platformOptions, IOptions<WebAnalyticsOptions> webAnalyticsOptions, IOptions<LocalStorageModuleCatalogOptions> localStorageModuleCatalogOptions, IOptions<PushNotificationOptions> pushNotificationOptions, LicenseProvider licenseProvider, ISettingsManager settingsManager)
        {
            _platformOptions = platformOptions.Value;
            _webAnalyticsOptions = webAnalyticsOptions.Value;
            _localStorageModuleCatalogOptions = localStorageModuleCatalogOptions.Value;
            _pushNotificationOptions = pushNotificationOptions.Value;
            _licenseProvider = licenseProvider;
            _settingsManager = settingsManager;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexModel
            {
                PlatformVersion = new HtmlString(Core.Common.PlatformVersion.CurrentVersion.ToString()),
                DemoCredentials = new HtmlString(_platformOptions.DemoCredentials ?? "''"),
                DemoResetTime = new HtmlString(_platformOptions.DemoResetTime ?? "''"),
                WebAnalyticsOptions = _webAnalyticsOptions,
                RefreshProbingFolder = _localStorageModuleCatalogOptions.RefreshProbingFolderOnStart,
                ForceWebSockets = _pushNotificationOptions.ForceWebSockets
            };

            var license = await _licenseProvider.GetLicenseAsync();

            if (license != null)
            {
                model.SendDiagnosticData = license.ExpirationDate < DateTime.UtcNow || await _settingsManager.GetValueAsync(Setup.SendDiagnosticData.Name, (bool)Setup.SendDiagnosticData.DefaultValue);
                model.License = new HtmlString(JsonConvert.SerializeObject(license, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                }).Replace("\"", "'"));
            }

            if (!string.IsNullOrEmpty(model.DemoResetTime.Value))
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(model.DemoResetTime.Value, out timeSpan))
                {
                    var now = DateTime.UtcNow;
                    var resetTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, DateTimeKind.Utc);

                    if (resetTime < now)
                    {
                        resetTime = resetTime.AddDays(1);
                    }

                    model.DemoResetTime = new HtmlString(JsonConvert.SerializeObject(resetTime).Replace("\"", "'") ?? "''");
                }
            }

            return View(model);
        }

        public ActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
