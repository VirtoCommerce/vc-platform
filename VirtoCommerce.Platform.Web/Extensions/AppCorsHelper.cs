using System.Web.Http;
using System.Web.Http.Cors;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class AppCorsHelper
    {
        public static void SetupCors(this HttpConfiguration config)
        {
            var origins = ConfigurationHelper.GetAppSettingsValue("cors:origins");
            var headers = ConfigurationHelper.GetAppSettingsValue("cors:headers");
            var methods = ConfigurationHelper.GetAppSettingsValue("cors:methods");
            var credentials = ConfigurationHelper.GetAppSettingsValue("cors:credentials", false);
            config.EnableCors(new EnableCorsAttribute(origins, headers, methods) { SupportsCredentials = credentials});
        }
    }
}
