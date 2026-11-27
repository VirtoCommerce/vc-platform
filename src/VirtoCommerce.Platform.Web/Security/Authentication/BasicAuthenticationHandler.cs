using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Exceptions;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string InvalidCredentialsMessage = "Invalid user name or password.";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PasswordLoginOptions _passwordLoginOptions;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<PasswordLoginOptions> passwordLoginOptions)
            : base(options, logger, encoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordLoginOptions = passwordLoginOptions.Value;
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

            // Failures wait at least as long as the last success took, so the response time
            // does not tell an unknown user name from a wrong password.
            var delayedResponse = DelayedResponse.Create(nameof(BasicAuthenticationHandler), nameof(HandleAuthenticateAsync));

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return await FailAsync(delayedResponse);
            }

            if (!_passwordLoginOptions.Enabled && !user.IsAdministrator)
            {
                return await FailAsync(delayedResponse);
            }

            // Same check as the token and login endpoints: counts the failure and locks the account
            // after Lockout:MaxFailedAccessAttempts, and refuses users that cannot sign in or are locked out.
            SignInResult signInResult;
            try
            {
                signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            }
            catch (DuplicateEmailException)
            {
                // Saving the failure count validates the user, and that throws when another user has the same email.
                // Fail like any other wrong attempt instead of returning 500, as the token endpoint does.
                return await FailAsync(delayedResponse);
            }

            if (!signInResult.Succeeded)
            {
                return await FailAsync(delayedResponse);
            }

            // Records how long a success takes, without delaying it: Basic authentication runs on every request.
            _ = delayedResponse.SucceedAsync();

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var ticket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        private static async Task<AuthenticateResult> FailAsync(DelayedResponse delayedResponse)
        {
            await delayedResponse.FailAsync();

            // Same message for every reason, so the response does not reveal whether the account exists or is locked.
            return AuthenticateResult.Fail(InvalidCredentialsMessage);
        }

        private bool TryGetEncodedCredentials(out string encodedCredentials)
        {
            encodedCredentials = null;

            if (Request.Headers.TryGetValue("Authorization", out var values))
            {
                var headerValue = values.ToString();
                const string scheme = "Basic ";

                if (headerValue.StartsWithIgnoreCase(scheme))
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
