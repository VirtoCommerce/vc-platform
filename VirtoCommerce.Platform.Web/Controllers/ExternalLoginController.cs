using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Web.Model.Security;
using PlatformAuthenticationOptions = VirtoCommerce.Platform.Core.Security.AuthenticationOptions;

namespace VirtoCommerce.Platform.Web.Controllers
{
    [RoutePrefix("externalsignin")]
    public class ExternalLoginController : Controller
    {
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly ISecurityService _securityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly PlatformAuthenticationOptions _authenticationOptions;

        public ExternalLoginController(Func<IAuthenticationManager> authenticationManagerFactory, Func<ApplicationSignInManager> signInManagerFactory,
            ISecurityService securityService, IEventPublisher eventPublisher, PlatformAuthenticationOptions authenticationOptions)
        {
            _authenticationManagerFactory = authenticationManagerFactory;
            _signInManagerFactory = signInManagerFactory;
            _securityService = securityService;
            _eventPublisher = eventPublisher;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("")]
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
        [Route("callback")]
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

            //try yo take an user name from claims
            var userName = externalLoginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Upn) ?? externalLoginInfo.DefaultUserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("Received external login info does not have an UPN claim or DefaultUserName.");
            }

            var signInManager = _signInManagerFactory();
            var externalLoginResult = await signInManager.ExternalSignInAsync(externalLoginInfo, false);
            if (externalLoginResult == SignInStatus.Failure)
            {
                //Need handle the two cases
                //first - when the VC platform user account already exists, it is just missing an external login info and
                //second - when user does not have an account, then create a new account for them
                platformUser = await _securityService.FindByNameAsync(userName, UserDetails.Full);
                var newExtenalLogin = new ApplicationUserLogin
                {
                    LoginProvider = externalLoginInfo.Login.LoginProvider,
                    ProviderKey = externalLoginInfo.Login.ProviderKey
                };

                if (platformUser == null)
                {
                    platformUser = new ApplicationUserExtended
                    {
                        UserName = userName,
                        UserType = _authenticationOptions.AzureAdDefaultUserType,
                        Logins = new ApplicationUserLogin[] { }
                    };
                }
                platformUser.Logins = platformUser.Logins.Concat(new[] { newExtenalLogin }).ToArray();

                var result = await (platformUser.IsTransient() ? _securityService.CreateAsync(platformUser) : _securityService.UpdateAsync(platformUser));
                if (!result.Succeeded)
                {
                    var joinedErrors = string.Join(Environment.NewLine, result.Errors);
                    throw new InvalidOperationException("Failed to save a VC platform account due the errors: " + joinedErrors);
                }
                //SignIn  user in the system
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

        [HttpGet]
        [Route("providers")]
        [AllowAnonymous]
        public ActionResult GetExternalLoginProviders()
        {
            var authenticationManager = _authenticationManagerFactory();
            var externalLoginProviders = authenticationManager.GetExternalAuthenticationTypes()
                .Select(authenticationDescription => new ExternalLoginProviderInfo
                {
                    AuthenticationType = authenticationDescription.AuthenticationType,
                    DisplayName = authenticationDescription.Caption
                })
                .ToArray();

            // Unfortunately, JsonResult (Json(...)) doesn't allow to translate PascalCase to camelCase
            // in property names, so we'll have to do it manually.
            var serializedResult = JsonConvert.SerializeObject(externalLoginProviders,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            return Content(serializedResult, "application/json", Encoding.UTF8);
        }
    }
}
