using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static MvcHtmlString _version;
        /// <summary>
        /// Versions the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString Version(this HtmlHelper html)
        {
            if (_version == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                _version = new MvcHtmlString(String.Format("{0}.{1}", assembly.GetInformationalVersion(), assembly.GetFileVersion()));
            }

            return _version;
        }

        private static MvcHtmlString _demoCredentials;
        /// <summary>
        /// Demo credentials displayed on manager login dialog
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString DemoCredentials(this HtmlHelper html)
        {

            if (_demoCredentials == null)
            {
                _demoCredentials = MvcHtmlString.Create(ConfigurationManager.AppSettings.GetValue("VirtoCommerce:DemoCredentials", string.Empty));
            }

            return _demoCredentials;
        }

        private static MvcHtmlString _resetTime;
        public static MvcHtmlString DemoResetTime(this HtmlHelper html)
        {

            if (_resetTime == null)
            {
                var resetTimeStr = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:DemoResetTime", string.Empty);
                if(!string.IsNullOrEmpty(resetTimeStr))
                {
                    TimeSpan timeSpan;
                    if(TimeSpan.TryParse(resetTimeStr, out timeSpan))
                    {
                        var now = DateTime.UtcNow;
                        var resetTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                        if(resetTime < now)
                        {
                            resetTime = resetTime.AddDays(1);
                        }
                        _resetTime = MvcHtmlString.Create(Newtonsoft.Json.JsonConvert.SerializeObject(resetTime)); 
                    }
                }
            }

            return _resetTime;
        }
    }
}