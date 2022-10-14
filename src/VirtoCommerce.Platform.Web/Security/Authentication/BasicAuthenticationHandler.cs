using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(options, logger, encoder, clock)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // This block is important when working with multiple authentication schemes when only cookies are being sent in request.
            // See Authorization:LimitedCookiePermissions setting.
            if (Context.User.Identity?.IsAuthenticated ?? false)
            {
                return AuthenticateResult.Success(new AuthenticationTicket(Context.User, "context.User"));
            }

            if (!TryGetEncodedCredentials(out var encodedCredentials))
            {
                return AuthenticateResult.NoResult();
            }

            if (!TryDecodeCredentials(encodedCredentials, out var userName, out var password))
            {
                return AuthenticateResult.Fail("Invalid Authorization header value.");
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null ||
                !await _signInManager.CanSignInAsync(user) ||
                _userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user) ||
                !await _userManager.CheckPasswordAsync(user, password))
            {
                return AuthenticateResult.Fail("Invalid user name or password.");
            }

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var ticket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }


        private bool TryGetEncodedCredentials(out string encodedCredentials)
        {
            encodedCredentials = null;

            if (Request.Headers.TryGetValue("Authorization", out var values))
            {
                var headerValue = values.ToString();
                const string scheme = "Basic ";

                if (headerValue != null && headerValue.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    encodedCredentials = headerValue.Substring(scheme.Length).Trim();
                }
            }

            return !string.IsNullOrEmpty(encodedCredentials);
        }

        private static bool TryDecodeCredentials(string encodedCredentials, out string userName, out string password)
        {
            userName = null;
            password = null;

            try
            {
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var separatorPosition = decodedCredentials.IndexOf(':');

                if (separatorPosition >= 0)
                {
                    userName = decodedCredentials.Substring(0, separatorPosition);
                    password = decodedCredentials.Substring(separatorPosition + 1);
                }
            }
            catch
            {
                // Ignore exceptions
            }

            return userName != null && password != null;
        }
    }
}
