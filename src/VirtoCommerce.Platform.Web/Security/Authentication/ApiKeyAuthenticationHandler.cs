using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{

    /// <summary>
    /// Handle the Api Key scheme authentication.
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserApiKeyService _userApiKeyService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserApiKeyService userApiKeyService)
            : base(options, logger, encoder, clock)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userApiKeyService = userApiKeyService;

        }

        /// This method gets called for every request that requires authentication.
        //The logic goes something like this:
        //If no ApiKey is present on query string -> Return no result, let other handlers (if present) handle the request.
        //If the api_key is present but null or empty -> Return no result.
        //If the provided key does not exists -> Fail the authentication.
        //If the key is valid, create a new identity based on associated with key user      
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //This line is important to correct working the multiple authentication schemes when only cookies are being sent in request
            //see Authorization:LimitedCookiePermissions setting 
            if (Context.User?.Identity?.IsAuthenticated ?? false)
            {
                return AuthenticateResult.Success(new AuthenticationTicket(Context.User, "context.User"));
            }

            if (!Request.Query.TryGetValue(Options.ApiKeyParamName, out var apiKeyValues))
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyValues.FirstOrDefault();

            if (apiKeyValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = await _userApiKeyService.GetApiKeyByKeyAsync(providedApiKey);
            if (apiKey == null)
            {
                return AuthenticateResult.NoResult();
            }

            var user = await _userManager.FindByIdAsync(apiKey.UserId);
            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid authentication provided, access denied.");
            }

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var ticket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
