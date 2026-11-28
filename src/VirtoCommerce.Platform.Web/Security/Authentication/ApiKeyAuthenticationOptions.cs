using Microsoft.AspNetCore.Authentication;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";

        public string Scheme { get; set; } = DefaultScheme;
        public string ApiKeyParamName { get; set; } = PlatformConstants.Security.ApiKeyAuthentication.ParamName;

        /// <summary>
        /// Whether the key is also accepted in the query string. A key in a URL ends up in access logs, proxy logs
        /// and browser history, so set it to false once all clients send the key in the header.
        /// </summary>
        public bool AllowQueryString { get; set; } = true;
    }
}
