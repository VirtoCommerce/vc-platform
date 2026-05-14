using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenIddict.Validation.AspNetCore;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Infrastructure
{
    /// <summary>
    /// https://github.com/openiddict/openiddict-core/issues/594
    /// This custom provider allows able to use just [Authorize] instead of having to define [Authorize(AuthenticationSchemes = "Bearer")] above every API controller
    /// without this Bearer authorization will not work
    /// </summary>
    [Obsolete("Not used. Switched to Mixed authentication schema.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class CustomAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthenticationSchemeProvider(IHttpContextAccessor httpContextAccessor, IOptions<AuthenticationOptions> options)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<AuthenticationScheme> GetRequestSchemeAsync()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null)
            {
                throw new ArgumentNullException("The HTTP request cannot be retrieved.");
            }

            var authorization = request.Headers.Authorization.ToString();

            if (!authorization.IsNullOrEmpty() && authorization.StartsWithIgnoreCase("Bearer "))
            {
                return await GetSchemeAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            }

            return null;
        }

        public override async Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync() =>
                    await GetRequestSchemeAsync() ??
                    await base.GetDefaultAuthenticateSchemeAsync();

        public override async Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync() =>
            await GetRequestSchemeAsync() ??
            await base.GetDefaultChallengeSchemeAsync();

        public override async Task<AuthenticationScheme> GetDefaultForbidSchemeAsync() =>
            await GetRequestSchemeAsync() ??
            await base.GetDefaultForbidSchemeAsync();

        public override async Task<AuthenticationScheme> GetDefaultSignInSchemeAsync() =>
            await GetRequestSchemeAsync() ??
            await base.GetDefaultSignInSchemeAsync();

        public override async Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync() =>
            await GetRequestSchemeAsync() ??
            await base.GetDefaultSignOutSchemeAsync();
    }
}
