using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    public class AuthenticationOptions
    {
        #region User validation

        public bool AllowOnlyAlphanumericUserNames { get; set; }
        public bool RequireUniqueEmail { get; set; }

        #endregion

        #region Password validation

        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonLetterOrDigit { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }

        #endregion

        #region Lockout

        public bool UserLockoutEnabledByDefault { get; set; }
        public TimeSpan DefaultAccountLockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        #endregion

        #region Cookies, tokens, API keys

        public TimeSpan DefaultTokenLifespan { get; set; }

        public bool CookiesEnabled { get; set; }
        public TimeSpan CookiesValidateInterval { get; set; }

        public bool BearerTokensEnabled { get; set; }
        public TimeSpan BearerTokensExpireTimeSpan { get; set; }

        public bool HmacEnabled { get; set; }
        public TimeSpan HmacSignatureValidityPeriod { get; set; }

        public bool ApiKeysEnabled { get; set; }
        public string ApiKeysHttpHeaderName { get; set; }
        public string ApiKeysQueryStringParameterName { get; set; }

        #endregion
    }
}
