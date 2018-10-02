using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
        public ActionResult SignIn(string authenticationType)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var returnUrl = Url.Action("Index", "Home");
            var callbackUrl = Url.Action("SignInCallback", "ExternalLogin", new { returnUrl });

            var authenticationProperties = new AuthenticationProperties { RedirectUri = callbackUrl };
            authenticationProperties.Dictionary["LoginProvider"] = authenticationType;

            var authenticationManager = _authenticationManagerFactory();
            authenticationManager.Challenge(authenticationProperties, authenticationType);
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
                return RedirectToAction("Index", "Home");
            }
            ApplicationUserExtended platformUser = null;

            // If the user does not have an account, then create a new account for them.
            var userName = externalLoginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Upn) ?? externalLoginInfo.DefaultUserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("Received external login info does not have an UPN claim or DefaultUserName.");
            }

            var signInManager = _signInManagerFactory();
            var externalLoginResult = await signInManager.ExternalSignInAsync(externalLoginInfo, false);

            if (externalLoginResult == SignInStatus.Failure)
            {
                platformUser = await _securityService.FindByNameAsync(userName, UserDetails.Full);
                if (platformUser != null)
                {
                    // The VC platform user account already exists, it is just missing an external login info.
                    var existingLogins = platformUser.Logins?.ToList() ?? new List<ApplicationUserLogin>();
                    existingLogins.Add(
                        new ApplicationUserLogin
                        {
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey
                        });
                    platformUser.Logins = existingLogins.ToArray();

                    var securityResult = await _securityService.UpdateAsync(platformUser);
                    if (!securityResult.Succeeded)
                    {
                        var joinedErrors = string.Join(Environment.NewLine, securityResult.Errors);
                        throw new InvalidOperationException($"Failed to add external login info to the VC platform account '{userName}' due to errors: " + joinedErrors);
                    }
                }
                else
                {
                    platformUser = new ApplicationUserExtended
                    {
                        UserName = userName,
                        UserType = _authenticationOptions.AzureAdDefaultUserType,
                        Logins = new[] {
                            new ApplicationUserLogin
                            {
                                LoginProvider = externalLoginInfo.Login.LoginProvider,
                                ProviderKey = externalLoginInfo.Login.ProviderKey
                            }
                        }
                    };
                    var securityResult = await _securityService.CreateAsync(platformUser);
                    if (!securityResult.Succeeded)
                    {
                        var joinedErrors = string.Join(Environment.NewLine, securityResult.Errors);
                        throw new InvalidOperationException("Failed to create a VC platform account due to errors: " + joinedErrors);
                    }
                }

                var aspNetUser = await signInManager.UserManager.FindByNameAsync(platformUser.UserName);
                await signInManager.SignInAsync(aspNetUser, isPersistent: true, rememberBrowser: true);
            }
            else if (externalLoginResult == SignInStatus.LockedOut || externalLoginResult == SignInStatus.RequiresVerification)
            {
                // TODO: handle user lock-out and two-factor authentication
                return RedirectToAction("Index", "Home");
            }

            if (platformUser == null)
            {
                platformUser = await _securityService.FindByNameAsync(userName, UserDetails.Full);
            }
            await _eventPublisher.Publish(new UserLoginEvent(platformUser));
            return Redirect(returnUrl);
        }

    }
}
