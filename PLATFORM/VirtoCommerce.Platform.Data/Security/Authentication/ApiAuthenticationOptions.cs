using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Authentication
{
    public class ApiAuthenticationOptions : AuthenticationOptions
    {
        public IClaimsIdentityProvider IdentityProvider { get; set; }
        public IApiAccountProvider ApiCredentialsProvider { get; set; }
        public CacheManager CacheManager { get; set; }

        public ApiAuthenticationOptions(string authenticationType)
            : base(authenticationType)
        {
        }
    }
}
