using VirtoCommerce.Platform.Web.Infrastructure;

namespace VirtoCommerce.Platform.Web.Model.Home
{
    public class IndexModel
    {
        public WebAnalyticsOptions WebAnalyticsOptions { get; set; }
        public string PlatformVersion { get; set; }
        public string DemoCredentials { get; set; }
        public string DemoResetTime { get; set; }
        public string License { get; set; }
    }
}
