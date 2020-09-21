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
    }
}
