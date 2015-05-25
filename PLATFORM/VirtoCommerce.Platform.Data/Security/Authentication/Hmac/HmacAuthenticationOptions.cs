using System;
using System.Security.Cryptography;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Hmac
{
    public class HmacAuthenticationOptions : ApiAuthenticationOptions
    {
        public TimeSpan SignatureValidityPeriod { get; set; }
        public Func<byte[], HMAC> HmacFactory { get; set; }

        public HmacAuthenticationOptions()
            : base("HMACSHA256")
        {
            SignatureValidityPeriod = TimeSpan.FromMinutes(20);
            HmacFactory = key => new HMACSHA256(key);
        }
    }
}
