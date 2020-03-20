using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
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

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Query.TryGetValue(Options.ApiKeyParamName, out var apiKeyValues))
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyValues.FirstOrDefault();

            if (apiKeyValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = await _userApiKeyService.GetApiKeyByIdAsync(providedApiKey);
            if(apiKey == null)
            {
                return AuthenticateResult.NoResult();
            }

            var user = await _userManager.FindByIdAsync(apiKey.Id);
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
