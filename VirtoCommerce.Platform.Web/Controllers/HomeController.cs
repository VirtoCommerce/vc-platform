using System;
using System.Reflection;
using System.Web.Hosting;
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
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = string.Join(".", assembly.GetInformationalVersion(), assembly.GetFileVersion());
            var demoCredentials = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:DemoCredentials");
            var resetTimeStr = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:DemoResetTime");
            var license = LoadLicense();
            var licenseString = JsonConvert.SerializeObject(license, new JsonSerializerSettings
            {
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

        private static License LoadLicense()
        {
            License license = null;

            var licenseFilePath = HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data/VirtoCommerce.lic");
            if (System.IO.File.Exists(licenseFilePath))
            {
                var rawLicense = System.IO.File.ReadAllText(licenseFilePath);
                license = License.Parse(rawLicense);

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }

            return license;
        }
    }
}
