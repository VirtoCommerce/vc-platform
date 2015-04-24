using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Hmac
{
    public class HmacAuthenticationHandler : AuthenticationHandler<HmacAuthenticationOptions>
    {
        public const string CacheGroup = CacheGroups.Security + "_HmacAuthenticationHandler";

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            ClaimsIdentity identity = null;

            var userId = ExtractUserIdFromRequest();
            if (!string.IsNullOrEmpty(userId))
            {
                identity = await CreateIdentityByUserId(userId);
            }

            var properties = new AuthenticationProperties();
            var ticket = new AuthenticationTicket(identity, properties);
            return ticket;
        }


        private string ExtractUserIdFromRequest()
        {
            string userId = null;

            var headerValue = Request.Headers.Get("Authorization");

            AuthenticationHeaderValue authorization;
            if (AuthenticationHeaderValue.TryParse(headerValue, out authorization))
            {
                if (string.Equals(authorization.Scheme, Options.AuthenticationType, StringComparison.OrdinalIgnoreCase))
                {
                    ApiRequestSignature signature;
                    if (ApiRequestSignature.TryParse(authorization.Parameter, out signature))
                    {
                        if ((DateTime.UtcNow - signature.Timestamp).Duration() < Options.SignatureValidityPeriod)
                        {
                            var credentials = Options.ApiCredentialsProvider.GetAccountByAppId(signature.AppId);
                            if (credentials != null && IsValidSignature(signature, credentials))
                            {
                                userId = credentials.AccountId;
                            }
                        }
                    }
                }
            }

            return userId;
        }

        private bool IsValidSignature(ApiRequestSignature signature, ApiAccountEntity credentials)
        {
            var parameters = new[]
            {
                new NameValuePair(null, signature.AppId),
                new NameValuePair(null, signature.TimestampString)
            };

            var validSignature = HmacUtility.GetHashString(Options.HmacFactory, credentials.SecretKey, parameters);
            var isValid = string.Equals(signature.Hash, validSignature, StringComparison.OrdinalIgnoreCase);
            return isValid;
        }

        private async Task<ClaimsIdentity> CreateIdentityByUserId(string userId)
        {
            var cacheKey = CacheKey.Create(CacheGroup, "CreateIdentityByUserId", userId);
            var result = await Options.CacheManager.Get(cacheKey, () => CreateIdentityByUserIdInternal(userId));
            return result;
        }

        private async Task<ClaimsIdentity> CreateIdentityByUserIdInternal(string userId)
        {
            ClaimsIdentity identity = null;

            var userManager = Context.GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                identity = await userManager.CreateIdentityAsync(user, Options.AuthenticationType);
            }

            return identity;
        }
    }
}
