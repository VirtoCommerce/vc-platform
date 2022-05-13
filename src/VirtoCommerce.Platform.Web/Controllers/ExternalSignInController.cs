using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Azure;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [Route("externalsignin")]
    public class ExternalSignInController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AzureAdOptions _azureAdOptions;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISettingsManager _settingsManager;

        public ExternalSignInController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<AzureAdOptions> azureAdOptions,
            IEventPublisher eventPublisher,
            ISettingsManager settingsManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _settingsManager = settingsManager;
            _azureAdOptions = azureAdOptions.Value;
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
            var userName = GetUserName(externalLoginInfo);
            var userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ??
                            (userName.IsValidEmail() ? userName : null);

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
                platformUser = await _userManager.FindByNameAsync(userName) ??
                               await FindUserByEmail(userEmail);

                if (platformUser == null)
                {
                    platformUser = new ApplicationUser
                    {
                        UserName = userName,
                        Email = userEmail,
                        UserType = await GetAzureAdDefaultUserType()
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

            platformUser ??= await _userManager.FindByNameAsync(userName);

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

        private async Task<string> GetAzureAdDefaultUserType()
        {
            var userType = _azureAdOptions.DefaultUserType ?? "Manager";
            var userTypesSetting = await _settingsManager.GetObjectSettingAsync("VirtoCommerce.Platform.Security.AccountTypes");

            var userTypes = userTypesSetting.AllowedValues.Select(x => x.ToString()).ToList();

            if (!userTypes.Contains(userType))
            {
                userTypes.Add(userType);
                userTypesSetting.AllowedValues = userTypes.Select(x => (object)x).ToArray();

                using (await AsyncLock.GetLockByKey("settings").GetReleaserAsync())
                {
                    await _settingsManager.SaveObjectSettingsAsync(new [] { userTypesSetting });
                }
            }

            return userType;
        }

        /// <summary>
        /// Try to take a user name from claims.
        /// </summary>
        private string GetUserName(ExternalLoginInfo externalLoginInfo)
        {
            var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Upn);

            if (string.IsNullOrWhiteSpace(userName) && _azureAdOptions.UsePreferredUsername)
            {
                userName = externalLoginInfo.Principal.FindFirstValue("preferred_username");
            }

            if (string.IsNullOrWhiteSpace(userName) && _azureAdOptions.UseEmail)
            {
                userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            }

            return userName;
        }

        private async Task<ApplicationUser> FindUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
