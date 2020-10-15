using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [Route("externalsignin")]
    public class ExternalSignInController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;

        public ExternalSignInController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IEventPublisher eventPublisher)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult SignIn(string authenticationType)
        {
            var returnUrl = Url.Action("Index", "Home");
            var callbackUrl = Url.Action("SignInCallback", "ExternalSignIn", new { returnUrl });

            var authenticationProperties = new AuthenticationProperties { RedirectUri = callbackUrl };
            authenticationProperties.Items["LoginProvider"] = authenticationType;

            return Challenge(authenticationProperties, authenticationType);
        }

        [HttpGet]
        [Route("callback")]
        [AllowAnonymous]
        public async Task<ActionResult> SignInCallback(string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser platformUser = null;

            //try yo take an user name from claims
            var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Upn);
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("Received external login info does not have an UPN claim or DefaultUserName.");
            }

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            if (!externalLoginResult.Succeeded)
            {
                //Need handle the two cases
                //first - when the VC platform user account already exists, it is just missing an external login info and
                //second - when user does not have an account, then create a new account for them
                platformUser = await _userManager.FindByNameAsync(userName);
                if (platformUser == null)
                {
                    platformUser = new ApplicationUser
                    {
                        UserName = userName,
                        Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? (userName.IsValidEmail() ? userName : null)
                        // TODO: somehow access the AzureAd configuration section and read the default user type from there
                        //UserType = _authenticationOptions.AzureAdDefaultUserType
                    };

                    var result = await _userManager.CreateAsync(platformUser);
                    if (!result.Succeeded)
                    {
                        var joinedErrors = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                        throw new InvalidOperationException("Failed to save a VC platform account due the errors: " + joinedErrors);
                    }
                }

                var newExternalLogin = new UserLoginInfo(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, externalLoginInfo.ProviderDisplayName);

                await _userManager.AddLoginAsync(platformUser, newExternalLogin);

                //SignIn  user in the system
                var aspNetUser = await _signInManager.UserManager.FindByNameAsync(platformUser.UserName);
                await _signInManager.SignInAsync(aspNetUser, isPersistent: true);
            }
            else if (externalLoginResult.IsLockedOut || externalLoginResult.RequiresTwoFactor)
            {
                // TODO: handle user lock-out and two-factor authentication
                return RedirectToAction("Index", "Home");
            }

            if (platformUser == null)
            {
                platformUser = await _userManager.FindByNameAsync(userName);
            }

            await _eventPublisher.Publish(new UserLoginEvent(platformUser));

            return Redirect(returnUrl);
        }

        [HttpGet]
        [Route("providers")]
        [AllowAnonymous]
        public async Task<ActionResult> GetExternalLoginProviders()
        {
            var externalLoginProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Select(authenticationDescription => new ExternalSignInProviderInfo
                {
                    AuthenticationType = authenticationDescription.Name,
                    DisplayName = authenticationDescription.DisplayName
                })
                .ToArray();

            return Ok(externalLoginProviders);
        }
    }
}
