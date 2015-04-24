using System;
using System.Security.Cryptography;
using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Hmac
{
    public class HmacAuthenticationOptions : AuthenticationOptions
    {
        public TimeSpan SignatureValidityPeriod { get; set; }
        public Func<byte[], HMAC> HmacFactory { get; set; }
        public IClaimsIdentityProvider IdentityProvider { get; set; }
        public IApiAccountProvider ApiCredentialsProvider { get; set; }
        public CacheManager CacheManager { get; set; }

        public HmacAuthenticationOptions()
            : base("HMACSHA256")
        {
            SignatureValidityPeriod = TimeSpan.FromMinutes(20);
            HmacFactory = key => new HMACSHA256(key);
        }
    }
}
