using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IExternalSigninService _externalSigninService;
        private readonly IEventPublisher _eventPublisher;

        public ExternalSignInController(SignInManager<ApplicationUser> signInManager,
            IExternalSigninService externalSigninService,
            IEventPublisher eventPublisher)
        {
            _signInManager = signInManager;
            _externalSigninService = externalSigninService;
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
        [Route("signout")]
        public async Task<ActionResult> SignOut(string authenticationType)
        {
            // sign out the current user
            var user = await _signInManager.UserManager.FindByNameAsync(User?.Identity?.Name);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _eventPublisher.Publish(new UserLogoutEvent(user));
            }

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.Items["LoginProvider"] = authenticationType;
            return SignOut(authenticationProperties, authenticationType);
        }

        [HttpGet]
        [Route("callback")]
        [AllowAnonymous]
        public async Task<ActionResult> SignInCallback(string returnUrl)
        {
            var redirectUrl = await _externalSigninService.ProcessCallbackAsync(returnUrl, Url);

            return Redirect(redirectUrl);
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
