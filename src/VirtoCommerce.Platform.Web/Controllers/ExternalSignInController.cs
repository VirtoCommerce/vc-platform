using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [Route("externalsignin")]
    public class ExternalSignInController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IExternalSigninService _externalSigninService;

        public ExternalSignInController(SignInManager<ApplicationUser> signInManager,
            IExternalSigninService externalSigninService)
        {
            _signInManager = signInManager;
            _externalSigninService = externalSigninService;
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
