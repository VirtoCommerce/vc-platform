using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICustomerManagementModuleApi _customerApi;

        public AccountController(WorkContext workContext, IVirtoCommercePlatformApi platformApi, ICustomerManagementModuleApi customerApi)
        {
            _workContext = workContext;
            _platformApi = platformApi;
            _customerApi = customerApi;
        }

        private IAuthenticationManager _authenticationManager;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication);
            }
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index(int page = 1)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _workContext.CurrentPage = page;
                return View("customers/account");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("customers/register", _workContext);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(Register formModel)
        {
            if (!ValidateModelState())
            {
                return View("customers/register", _workContext);
            }

            var user = new VirtoCommercePlatformCoreSecurityApplicationUserExtended
            {
                Email = formModel.Email,
                Password = formModel.Password,
                UserName = formModel.Email,
            };

            var result = await _platformApi.FrontEndSecurityCreateAsync(user);

            if (result.Succeeded.Value)
            {
                user = await _platformApi.FrontEndSecurityGetUserByNameAsync(user.UserName);

                var contact = new VirtoCommerceCustomerModuleWebModelContact
                {
                    Id = user.Id,
                    Emails = new List<string> { formModel.Email },
                    FullName = string.Join(" ", formModel.FirstName, formModel.LastName),
                };

                if (string.IsNullOrEmpty(contact.FullName))
                {
                    contact.FullName = formModel.Email;
                }

                contact = await _customerApi.CustomerModuleCreateContactAsync(contact);

                await _platformApi.FrontEndSecurityPasswordSignInAsync(formModel.Email, formModel.Password, false);

                var identity = CreateClaimsIdentity(formModel.Email);
                AuthenticationManager.SignIn(identity);
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/register", _workContext);
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut();
            }

            _workContext.Login = new Login();

            return View("customers/login", _workContext);
        }

        // POST: /Account/Login
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login formModel, string returnUrl)
        {
            if (!ValidateModelState())
            {
                return View("customers/login", _workContext);
            }

            var loginResult = await _platformApi.FrontEndSecurityPasswordSignInAsync(formModel.Email, formModel.Password, false);

            switch (loginResult.Status)
            {
                case "success":
                    var identity = CreateClaimsIdentity(formModel.Email);
                    AuthenticationManager.SignIn(identity);
                    return RedirectToLocal(returnUrl);
                case "lockedOut":
                    return View("lockedout");
                case "requiresVerification":
                    return RedirectToAction("SendCode", "Account");
                case "failure":
                default:
                    ModelState.AddModelError("form", "Login attempt failed.");
                    return View("customers/login");
            }
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return Redirect("~");
        }


        private ClaimsIdentity CreateClaimsIdentity(string userName)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));

            var identity = new ClaimsIdentity(claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~");
        }

        private bool ValidateModelState()
        {
            var formErrors = GetFormErrors(ModelState);

            if (formErrors != null)
            {
                foreach (var error in formErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return formErrors == null;
        }

        private IDictionary<string, string> GetFormErrors(ModelStateDictionary modelState)
        {
            IDictionary<string, string> formErrors = null;

            if (!modelState.IsValid)
            {
                var errorsDictionary = new Dictionary<string, string>();

                foreach (var error in modelState.Where(f => f.Value.Errors.Count > 0))
                {
                    var errorMessage = error.Value.Errors.First();
                    errorsDictionary.Add(error.Key, errorMessage.ErrorMessage);
                }

                formErrors = errorsDictionary;
            }

            return formErrors;
        }
    }
}
