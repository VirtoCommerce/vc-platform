using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Home;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlatformOptions _platformOptions;
        private readonly WebAnalyticsOptions _webAnalyticsOptions;
        private readonly IHostingEnvironment _hostEnv;

        public HomeController(IOptions<PlatformOptions> platformOptions, IOptions<WebAnalyticsOptions> webAnalyticsOptions, IHostingEnvironment hostEnv)
        {
            _platformOptions = platformOptions.Value;
            _webAnalyticsOptions = webAnalyticsOptions.Value;
            _hostEnv = hostEnv;
        }

        public ActionResult Index()
        {
            var model = new IndexModel
            {
                PlatformVersion = Core.Common.PlatformVersion.CurrentVersion.ToString(),
                DemoCredentials = _platformOptions.DemoCredentials,
                DemoResetTime = _platformOptions.DemoResetTime,
                WebAnalyticsOptions = _webAnalyticsOptions
            };

            var license = LoadLicense();

            if (license != null)
            {
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

        public ActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private License LoadLicense()
        {
            License license = null;

            var licenseFilePath = Path.GetFullPath(_platformOptions.LicenseFilePath);
            if (System.IO.File.Exists(licenseFilePath))
            {
                var rawLicense = System.IO.File.ReadAllText(licenseFilePath);
                license = License.Parse(rawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }

            return license;
        }
    }
}
