using Microsoft.AspNetCore.Html;
using VirtoCommerce.Platform.Web.Infrastructure;

namespace VirtoCommerce.Platform.Web.Model.Home
{
    public class IndexModel
    {
        public WebAnalyticsOptions WebAnalyticsOptions { get; set; }
        public HtmlString PlatformVersion { get; set; }
        public HtmlString DemoCredentials { get; set; }
        public HtmlString DemoResetTime { get; set; }
        public HtmlString License { get; set; }
        public bool SendDiagnosticData { get; internal set; } = true;
        /// <summary>
        /// This allows knowing at the client if this option enabled or not at the backend.
        /// If the option set to false, then the module install feature should be disabled at the client.
        /// True by default.
        /// </summary>
        public bool RefreshProbingFolder { get; internal set; } = true;
    }
}
