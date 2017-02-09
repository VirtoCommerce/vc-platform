using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly LicenseService _licenseService = new LicenseService();

        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = string.Join(".", assembly.GetInformationalVersion(), assembly.GetFileVersion());
            var demoCredentials = ConfigurationManager.AppSettings.GetValue<string>("VirtoCommerce:DemoCredentials", null);
            var resetTimeStr = ConfigurationManager.AppSettings.GetValue<string>("VirtoCommerce:DemoResetTime", null);
            var license = _licenseService.LoadLicense();
            var licenseString = JsonConvert.SerializeObject(license, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            if (!string.IsNullOrEmpty(resetTimeStr))
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(resetTimeStr, out timeSpan))
                {
                    var now = DateTime.UtcNow;
                    var resetTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, DateTimeKind.Utc);

                    if (resetTime < now)
                    {
                        resetTime = resetTime.AddDays(1);
                    }

                    resetTimeStr = JsonConvert.SerializeObject(resetTime).Replace("\"", "'");
                }
            }

            return View(new PlatformSetting
            {
                PlatformVersion = new MvcHtmlString(version),
                DemoCredentials = new MvcHtmlString(demoCredentials ?? "''"),
                DemoResetTime = new MvcHtmlString(resetTimeStr ?? "''"),
                License = new MvcHtmlString(licenseString),
            });
        }
    }
}
