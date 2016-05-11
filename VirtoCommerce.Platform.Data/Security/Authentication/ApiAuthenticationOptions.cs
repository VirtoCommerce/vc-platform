using System;
using CacheManager.Core;
using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Authentication
{
    public class ApiAuthenticationOptions : AuthenticationOptions
    {
        public IClaimsIdentityProvider IdentityProvider { get; set; }
        public IApiAccountProvider ApiCredentialsProvider { get; set; }
        [CLSCompliant(false)]
        public ICacheManager<object> CacheManager { get; set; }

        public ApiAuthenticationOptions(string authenticationType)
            : base(authenticationType)
        {
        }
    }
}
