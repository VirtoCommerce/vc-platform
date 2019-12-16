using System;
using System.Reflection;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISettingsManager _settingsManager;

        public HomeController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = PlatformVersion.CurrentVersion.ToString();
            var demoCredentials = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:DemoCredentials");
            var resetTimeStr = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:DemoResetTime");
            var license = LicenseProvider.LoadLicense();
            var licenseString = JsonConvert.SerializeObject(license, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            if (!string.IsNullOrEmpty(resetTimeStr))
            {
                if (TimeSpan.TryParse(resetTimeStr, out var timeSpan))
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
                FavIcon = new MvcHtmlString(GetFavIcon() ?? "favicon.ico"),
            });
        }


        private string GetFavIcon()
        {
            string result = null;
            var uiSettings = _settingsManager.GetSettingByName("VirtoCommerce.Platform.UI.Customization");
            if (uiSettings != null)
            {
                try
                {
                    var jObject = JObject.Parse(uiSettings.Value);
                    result = (string)jObject?.SelectToken("favicon", false);
                }
                catch (JsonReaderException)
                {
                    result = null;
                }
            }
            return result;
        }
    }
}
