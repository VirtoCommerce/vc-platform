using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = string.Format("{0}.{1}", assembly.GetInformationalVersion(), assembly.GetFileVersion());
            var demoCredentials = ConfigurationManager.AppSettings.GetValue<string>("VirtoCommerce:DemoCredentials", null);
            var license = "{type:'Community deployment', name:'john doe company', expirationDate:1488102833811, hasExpired: true}";
            var resetTimeStr = ConfigurationManager.AppSettings.GetValue<string>("VirtoCommerce:DemoResetTime", null);

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

                    resetTimeStr = Newtonsoft.Json.JsonConvert.SerializeObject(resetTime).Replace("\"", "'");
                }
            }

            return View(new PlatformSetting
            {
                PlatformVersion = new MvcHtmlString(version),
                DemoCredentials = new MvcHtmlString(demoCredentials ?? "''"),
                License = new MvcHtmlString(license ?? "''"),
                DemoResetTime = new MvcHtmlString(resetTimeStr ?? "''")
            });
        }
    }
}
