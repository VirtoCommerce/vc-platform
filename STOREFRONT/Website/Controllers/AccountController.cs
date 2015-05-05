using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.FormModels;

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("account")]
    [Authorize]
    public class AccountController : StoreControllerBase
    {
        private const string ResetCustomerPasswordTokenCookie = "Vcf.ResetCustomerPasswordToken";
        private const string CustomerIdCookie = "Vcf.CustomerId";

        private IAuthenticationManager _authenticationManager;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication);
            }
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        //[Route("login")]
        public ActionResult Login(string returnUrl)
        {
            return View("customers/login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[Route("login")]
        public async Task<ActionResult> Login(LoginFormModel formModel, string returnUrl)
        {
            var form = Service.GetForm(formModel.Id);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.PostedSuccessfully = true;

                    var loginResult = await SecurityService.PasswordSingInAsync(
                        formModel.Email, formModel.Password, false);

                    switch (loginResult)
                    {
                        case SignInStatus.Success:
                            var identity = SecurityService.CreateClaimsIdentity(formModel.Email);
                            AuthenticationManager.SignIn(identity);
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("lockedout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", "Account");
                        case SignInStatus.Failure:
                        default:
                            form.Errors = new SubmitFormErrors("form", "Login attempt fails.");
                            form.PostedSuccessfully = false;
                            return View("customers/login");
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    return View("customers/login");
                }
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        [Route("register")]
        public ActionResult Register()
        {
            return View("customers/register");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterFormModel formModel)
        {
            var form = Service.GetForm(formModel.Id);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.PostedSuccessfully = true;

                    var user = new ApplicationUser
                    {
                        Email = formModel.Email,
                        Password = formModel.Password,
                        UserName = formModel.Email
                    };

                    var result = await SecurityService.CreateUserAsync(user);

                    if (result.Succeeded)
                    {
                        user = await SecurityService.GetUserByNameAsync(user.UserName);

                        Context.Customer = await this.CustomerService.CreateCustomerAsync(
                            formModel.Email, formModel.FirstName, formModel.LastName, user.Id, null);

                        await SecurityService.PasswordSingInAsync(formModel.Email, formModel.Password, false);

                        var identity = SecurityService.CreateClaimsIdentity(formModel.Email);
                        AuthenticationManager.SignIn(identity);

                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;
                }
            }
            else
            {
                return View("error");
            }

            return View("customers/register");
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordFormModel formModel)
        {
            var form = Service.GetForm(formModel.Id);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.PostedSuccessfully = true;

                    var user = await SecurityService.GetUserByNameAsync(formModel.Email);

                    if (user != null)
                    {
                        string callbackUrl = Url.Action("ResetPassword", "Account",
                            new { UserId = user.Id, Code = "token" }, protocol: Request.Url.Scheme);

                        await SecurityService.GenerateResetPasswordTokenAsync(
                            user.Id, Context.Shop.Name, callbackUrl);
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", "User not found");
                        form.PostedSuccessfully = false;
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;
                }
            }
            else
            {
                Context.ErrorMessage = "Liquid error: Form context was not found.";

                return View("error");
            }

            return new RedirectResult(Url.Action("Login", "Account") + "#recover");
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string userId)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(userId))
            {
                Context.ErrorMessage = "Error in URL format";

                return View("error");
            }

            var user = await SecurityService.GetUserByIdAsync(userId);
            if (user == null)
            {
                Context.ErrorMessage = "User was not found.";

                return View("error");
            }

            var tokenCookie = new HttpCookie(ResetCustomerPasswordTokenCookie, code);
            tokenCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(tokenCookie);

            var customerIdCookie = new HttpCookie(CustomerIdCookie, userId);
            customerIdCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(customerIdCookie);

            return View("customers/reset_password");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordFormModel formModel)
        {
            var form = Service.GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                string userId = HttpContext.Request.Cookies[CustomerIdCookie] != null ?
                    HttpContext.Request.Cookies[CustomerIdCookie].Value : null;
                string token = HttpContext.Request.Cookies[ResetCustomerPasswordTokenCookie] != null ?
                    HttpContext.Request.Cookies[ResetCustomerPasswordTokenCookie].Value : null;

                if (userId == null && token == null)
                {
                    Context.ErrorMessage = "Not enough info for reseting password";

                    return View("error");
                }

                if (formErrors == null)
                {
                    var result = await SecurityService.ResetPasswordAsync(userId, token, formModel.Password);

                    if (result.Succeeded)
                    {
                        HttpContext.Response.Cookies.Remove(CustomerIdCookie);
                        HttpContext.Response.Cookies.Remove(ResetCustomerPasswordTokenCookie);

                        return View("password_reseted");
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;
                }
            }
            else
            {
                Context.ErrorMessage = "Liquid error: Form context was not found.";

                return View("error");
            }

            return View("customers/reset_password");
        }

        //
        // GET: /Account/LogOFf
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return Redirect("~");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(ExternalLoginFormModel formModel, string returnUrl)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                return new ChallengeResult(
                    formModel.AuthenticationType,
                    Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                Context.ErrorMessage = "External login info was not found.";

                return View("error");
            }

            var user = await SecurityService.GetUserByLoginAsync(new UserLoginInfo
            {
                LoginProvider = loginInfo.Login.LoginProvider,
                ProviderKey = loginInfo.Login.ProviderKey
            });

            if (user == null)
            {
                return RedirectToAction("ExternalLoginConfirmation", "Account",
                    new { ReturnUrl = returnUrl, LoginProvider = loginInfo.Login.LoginProvider });
            }
            else
            {
                var identity = SecurityService.CreateClaimsIdentity(user.UserName);

                AuthenticationManager.SignIn(identity);

                return RedirectToLocal(returnUrl);
            }
        }

        //
        // GET: /Account/ExternalLoginConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginConfirmation(string returnUrl, string loginProvider)
        {
            if (string.IsNullOrEmpty(loginProvider))
            {
                Context.ErrorMessage = "URL format error.";

                return View("error");
            }

            return View("external_login_confirmation");
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationFormModel formModel)
        {
            var form = GetForm(formModel.Id);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.PostedSuccessfully = true;

                    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                    if (loginInfo == null)
                    {
                        Context.ErrorMessage = "External login info was not found";

                        return View("error");
                    }

                    var user = new ApplicationUser { UserName = formModel.Email, Email = formModel.Email };
                    user.Logins = new List<UserLoginInfo>
                    {
                        new UserLoginInfo
                        {
                            LoginProvider = loginInfo.Login.LoginProvider,
                            ProviderKey = loginInfo.Login.ProviderKey
                        }
                    };

                    var result = await SecurityService.CreateUserAsync(user);

                    if (result.Succeeded)
                    {
                        form.PostedSuccessfully = true;

                        user = await SecurityService.GetUserByNameAsync(formModel.Email);

                        Context.Customer = await this.CustomerService.CreateCustomerAsync(
                            formModel.Email, formModel.Email, null, user.Id, null);

                        var identity = SecurityService.CreateClaimsIdentity(user.UserName);
                        AuthenticationManager.SignIn(identity);

                        return RedirectToLocal(formModel.ReturnUrl);
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;

                        return View("external_login_confirmation");
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    return View("external_login_confirmation");
                }
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
        }

        //
        // GET: /Account
        [HttpGet]
        public async Task<ActionResult> Index(int? skip, int? take)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                skip = skip ?? 0;
                take = take ?? 10;

                this.Context.Customer = await CustomerService.GetCustomerAsync(
                    HttpContext.User.Identity.Name, Context.StoreId);

                var orderSearchResult =
                    await
                        this.CustomerService.GetOrdersAsync(
                            this.Context.Shop.StoreId,
                            this.Context.Customer.Id,
                            null,
                            skip.Value,
                            take.Value);

                this.Context.Customer.OrdersCount = orderSearchResult.TotalCount;

                if (orderSearchResult.TotalCount > 0)
                {
                    this.Context.Customer.Orders = new List<CustomerOrder>();

                    foreach (var order in orderSearchResult.CustomerOrders)
                    {
                        this.Context.Customer.Orders.Add(order.AsWebModel());
                    }
                }

                return this.View("customers/account");
            }

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Addresses
        [HttpGet]
        public ActionResult Addresses()
        {
            foreach (var address in this.Context.Customer.Addresses)
            {
                var addressForm = new AddressForm();
                addressForm.FormContext = address;
                addressForm.FormType = "customer_address";
                addressForm.Id = address.Id;

                Context.Forms.Add(addressForm);
            }

            return View("customers/addresses");
        }

        //
        // POST: /Account/EditAddress
        [HttpPost]
        public async Task<ActionResult> EditAddress(CustomerAddressFormModel formModel, string id)
        {
            var form = GetForm(formModel.form_type);

            var customer = this.Context.Customer;
            var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == id);

            if (customerAddress != null)
            {
                customer.Addresses.Remove(customerAddress);
                customer.Addresses.Add(formModel.AsWebModel());
            }
            else
            {
                customer.Addresses.Add(formModel.AsWebModel());
            }

            await this.CustomerService.UpdateCustomerAsync(customer);

            return RedirectToAction("Addresses", "Account");
        }

        //
        // POST: /Account/Addresses
        [HttpPost]
        public async Task<ActionResult> Addresses(string id)
        {
            var address = Context.Customer.Addresses.FirstOrDefault(a => a.Id == id);

            if (address != null)
            {
                Context.Customer.Addresses.Remove(address);
                await CustomerService.UpdateCustomerAsync(Context.Customer);

                return View("customers/addresses");
            }

            return View("error");
        }

        [HttpGet]
        [Route("account/order/{id}")]
        public async Task<ActionResult> Order(string id)
        {
            this.Context.Order =
                await this.CustomerService.GetOrderAsync(this.Context.Shop.StoreId, this.Context.Customer.Email, id);

            return this.View("customers/order");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public ChallengeResult(string loginProvider, string redirectUri)
                : this(loginProvider, redirectUri, null)
            {
            }

            public ChallengeResult(string loginProvider, string redirectUri, string userId)
            {
                LoginProvider = loginProvider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (this.UserId != null)
                {
                    properties.Dictionary["XsrfId"] = this.UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
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