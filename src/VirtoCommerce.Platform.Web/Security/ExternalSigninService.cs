using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Security
{
    public class ExternalSigninService : IExternalSigninService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IdentityOptions _identityOptions;
        private readonly ISettingsManager _settingsManager;
        private readonly IEnumerable<ExternalSignInProviderConfiguration> _externalSigninProviderConfigs;

        private IUrlHelper _urlHelper;

        [ActivatorUtilitiesConstructor]
        public ExternalSigninService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEventPublisher eventPublisher,
            IOptions<IdentityOptions> identityOptions,
            ISettingsManager settingsManager,
            IEnumerable<ExternalSignInProviderConfiguration> externalSigninProviderConfigs)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _identityOptions = identityOptions.Value;
            _settingsManager = settingsManager;
            _externalSigninProviderConfigs = externalSigninProviderConfigs;
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

            var platformUser = await GetOrCreatePlatformUser(externalLoginInfo, userName, userEmail);
            if (platformUser == null)
            {
                throw new AuthenticationException($"The user {externalLoginInfo.Principal.Identity?.Name} for the external provider {externalLoginInfo.ProviderDisplayName} is not found.");
            }

            await _eventPublisher.Publish(new BeforeUserLoginEvent(platformUser, externalLoginInfo));

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);

            if (externalLoginResult == SignInResult.Failed)
            {
                throw new AuthenticationException($"The requested provider {externalLoginInfo.ProviderDisplayName} has not been linked to an account, the provider must be linked from the back office.");
            }
            else if (externalLoginResult == SignInResult.LockedOut)
            {
                throw new AuthenticationException($"The user {externalLoginInfo.Principal.Identity?.Name} for the external provider {externalLoginInfo.ProviderDisplayName} is locked out.");
            }
            else if (externalLoginResult == SignInResult.TwoFactorRequired)
            {
                throw new NotImplementedException();
            }

            var validationResult = await ValidateUserAsync(platformUser, externalLoginResult, returnUrl);
            if (!validationResult.Item1)
            {
                return validationResult.Item2;
            }

            await SetLastLoginDate(platformUser);
            await _eventPublisher.Publish(new UserLoginEvent(platformUser, externalLoginInfo));

            return returnUrl;
        }

        private Task SetLastLoginDate(ApplicationUser user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            return _signInManager.UserManager.UpdateAsync(user);
        }

        protected virtual Task<(bool, string)> ValidateUserAsync(ApplicationUser platformUser, SignInResult externalLoginResult, string returnUrl)
        {
            return Task.FromResult((true, returnUrl));
        }

        protected virtual async Task<ApplicationUser> GetOrCreatePlatformUser(ExternalLoginInfo externalLoginInfo, string userName, string userEmail)
        {
            //Need handle the two cases
            //first - when the VC platform user account already exists, it is just missing an external login info and
            //second - when user does not have an account, then create a new account for them
            var platformUser = await _userManager.FindByNameAsync(userName);

            if (_identityOptions.User.RequireUniqueEmail && platformUser == null)
            {
                platformUser = await FindUserByEmail(userEmail);
            }

            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            if (platformUser == null)
            {
                if (providerConfig?.Provider.AllowCreateNewUser == true)
                {
                    platformUser = AbstractTypeFactory<ApplicationUser>.TryCreateInstance();

                    platformUser.UserName = userName;
                    platformUser.Email = userEmail;
                    platformUser.UserType = await GetDefaultUserType(externalLoginInfo);

                    var result = await _userManager.CreateAsync(platformUser);
                    if (!result.Succeeded)
                    {
                        var joinedErrors = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                        throw new InvalidOperationException("Failed to save a VC platform account due the errors: " + joinedErrors);
                    }

                    var roles = GetDefaultUserRoles(externalLoginInfo);

                    if (roles is { Length: > 0 })
                    {
                        await _userManager.AddToRolesAsync(platformUser, roles);
                    }
                }
            }

            var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
            if (user == null)
            {
                // Register a new external login
                var newExternalLogin = new UserLoginInfo(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, externalLoginInfo.ProviderDisplayName);
                await _userManager.AddLoginAsync(platformUser, newExternalLogin);
            }


            return platformUser;
        }

        protected virtual async Task<ApplicationUser> FindUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }


        protected virtual async Task<string> GetDefaultUserType(ExternalLoginInfo externalLoginInfo)
        {
            var userType = "Manager";

            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            if (providerConfig?.Provider is not null)
            {
                userType = providerConfig.Provider.GetUserType();
            }

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

        protected virtual string[] GetDefaultUserRoles(ExternalLoginInfo externalLoginInfo)
        {
            var userRoles = Array.Empty<string>();
            var providerConfig = GetExternalSigninProviderConfiguration(externalLoginInfo);
            if (providerConfig?.Provider is not null)
            {
                userRoles = providerConfig.Provider.GetUserRoles();
            }

            return userRoles;
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

        protected virtual ExternalSignInProviderConfiguration GetExternalSigninProviderConfiguration(ExternalLoginInfo externalLoginInfo)
        {
            return _externalSigninProviderConfigs.FirstOrDefault(x => x.AuthenticationType.EqualsInvariant(externalLoginInfo.LoginProvider));
        }
    }
}
