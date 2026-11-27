using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Extensions;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string InvalidCredentialsMessage = "Invalid user name or password.";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITwoFactorSignInService _twoFactorSignInService;
        private readonly PasswordLoginOptions _passwordLoginOptions;
        private readonly IEventPublisher _eventPublisher;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITwoFactorSignInService twoFactorSignInService,
            IOptions<PasswordLoginOptions> passwordLoginOptions,
            IEventPublisher eventPublisher)
            : base(options, logger, encoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _twoFactorSignInService = twoFactorSignInService;
            _passwordLoginOptions = passwordLoginOptions.Value;
            _eventPublisher = eventPublisher;
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
                return await FailAsync(delayedResponse, userName, user: null, SignInFailureReason.UserNotFound);
            }

            if (!_passwordLoginOptions.Enabled && !user.IsAdministrator)
            {
                return await FailAsync(delayedResponse, userName, user, SignInFailureReason.PasswordLoginDisabled);
            }

            // Same check as the token and login endpoints: counts the failure and locks the account
            // after Lockout:MaxFailedAccessAttempts, and refuses users that cannot sign in or are locked out.
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                return await FailAsync(delayedResponse, userName, user, ToFailureReason(signInResult));
            }

            if (await _twoFactorSignInService.IsTwoFactorRequiredAsync(user))
            {
                await PublishSignInAttemptAsync(userName, user, SignInFailureReason.RequiresTwoFactor);
                await delayedResponse.FailAsync();
                return AuthenticateResult.Fail("Two-factor authentication is required for this user. Use an API key instead of the password.");
            }

            // Records how long a success takes, without delaying it: Basic authentication runs on every request.
            _ = delayedResponse.SucceedAsync();

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            claimsPrincipal.SetAuthenticationMethod(SignInType.Password, []);
            var ticket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        private async Task<AuthenticateResult> FailAsync(DelayedResponse delayedResponse, string userName, ApplicationUser user, string failureReason)
        {
            await PublishSignInAttemptAsync(userName, user, failureReason);
            await delayedResponse.FailAsync();

            // Same message for every reason, so the response does not reveal whether the account exists or is locked.
            return AuthenticateResult.Fail(InvalidCredentialsMessage);
        }

        /// <summary>
        /// Only failures are recorded. Basic authentication sends the password with every request,
        /// so recording successes would add a sign-in log row per API call.
        /// </summary>
        private Task PublishSignInAttemptAsync(string userName, ApplicationUser user, string failureReason)
        {
            return _eventPublisher.Publish(new UserSignInAttemptEvent
            {
                UserName = userName,
                UserId = user?.Id,
                Succeeded = false,
                FailureReason = failureReason,
                SignInType = SignInType.Password,
                Provider = Options.Scheme,
                StoreId = user?.StoreId,
                MemberId = user?.MemberId,
            });
        }

        private static string ToFailureReason(SignInResult signInResult)
        {
            if (signInResult.IsLockedOut)
            {
                return SignInFailureReason.LockedOut;
            }

            if (signInResult.IsNotAllowed)
            {
                return SignInFailureReason.NotAllowed;
            }

            return SignInFailureReason.InvalidPassword;
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
