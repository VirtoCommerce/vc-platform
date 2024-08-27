using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.ExternalSignIn;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Security
{
    public class ExternalSignInService : IExternalSignInService, IExternalSigninService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IdentityOptions _identityOptions;
        private readonly ISettingsManager _settingsManager;
        private readonly IEnumerable<ExternalSignInProviderConfiguration> _externalSigninProviderConfigs;
        private readonly IEnumerable<IExternalSignInUserBuilder> _userBuilders;

        public ExternalSignInService(
            SignInManager<ApplicationUser> signInManager,
            IEventPublisher eventPublisher,
            IOptions<IdentityOptions> identityOptions,
            ISettingsManager settingsManager,
            IEnumerable<ExternalSignInProviderConfiguration> externalSigninProviderConfigs,
            IEnumerable<IExternalSignInUserBuilder> userBuilders)
        {
            _signInManager = signInManager;
            _userManager = signInManager.UserManager;
            _eventPublisher = eventPublisher;
            _identityOptions = identityOptions.Value;
            _settingsManager = settingsManager;
            _externalSigninProviderConfigs = externalSigninProviderConfigs;
            _userBuilders = userBuilders;
        }

        [Obsolete("Not being called. Use SignInAsync()", DiagnosticId = "VC0009", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public virtual async Task<string> ProcessCallbackAsync(string returnUrl, IUrlHelper urlHelper)
        {
            var signInResult = await SignInAsync();

            return signInResult.Success && urlHelper.IsLocalUrl(returnUrl)
                ? returnUrl
                : urlHelper.Action("Index", "Home") ?? "/";
        }

        public virtual async Task<ExternalSignInResult> SignInAsync()
        {
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo is null)
            {
                return ExternalSignInResult.Fail();
            }

            if (!TryGetUserInfo(externalLoginInfo, out var userName, out var userEmail))
            {
                return ExternalSignInResult.Fail();
            }

            var platformUser = await GetOrCreatePlatformUser(externalLoginInfo, userName, userEmail);
            if (platformUser == null)
            {
                throw new AuthenticationException($"The user {externalLoginInfo.Principal.Identity?.Name} for the external provider {externalLoginInfo.ProviderDisplayName} is not found.");
            }

            await _eventPublisher.Publish(new BeforeUserLoginEvent(platformUser, externalLoginInfo));

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false);

            if (externalLoginResult == SignInResult.Failed)
            {
                throw new AuthenticationException($"The requested provider {externalLoginInfo.ProviderDisplayName} has not been linked to an account, the provider must be linked from the back office.");
            }

            if (externalLoginResult == SignInResult.LockedOut)
            {
                throw new AuthenticationException($"The user {externalLoginInfo.Principal.Identity?.Name} for the external provider {externalLoginInfo.ProviderDisplayName} is locked out.");
            }

            if (externalLoginResult == SignInResult.TwoFactorRequired)
            {
                throw new NotImplementedException();
            }

            await SetLastLoginDate(platformUser);
            await _eventPublisher.Publish(new UserLoginEvent(platformUser, externalLoginInfo));

            return ExternalSignInResult.Succeed(externalLoginInfo.LoginProvider, platformUser);
        }

        private Task<IdentityResult> SetLastLoginDate(ApplicationUser user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            return _userManager.UpdateAsync(user);
        }

        private async Task<ApplicationUser> GetOrCreatePlatformUser(ExternalLoginInfo externalLoginInfo, string userName, string userEmail)
        {
            //Need handle the two cases
            //first - when the VC platform user account already exists, it is just missing an external login info and
            //second - when user does not have an account, then create a new account for them
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null && _identityOptions.User.RequireUniqueEmail && !string.IsNullOrEmpty(userEmail))
            {
                user = await _userManager.FindByEmailAsync(userEmail);
            }

            if (user == null && AllowCreateNewUser(externalLoginInfo))
            {
                user = AbstractTypeFactory<ApplicationUser>.TryCreateInstance();
                user.UserName = userName;
                user.Email = userEmail;
                user.EmailConfirmed = true;
                user.UserType = await GetDefaultUserType(externalLoginInfo);
                user.StoreId = externalLoginInfo.AuthenticationProperties.GetStoreId();
                user.Status = await _settingsManager.GetValueAsync<string>(PlatformConstants.Settings.Security.DefaultExternalAccountStatus);

                foreach (var userBuilder in _userBuilders)
                {
                    await userBuilder.BuildNewUser(user, externalLoginInfo);
                }

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var joinedErrors = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                    throw new InvalidOperationException("Failed to save a VC platform account due the errors: " + joinedErrors);
                }

                var roles = GetDefaultUserRoles(externalLoginInfo);

                if (roles is { Length: > 0 })
                {
                    await _userManager.AddToRolesAsync(user, roles);
                }
            }

            if (user != null && await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey) == null)
            {
                // Register a new external login
                var newExternalLogin = new UserLoginInfo(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, externalLoginInfo.ProviderDisplayName);
                await _userManager.AddLoginAsync(user, newExternalLogin);
            }

            return user;
        }

        private bool AllowCreateNewUser(ExternalLoginInfo externalLoginInfo)
        {
            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            return providerConfig?.Provider.AllowCreateNewUser == true;
        }

        private async Task<string> GetDefaultUserType(ExternalLoginInfo externalLoginInfo)
        {
            var userType = externalLoginInfo.AuthenticationProperties?.GetNewUserType();

            if (string.IsNullOrEmpty(userType))
            {
                var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);

                userType = providerConfig?.Provider is not null
                    ? providerConfig.Provider.GetUserType()
                    : "Manager";
            }

            var userTypesSetting = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Security.SecurityAccountTypes.Name);
            var userTypes = userTypesSetting.AllowedValues.Select(x => x.ToString()).ToList();

            if (!userTypes.Contains(userType))
            {
                userTypes.Add(userType);
                userTypesSetting.AllowedValues = userTypes.Select(x => (object)x).ToArray();

                using (await AsyncLock.GetLockByKey("settings").LockAsync())
                {
                    await _settingsManager.SaveObjectSettingsAsync([userTypesSetting]);
                }
            }

            return userType;
        }

        private string[] GetDefaultUserRoles(ExternalLoginInfo externalLoginInfo)
        {
            var userRoles = Array.Empty<string>();
            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            if (providerConfig?.Provider is not null)
            {
                userRoles = providerConfig.Provider.GetUserRoles();
            }

            return userRoles;
        }

        private bool TryGetUserInfo(ExternalLoginInfo externalLoginInfo, out string userName, out string userEmail)
        {
            userName = string.Empty;
            userEmail = string.Empty;

            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            if (providerConfig?.Provider is not null)
            {
                userName = providerConfig.Provider.GetUserName(externalLoginInfo);
                userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ??
                            (userName.IsValidEmail() ? userName : null);
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("External login provider does not return user name.");
            }

            return true;
        }

        private ExternalSignInProviderConfiguration GetExternalSigninProviderConfiguration(ExternalLoginInfo externalLoginInfo)
        {
            return _externalSigninProviderConfigs.FirstOrDefault(x => x.AuthenticationType.EqualsIgnoreCase(externalLoginInfo.LoginProvider));
        }
    }
}
