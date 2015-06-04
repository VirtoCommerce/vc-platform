using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Authentication
{
    public abstract class ApiAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
        where TOptions : ApiAuthenticationOptions
    {
        public const string CacheGroup = CacheGroups.Security + "_ApiAuthenticationHandler";

        protected abstract string ExtractUserIdFromRequest();

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            ClaimsIdentity identity = null;

            var userId = ExtractUserIdFromRequest();
            if (!string.IsNullOrEmpty(userId))
            {
                identity = await GetIdentityByUserId(userId);
            }

            var properties = new AuthenticationProperties();
            var ticket = new AuthenticationTicket(identity, properties);
            return ticket;
        }

        protected virtual string GetAuthenticationHeaderCredentials()
        {
            string result = null;

            var headerValue = Request.Headers.Get("Authorization");

            AuthenticationHeaderValue authentication;
            if (AuthenticationHeaderValue.TryParse(headerValue, out authentication))
            {
                if (string.Equals(authentication.Scheme, Options.AuthenticationType, StringComparison.OrdinalIgnoreCase))
                {
                    result = authentication.Parameter;
                }
            }

            return result;
        }

        protected virtual async Task<ClaimsIdentity> GetIdentityByUserId(string userId)
        {
            var cacheKey = CacheKey.Create(CacheGroup, "GetIdentityByUserId", userId);
            var result = await Options.CacheManager.Get(cacheKey, () => CreateIdentityByUserId(userId));
            return result;
        }

        protected virtual async Task<ClaimsIdentity> CreateIdentityByUserId(string userId)
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
