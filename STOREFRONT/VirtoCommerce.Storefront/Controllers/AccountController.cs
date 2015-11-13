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
        private const string ResetCustomerPasswordTokenCookie = "Vcf.ResetCustomerPasswordToken";
        private const string CustomerIdCookie = "Vcf.CustomerId";

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
            var user = new VirtoCommercePlatformCoreSecurityApplicationUserExtended
            {
                Email = formModel.Email,
                Password = formModel.Password,
                UserName = formModel.Email,
            };

            var result = await _platformApi.FrontEndSecurityCreateAsync(user);

            if (result.Succeeded == true)
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

        [HttpPost]
        [Route("forgotpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPassword formModel)
        {
            var user = await _platformApi.FrontEndSecurityGetUserByNameAsync(formModel.Email);

            if (user != null)
            {
                string callbackUrl = Url.Action("ResetPassword", "Account",
                    new { UserId = user.Id, Code = "token" }, protocol: Request.Url.Scheme);

                await _platformApi.FrontEndSecurityGenerateResetPasswordTokenAsync(
                    user.Id, _workContext.CurrentStore.Id, callbackUrl);
            }
            else
            {
                ModelState.AddModelError("form", "User not found");
            }

            return new RedirectResult(Url.Action("Login", "Account") + "#recover");
        }

        [HttpGet]
        [Route("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string userId)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(userId))
            {
                _workContext.ErrorMessage = "Error in URL format";

                return View("error");
            }

            var user = await _platformApi.FrontEndSecurityGetUserByIdAsync(userId);
            if (user == null)
            {
                _workContext.ErrorMessage = "User was not found.";
                return View("error");
            }

            var tokenCookie = new HttpCookie(ResetCustomerPasswordTokenCookie, code);
            tokenCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(tokenCookie);

            var customerIdCookie = new HttpCookie(CustomerIdCookie, userId);
            customerIdCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(customerIdCookie);

            return View("customers/reset_password", _workContext);
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword formModel)
        {
            var customerIdCookie = HttpContext.Request.Cookies[CustomerIdCookie];
            string userId = customerIdCookie != null ? customerIdCookie.Value : null;

            var tokenCookie = HttpContext.Request.Cookies[ResetCustomerPasswordTokenCookie];
            string token = tokenCookie != null ? tokenCookie.Value : null;

            if (userId == null && token == null)
            {
                _workContext.ErrorMessage = "Not enough info for reseting password";
                return View("error");
            }

            var result = await _platformApi.FrontEndSecurityResetPasswordAsync(userId, token, formModel.Password);

            if (result.Succeeded == true)
            {
                HttpContext.Response.Cookies.Add(new HttpCookie(CustomerIdCookie) { Expires = DateTime.UtcNow.AddDays(-1) });
                HttpContext.Response.Cookies.Add(new HttpCookie(ResetCustomerPasswordTokenCookie) { Expires = DateTime.UtcNow.AddDays(-1) });

                return View("customers/reset_password_confirmation");
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/reset_password");
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
    }
}
