using Microsoft.AspNetCore.Authentication;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";

        public string Scheme { get; set; } = DefaultScheme;
        public string ApiKeyParamName { get; set; } = PlatformConstants.Security.ApiKeyAuthentication.ParamName;
    }
}
