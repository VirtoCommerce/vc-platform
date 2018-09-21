using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Data.Security.Identity;

using PlatformAuthenticationOptions = VirtoCommerce.Platform.Core.Security.AuthenticationOptions;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [RoutePrefix("")]
    public class ExternalLoginController : Controller
    {
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly ISecurityService _securityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly PlatformAuthenticationOptions _authenticationOptions;

        public ExternalLoginController(Func<IAuthenticationManager> authenticationManagerFactory, Func<ApplicationSignInManager> signInManagerFactory,
            Func<ApplicationUserManager> userManagerFactory, ISecurityService securityService, IEventPublisher eventPublisher,
            PlatformAuthenticationOptions authenticationOptions)
        {
            _authenticationManagerFactory = authenticationManagerFactory;
            _signInManagerFactory = signInManagerFactory;
            _userManagerFactory = userManagerFactory;
            _securityService = securityService;
            _eventPublisher = eventPublisher;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("externalsignin")]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var returnUrl = Url.Action("Index", "Home");
            var callbackUrl = Url.Action("SignInCallback", "ExternalLogin", new { returnUrl });

            var authenticationProperties = new AuthenticationProperties { RedirectUri = callbackUrl };
            authenticationProperties.Dictionary["LoginProvider"] = OpenIdConnectAuthenticationDefaults.AuthenticationType;

            var authenticationManager = _authenticationManagerFactory();
            authenticationManager.Challenge(authenticationProperties, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            return new EmptyResult();
        }

        [HttpGet]
        [Route("externalsignin/callback")]
        [AllowAnonymous]
        public async Task<ActionResult> SignInCallback(string returnUrl)
        {
            var authenticationManager = _authenticationManagerFactory();
            var externalLoginInfo = await authenticationManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return Redirect(returnUrl);
            }

            var identity = externalLoginInfo.ExternalIdentity;
            var userName = identity.FindFirstValue(ClaimTypes.Upn);
            if (string.IsNullOrWhiteSpace(userName))
            {
                // TODO: apparently user doesn't have an UPN claim, but OpenID Connect provider must provide it:
                // https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-id-and-access-tokens#claims-in-idtokens
                return Redirect(returnUrl);
            }

            var signInManager = _signInManagerFactory();
            var externalLoginResult = await signInManager.ExternalSignInAsync(externalLoginInfo, false);

            switch (externalLoginResult)
            {
                case SignInStatus.Success:
                    await SignInExternalUser(externalLoginInfo, userName, signInManager);
                    return Redirect(returnUrl);

                case SignInStatus.Failure:
                    await CreateOrUpdatePlatformUser(externalLoginInfo, userName, signInManager);
                    return Redirect(returnUrl);

                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                    // TODO: handle user lock-out and 2FA
                    return Redirect(returnUrl);

                default:
                    // TODO: throw ArgumentOutOfRangeException?
                    return Redirect(returnUrl);
            }
        }

        private async Task SignInExternalUser(ExternalLoginInfo externalLoginInfo, string userName, ApplicationSignInManager signInManager)
        {
            var userManager = _userManagerFactory();

            var platformUser = await _securityService.FindByNameAsync(userName, UserDetails.Reduced);
            if (platformUser == null)
            {
                // TODO: something isn't right - user exists in ASP.NET Identity database, but does not exist in platform database.
                return;
            }

            var aspnetUser = await userManager.FindByNameAsync(userName);
            if (aspnetUser == null)
            {
                // TODO: this can't be - user successfully signed in, so they must exist in ASP.NET Identity database.
                return;
            }

            await signInManager.SignInAsync(aspnetUser, true, true);
            await _eventPublisher.Publish(new UserLoginEvent(platformUser));
        }

        private async Task CreateOrUpdatePlatformUser(ExternalLoginInfo externalLoginInfo, string userName,
            ApplicationSignInManager signInManager)
        {
            var platformUser = await _securityService.FindByNameAsync(userName, UserDetails.Reduced);
            if (platformUser != null)
            {
                await AddExternalLoginToExistingUser(platformUser, externalLoginInfo);
            }
            else
            {
                platformUser = await RegisterExternalUser(userName, externalLoginInfo);
                if (platformUser == null)
                {
                    return;
                }
            }

            var userManager = _userManagerFactory();
            var aspnetUser = await userManager.FindByNameAsync(userName);
            if (aspnetUser == null)
            {
                // TODO: at this moment user is guaranteed to exist in both platform and ASP.NET Identity database, so this normally couldn't be.
                return;
            }

            await signInManager.SignInAsync(aspnetUser, true, true);
            await _eventPublisher.Publish(new UserLoginEvent(platformUser));
        }

        private async Task AddExternalLoginToExistingUser(ApplicationUserExtended platformUser,
            ExternalLoginInfo externalLoginInfo)
        {
            var externalLogins = platformUser.Logins?.ToList() ?? new List<ApplicationUserLogin>();

            var externalLogin = ConvertExternalLoginInfoToExternalLogin(externalLoginInfo);
            externalLogins.Add(externalLogin);

            platformUser.Logins = externalLogins.ToArray();

            var result = await _securityService.UpdateAsync(platformUser);
            if (!result.Succeeded)
            {
                // TODO: something went wrong, so we should notify user or at least log it
            }
        }

        private async Task<ApplicationUserExtended> RegisterExternalUser(string userName, ExternalLoginInfo externalLoginInfo)
        {
            // TODO: access Graph API for more verbose information (like email and phone number)

            var user = new ApplicationUserExtended
            {
                UserName = userName,
                UserType = _authenticationOptions.AzureAdDefaultUserType,

                Logins = new[]
                {
                    ConvertExternalLoginInfoToExternalLogin(externalLoginInfo)
                }
            };

            var securityResult = await _securityService.CreateAsync(user);
            if (!securityResult.Succeeded)
            {
                // TODO: how to report error to UI?
                return null;
            }

            return user;
        }

        private ApplicationUserLogin ConvertExternalLoginInfoToExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            var externalLogin = externalLoginInfo.Login;
            return new ApplicationUserLogin
            {
                LoginProvider = externalLogin.LoginProvider,
                ProviderKey = externalLogin.ProviderKey
            };
        }
    }
}
