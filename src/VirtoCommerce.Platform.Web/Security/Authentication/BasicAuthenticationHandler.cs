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
            SignInManager<ApplicationUser> signInManager) : base(options, logger, encoder, clock)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var encodedAuth = authHeader.Substring("Basic ".Length).Trim();

                    if (string.IsNullOrEmpty(encodedAuth))
                    {
                        return AuthenticateResult.Fail("Invalid authentication provided, access denied.");
                    }

                    var loginPass = DecodeUserIdAndPassword(encodedAuth);
                    var user = await _userManager.FindByNameAsync(loginPass.Login);
                    if (user == null)
                    {
                        return AuthenticateResult.Fail("Invalid email provided, access denied.");
                    }

                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        return AuthenticateResult.Fail($"User { user.UserName } is not allowed to login.");
                    }

                    if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
                    {
                        return AuthenticateResult.Fail($"User { user.UserName } is currently locked out.");
                    }

                    if (!await _userManager.CheckPasswordAsync(user, loginPass.Password))
                    {
                        return AuthenticateResult.Fail("Invalid password provided, access denied.");
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

        private static (string Login, string Password) DecodeUserIdAndPassword(string encodedAuth)
        {
            var loginPass = Encoding.UTF8.GetString(Convert.FromBase64String(encodedAuth));

            var separator = loginPass.IndexOf(':');
            if (separator == -1) throw new InvalidOperationException("Invalid Authorization header: Missing separator character ':'. See RFC2617.");

            return (loginPass.Substring(0, separator), loginPass.Substring(separator + 1));
        }
    }
}
