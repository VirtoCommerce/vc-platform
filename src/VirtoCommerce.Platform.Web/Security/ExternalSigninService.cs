using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Azure;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Security
{
    public class ExternalSigninService : IExternalSigninService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly AzureAdOptions _azureAdOptions;
        private readonly IdentityOptions _identityOptions;
        private readonly ISettingsManager _settingsManager;

        private IUrlHelper _urlHelper;

        public ExternalSigninService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEventPublisher eventPublisher,
            IOptions<AzureAdOptions> azureAdOptions,
            IOptions<IdentityOptions> identityOptions,
            ISettingsManager settingsManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _azureAdOptions = azureAdOptions.Value;
            _identityOptions = identityOptions.Value;
            _settingsManager = settingsManager;
        }

        public virtual async Task<string> ProcessCallbackAsync(string returnUrl, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;

            if (!_urlHelper.IsLocalUrl(returnUrl))
            {
                return _urlHelper.Action("index", "Home");
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if (!TryGetUserInfo(externalLoginInfo, out var userName, out var userEmail, out var redirectUrl))
            {
                return redirectUrl;
            }

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            var platformUser = await GetPlatformUser(userName, userEmail);

            var validationResult = await ValidateUserAsync(platformUser, externalLoginResult, returnUrl);
            if (!validationResult.Item1)
            {
                return validationResult.Item2;
            }

            if (!externalLoginResult.Succeeded)
            {
                var newExternalLogin = new UserLoginInfo(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, externalLoginInfo.ProviderDisplayName);
                await _userManager.AddLoginAsync(platformUser, newExternalLogin);

                //SignIn user in the system
                var aspNetUser = await _signInManager.UserManager.FindByNameAsync(platformUser.UserName);
                await _signInManager.SignInAsync(aspNetUser, isPersistent: true);
            }
            else if (externalLoginResult.IsLockedOut || externalLoginResult.RequiresTwoFactor)
            {
                // TODO: handle user lock-out and two-factor authentication
                return _urlHelper.Action("index", "Home");
            }

            platformUser ??= await _userManager.FindByNameAsync(userName);

            await _eventPublisher.Publish(new UserLoginEvent(platformUser));

            return returnUrl;
        }

        protected virtual Task<(bool, string)> ValidateUserAsync(ApplicationUser platformUser, SignInResult externalLoginResult, string returnUrl)
        {
            return Task.FromResult((true, returnUrl));
        }

        protected virtual async Task<ApplicationUser> GetPlatformUser(string userName, string userEmail)
        {
            //Need handle the two cases
            //first - when the VC platform user account already exists, it is just missing an external login info and
            //second - when user does not have an account, then create a new account for them
            var platformUser = await _userManager.FindByNameAsync(userName);

            if (_identityOptions.User.RequireUniqueEmail && platformUser == null)
            {
                platformUser = await FindUserByEmail(userEmail);
            }

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

            return platformUser;
        }

        protected virtual string GetUserName(ExternalLoginInfo externalLoginInfo)
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

        protected virtual async Task<ApplicationUser> FindUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }

        protected virtual async Task<string> GetAzureAdDefaultUserType()
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
                    await _settingsManager.SaveObjectSettingsAsync(new[] { userTypesSetting });
                }
            }

            return userType;
        }

        protected virtual bool TryGetUserInfo(ExternalLoginInfo externalLoginInfo, out string userName, out string userEmail, out string redirectUrl)
        {
            userName = string.Empty;
            userEmail = string.Empty;
            redirectUrl = string.Empty;

            if (externalLoginInfo == null)
            {
                redirectUrl = _urlHelper.Action("index", "Home");
                return false;
            }

            userName = GetUserName(externalLoginInfo);
            userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ??
                        (userName.IsValidEmail() ? userName : null);

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("Received external login info does not have an UPN claim or DefaultUserName.");
            }

            return true;
        }
    }
}
