using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.FormModels;
using VirtoCommerce.Web.Models.Security;

namespace VirtoCommerce.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private const string XsrfKey = "XsrfId";

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return this.View("customers/login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginFormModel formModel, string returnUrl)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/login");
            }

            var result = await SignInManager.PasswordSignInAsync(
                formModel.Email, formModel.Password, isPersistent: false, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", "Account",
                        new { ReturnUrl = returnUrl, RememberMe = formModel.RememberMe });
                case SignInStatus.Failure:
                default:
                    form.Errors = new[] { "Invalid login attempt." };
                    return this.View("customers/login");
            }
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe = false)
        {
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return this.View("error");
            }

            var formModel = new VerifyCodeFormModel
            {
                Provider = provider,
                RememberMe = rememberMe,
                ReturnUrl = returnUrl
            };

            return this.View("verify-code", formModel);
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        public async Task<ActionResult> VerifyCode(VerifyCodeFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("verify-code");
            }

            var result = await SignInManager.TwoFactorSignInAsync(formModel.Provider, formModel.Code,
                isPersistent: formModel.RememberMe, rememberBrowser: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(formModel.ReturnUrl);
                case SignInStatus.LockedOut:
                    return this.View("lockout");
                case SignInStatus.Failure:
                default:
                    form.Errors = new[] { "Invalid code" };
                    return this.View("verify-code");
            }
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View("customers/register");
        }

        //
        // POST: /Accout/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/register");
            }

            var user = new ApplicationUser
            {
                FullName = string.Format("{0} {1}", formModel.FirstName.Trim(), formModel.LastName.Trim()).Trim(),
                Email = formModel.Email,
                StoreId = this.Context.StoreId,
                UserName = formModel.Email
            };

            var result = await UserManager.CreateAsync(user, formModel.Password);

            if (result.Succeeded)
            {
                if (user.TwoFactorEnabled)
                {
                    user = await UserManager.FindByNameAsync(user.UserName);

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string messageBody =
                        string.Format("Please confirm your account by clicking this <strong><a href=\"{0}\">link</a></strong>", callbackUrl);

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", messageBody);

                    return this.View("confirmation-sent");
                }
                else
                {
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                form.Errors = result.Errors.ToArray();

                return this.View("customers/register");
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.View("error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "confirmation-done" : "error");
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/login");
            }

            var user = await UserManager.FindByNameAsync(formModel.Email);

            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                return this.View("error");
            }

            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("ResetPassword", "Account",
                new { UserId = user.Id, Code = code }, protocol: Request.Url.Scheme);
            string messageBody =
                string.Format("Please reset your password by clicking <strong><a href=\"{0}\">here</a></strong>", callbackUrl);

            await UserManager.SendEmailAsync(user.Id, "Reset password", messageBody);

            return this.View("confirmation-forgot-password");
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return string.IsNullOrEmpty(code) ?
                this.View("error") :
                this.View("reset-password");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/reset_password");
            }

            var user = await UserManager.FindByNameAsync(formModel.Email);

            if (user == null)
            {
                return this.View("error");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, formModel.Code, formModel.Password);

            if (result.Succeeded)
            {
                return this.View("confirmation-reset-password");
            }
            else
            {
                return this.View("error");
            }
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(ExternalLoginFormModel formModel, string returnUrl)
        {
            return new ChallengeResult(
                formModel.AuthenticationType,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe = false)
        {
            string userId = await SignInManager.GetVerifiedUserIdAsync();

            if (userId == null)
            {
                return this.View("error");
            }

            var formModel = new SendCodeFormModel
            {
                Providers = await UserManager.GetValidTwoFactorProvidersAsync(userId),
                RememberMe = rememberMe,
                ReturnUrl = returnUrl
            };

            return this.View("send-code", formModel);
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(SendCodeFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("send-code");
            }

            if (!await SignInManager.SendTwoFactorCodeAsync(formModel.SelectedProvider))
            {
                return this.View("error");
            }
            else
            {
                return RedirectToAction("VerifyCode", "Account",
                    new { Provider = formModel.SelectedProvider, ReturnUrl = formModel.ReturnUrl, RememberMe = formModel.RememberMe });
            }
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(loginInfo.Email))
            {
                return RedirectToAction("", "Account", new { ReturnUrl = returnUrl });
            }

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    return this.View("confirmation-external-login");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationFormModel formModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            var form = this.Service.GetForm(formModel.form_type);

            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("confirmation-external-login");
            }

            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = new ApplicationUser { UserName = formModel.Email, Email = formModel.Email };

            var result = await UserManager.CreateAsync(user);

            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    form.Errors = result.Errors.ToArray();

                    return this.View("confirmation-external-login");
                }
            }
            else
            {
                form.Errors = result.Errors.ToArray();

                return this.View("confirmation-external-login");
            }
        }

        //
        // GET: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account
        [HttpGet]
        public async Task<ActionResult> Index(int? skip, int? take)
        {
            skip = skip ?? 0;
            take = take ?? 10;

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

        //
        // GET: /Account/Addresses
        [HttpGet]
        public ActionResult Addresses()
        {
            return this.View("customers/addresses");
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
                this.LoginProvider = loginProvider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };

                if (this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }

    //    [HttpGet]
    //    public ActionResult Addresses()
    //    {
    //        return this.View("customers/addresses");
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult> DeleteAddress(string id)
    //    {
    //        var customer = this.Context.Customer;
    //        var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == id);

    //        if (customerAddress != null)
    //        {
    //            customer.Addresses.Remove(customerAddress);

    //            await this.CustomerService.UpdateCustomerAsync(customer);
    //        }

    //        return this.View("customers/addresses");
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult> EditAddress(CustomerAddressFormModel formModel)
    //    {
    //        var form = this.Service.GetForm(formModel.form_type);

    //        if (!this.ModelState.IsValid)
    //        {
    //            var errors = this.ModelState.Values.SelectMany(v => v.Errors);
    //            form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

    //            return this.View("customers/addresses");
    //        }

    //        var customer = this.Context.Customer;
    //        var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == formModel.Id);

    //        if (customerAddress != null)
    //        {
    //            customerAddress = formModel.AsWebModel();
    //        }

    //        await this.CustomerService.UpdateCustomerAsync(customer);

    //        return this.View("customers/addresses");
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult> Index(int? skip, int? take)
    //    {
    //        skip = skip ?? 0;
    //        take = take ?? 20;

    //        var orderSearchResult =
    //            await
    //                this.CustomerService.GetOrdersAsync(
    //                    this.Context.Shop.StoreId,
    //                    this.Context.Customer.Id,
    //                    null,
    //                    skip.Value,
    //                    take.Value);

    //        this.Context.Customer.OrdersCount = orderSearchResult.TotalCount;

    //        if (orderSearchResult.TotalCount > 0)
    //        {
    //            this.Context.Customer.Orders = new List<CustomerOrder>();

    //            foreach (var order in orderSearchResult.CustomerOrders)
    //            {
    //                this.Context.Customer.Orders.Add(order.AsWebModel());
    //            }
    //        }

    //        return this.View("customers/account");
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult> NewAddress(NewAddressFormModel formModel)
    //    {
    //        var form = this.Service.GetForm(formModel.form_type);

    //        if (!this.ModelState.IsValid)
    //        {
    //            var errors = this.ModelState.Values.SelectMany(v => v.Errors);
    //            form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

    //            return this.View("customers/addresses");
    //        }

    //        var customer = this.Context.Customer;
    //        customer.Addresses.Add(formModel.AsWebModel());

    //        await this.CustomerService.UpdateCustomerAsync(customer);

    //        return this.View("customers/addresses");
    //    }

    //    [HttpGet]
    //    [Route("account/order/{id}")]
    //    public async Task<ActionResult> Order(string id)
    //    {
    //        this.Context.Order =
    //            await this.CustomerService.GetOrderAsync(this.Context.Shop.StoreId, this.Context.Customer.Email, id);

    //        return this.View("customers/order");
    //    }
}