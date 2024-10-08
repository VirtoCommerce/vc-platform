using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.ExternalSignIn;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [Route("externalsignin")]
    public class ExternalSignInController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExternalSignInService _externalSignInService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEnumerable<ExternalSignInProviderConfiguration> _externalSigninProviderConfigs;
        private readonly IList<IExternalSignInValidator> _externalSignInValidators;

        public ExternalSignInController(SignInManager<ApplicationUser> signInManager,
            IExternalSignInService externalSignInService,
            IEventPublisher eventPublisher,
            IEnumerable<ExternalSignInProviderConfiguration> externalSigninProviderConfigs,
            IEnumerable<IExternalSignInValidator> externalSignInValidators)
        {
            _signInManager = signInManager;
            _userManager = _signInManager.UserManager;
            _externalSignInService = externalSignInService;
            _eventPublisher = eventPublisher;
            _externalSigninProviderConfigs = externalSigninProviderConfigs;
            _externalSignInValidators = externalSignInValidators.ToList();
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn([FromQuery] ExternalSignInRequest request)
        {
            if (string.IsNullOrEmpty(request.AuthenticationType))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(request.ReturnUrl))
            {
                request.ReturnUrl = GetHomeUrl();
            }

            var authenticationProperties = new AuthenticationProperties
            {
                Items = { ["LoginProvider"] = request.AuthenticationType },
                RedirectUri = Url.Action("SignInCallback", "ExternalSignIn", new { request.ReturnUrl }),
            };

            // Validate and apply front-end parameters
            if (!string.IsNullOrEmpty(request.StoreId) && !string.IsNullOrEmpty(request.OidcUrl) && !string.IsNullOrEmpty(request.CallbackUrl))
            {
                if (_externalSignInValidators.Count == 0)
                {
                    return BadRequest();
                }

                foreach (var validator in _externalSignInValidators)
                {
                    if (!await validator.ValidateAsync(request))
                    {
                        return BadRequest();
                    }
                }

                authenticationProperties.SetStoreId(request.StoreId);
                authenticationProperties.SetOidcUrl(request.OidcUrl);
                authenticationProperties.SetNewUserType(UserType.Customer.ToString());
                authenticationProperties.RedirectUri = request.CallbackUrl;
            }

            return Challenge(authenticationProperties, request.AuthenticationType);
        }

        [HttpGet]
        [Route("signout")]
        public async Task<ActionResult> SignOut(string authenticationType, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(authenticationType))
            {
                return BadRequest();
            }

            var userName = User.Identity?.Name;

            // sign out the current user
            if (!string.IsNullOrEmpty(userName))
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    await _eventPublisher.Publish(new UserLogoutEvent(user));
                }
            }

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.Items["LoginProvider"] = authenticationType;

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                authenticationProperties.RedirectUri = returnUrl;
            }

            return SignOut(authenticationProperties, authenticationType);
        }

        [HttpGet]
        [Route("callback")]
        [AllowAnonymous]
        public async Task<ActionResult> SignInCallback(string returnUrl)
        {
            var signInResult = await _externalSignInService.SignInAsync();

            var redirectUrl = signInResult.Success && Url.IsLocalUrl(returnUrl)
                ? returnUrl
                : GetHomeUrl();

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Route("providers")]
        [AllowAnonymous]
        public async Task<ActionResult<ExternalSignInProviderInfo[]>> GetExternalLoginProviders()
        {
            var providers = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Select(scheme => new ExternalSignInProviderInfo
                {
                    AuthenticationType = scheme.Name,
                    DisplayName = scheme.DisplayName,
                    LogoUrl = _externalSigninProviderConfigs
                        ?.FirstOrDefault(x => x.AuthenticationType.EqualsIgnoreCase(scheme.Name))
                        ?.LogoUrl,
                })
                .ToArray();

            return Ok(providers);
        }

        private string GetHomeUrl()
        {
            return Url.Action("Index", "Home") ?? "/";
        }
    }
}
