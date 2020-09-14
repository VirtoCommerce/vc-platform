using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core;
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
        private readonly WebAnalyticsOptions _webAnalyticsOptions;
        private readonly LicenseProvider _licenseProvider;
        private readonly ISettingsManager _settingsManager;

        public HomeController(IOptions<PlatformOptions> platformOptions, IOptions<WebAnalyticsOptions> webAnalyticsOptions, LicenseProvider licenseProvider, ISettingsManager settingsManager)
        {
            _platformOptions = platformOptions.Value;
            _webAnalyticsOptions = webAnalyticsOptions.Value;
            _licenseProvider = licenseProvider;
            _settingsManager = settingsManager;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexModel
            {
                PlatformVersion = Core.Common.PlatformVersion.CurrentVersion.ToString(),
                DemoCredentials = _platformOptions.DemoCredentials,
                DemoResetTime = _platformOptions.DemoResetTime,
                WebAnalyticsOptions = _webAnalyticsOptions
            };

            var license = _licenseProvider.GetLicense();

            if (license != null)
            {
                model.SendDiagnosticData = license.ExpirationDate < DateTime.UtcNow || await _settingsManager.GetValueAsync(Setup.SendDiagnosticData.Name, (bool)Setup.SendDiagnosticData.DefaultValue);

                model.License = JsonConvert.SerializeObject(license, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                }).Replace("\"", "'");
            }

            if (!string.IsNullOrEmpty(model.DemoResetTime))
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(model.DemoResetTime, out timeSpan))
                {
                    var now = DateTime.UtcNow;
                    var resetTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, DateTimeKind.Utc);

                    if (resetTime < now)
                    {
                        resetTime = resetTime.AddDays(1);
                    }

                    model.DemoResetTime = JsonConvert.SerializeObject(resetTime).Replace("\"", "'");
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
