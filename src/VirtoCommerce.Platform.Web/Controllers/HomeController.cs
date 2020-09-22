using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
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
        private readonly IWebHostEnvironment _hostEnv;
        private readonly LicenseProvider _licenseProvider;

        public HomeController(IOptions<PlatformOptions> platformOptions, IOptions<WebAnalyticsOptions> webAnalyticsOptions, IWebHostEnvironment hostEnv, LicenseProvider licenseProvider)
        {
            _platformOptions = platformOptions.Value;
            _webAnalyticsOptions = webAnalyticsOptions.Value;
            _hostEnv = hostEnv;
            _licenseProvider = licenseProvider;
        }

        public ActionResult Index()
        {
            var model = new IndexModel
            {
                PlatformVersion = new HtmlString(Core.Common.PlatformVersion.CurrentVersion.ToString()),
                DemoCredentials = new HtmlString(_platformOptions.DemoCredentials ?? "''"),
                DemoResetTime = new HtmlString(_platformOptions.DemoResetTime ?? "''"),
                WebAnalyticsOptions = _webAnalyticsOptions
            };

            var license = _licenseProvider.GetLicense();

            if (license != null)
            {
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
