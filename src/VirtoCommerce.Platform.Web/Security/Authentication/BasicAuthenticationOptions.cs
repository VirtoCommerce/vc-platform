using Microsoft.AspNetCore.Authentication;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    /// <summary>
    /// Options class provides information needed to control Basic Authentication handler behavior
    /// </summary>
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "BasicAuthentication";

        public string Scheme { get; set; } = DefaultScheme;
    }
}
