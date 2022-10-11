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
            const string scheme = "Basic ";
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var encodedCredentials = authHeader.Substring(scheme.Length).Trim();

                    var (userName, password) = DecodeUserNameAndPassword(encodedCredentials);
                    var user = await _userManager.FindByNameAsync(userName);
                    if (user == null)
                    {
                        return AuthenticateResult.Fail("Invalid user name or password, access denied.");
                    }

                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        return AuthenticateResult.Fail($"User { user.UserName } is not allowed to login.");
                    }

                    if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
                    {
                        return AuthenticateResult.Fail($"User { user.UserName } is currently locked out.");
                    }

                    if (!await _userManager.CheckPasswordAsync(user, password))
                    {
                        return AuthenticateResult.Fail("Invalid user name or password, access denied.");
                    }

                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var ticket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);

                    return AuthenticateResult.Success(ticket);
                }
                catch (Exception e)
                {
                    return AuthenticateResult.Fail(e.Message);
                }
            }

            return AuthenticateResult.NoResult();
        }

        private static (string userName, string password) DecodeUserNameAndPassword(string encodedCredentials)
        {
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var separatorPosition = decodedCredentials.IndexOf(':');
            if (separatorPosition == -1)
            {
                throw new InvalidOperationException("Invalid Authorization header value: Missing separator character ':'. See RFC2617.");
            }
            return (decodedCredentials.Substring(0, separatorPosition), decodedCredentials.Substring(separatorPosition + 1));
        }
    }
}
