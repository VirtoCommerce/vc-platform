#region
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.FormModels;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Public Methods and Operators
        [HttpGet]
        public ActionResult Addresses()
        {
            return this.View("customers/addresses");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAddress(string id)
        {
            var customer = this.Context.Customer;
            var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == id);

            if (customerAddress != null)
            {
                customer.Addresses.Remove(customerAddress);

                await this.CustomerService.UpdateCustomerAsync(customer);
            }

            return this.View("customers/addresses");
        }

        [HttpPost]
        public async Task<ActionResult> EditAddress(CustomerAddressFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!this.ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/addresses");
            }

            var customer = this.Context.Customer;
            var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == formModel.Id);

            if (customerAddress != null)
            {
                customerAddress = formModel.AsWebModel();
            }

            await this.CustomerService.UpdateCustomerAsync(customer);

            return this.View("customers/addresses");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!this.ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/login");
            }

            return this.View("customers/login");
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? skip, int? take)
        {
            skip = skip ?? 0;
            take = take ?? 20;

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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return this.View("customers/login");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginFormModel formModel, string returnUrl)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!this.ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/login");
            }

            var loginErrors = await this.SecurityService.Login(formModel.Email, formModel.Password);

            if (loginErrors != null)
            {
                form.Errors = loginErrors;

                return this.View("customers/login");
            }

            return this.RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public ActionResult Logoff()
        {
            this.SecurityService.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> NewAddress(NewAddressFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!this.ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/addresses");
            }

            var customer = this.Context.Customer;
            customer.Addresses.Add(formModel.AsWebModel());

            await this.CustomerService.UpdateCustomerAsync(customer);

            return this.View("customers/addresses");
        }

        [HttpGet]
        [Route("account/order/{id}")]
        public async Task<ActionResult> Order(string id)
        {
            this.Context.Order =
                await this.CustomerService.GetOrderAsync(this.Context.Shop.StoreId, this.Context.Customer.Email, id);

            return this.View("customers/order");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View("customers/register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (!this.ModelState.IsValid)
            {
                var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                return this.View("customers/register");
            }

            var registrationErrors = await this.SecurityService.RegisterUser(formModel.Email, formModel.FirstName, formModel.LastName, formModel.Password, this.Context.StoreId);

            if (registrationErrors != null)
            {
                form.Errors = new[] { registrationErrors.FirstOrDefault() };

                return this.View("customers/register");
            }

            var customer = await this.CustomerService.CreateCustomerAsync(formModel.Email, formModel.FirstName, formModel.LastName, null);

            await this.SecurityService.Login(formModel.Email, formModel.Password);

            return this.RedirectToAction("Index", "Account");
        }
        #endregion

        #region Methods
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
        #endregion

        //#region Constants
        //private const string XsrfKey = "XsrfId";
        //#endregion

        //#region Fields
        //private ApplicationSignInManager _signInManager;

        //private ApplicationUserManager _userManager;
        //#endregion

        //#region Public Properties
        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return this._signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        this._signInManager = value;
        //    }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return this._userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        this._userManager = value;
        //    }
        //}
        //#endregion

        //#region Properties
        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return this.HttpContext.GetOwinContext().Authentication;
        //    }
        //}
        //#endregion

        //#region Public Methods and Operators
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return this.View("Error");
        //    }
        //    var result = await this.UserManager.ConfirmEmailAsync(userId, code);
        //    return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(
        //        provider,
        //        this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return this.RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await this.SignInManager.ExternalSignInAsync(loginInfo, false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return this.RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return this.View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            this.ViewBag.ReturnUrl = returnUrl;
        //            this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return this.View(
        //                "ExternalLoginConfirmation",
        //                new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(
        //    ExternalLoginConfirmationViewModel model,
        //    string returnUrl)
        //{
        //    if (this.User.Identity.IsAuthenticated)
        //    {
        //        return this.RedirectToAction("Index", "Manage");
        //    }

        //    if (this.ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return this.View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await this.UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await this.SignInManager.SignInAsync(user, false, false);
        //                return this.RedirectToLocal(returnUrl);
        //            }
        //        }
        //        this.AddErrors(result);
        //    }

        //    this.ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return this.View();
        //}

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return this.View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordFormModel formModel)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.UserManager.FindByNameAsync(formModel.Email);
        //        if (user == null || !(await this.UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return this.View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(formModel);
        //}

        ////
        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return this.View();
        //}

        //public async Task<ActionResult> Index(int? ordersSkip, int? ordersTake)
        //{
        //    ordersSkip = ordersSkip ?? 0;
        //    ordersTake = ordersTake ?? 20;

        //    var orderSearchResult = await this.Service.GetCustomerOrdersAsync(this.Context.Shop.StoreId, this.Context.Customer.Email, null, ordersSkip.Value, ordersTake.Value);

        //    this.Context.Customer.OrdersCount = orderSearchResult.TotalCount;

        //    if (orderSearchResult.TotalCount > 0)
        //    {
        //        this.Context.Customer.Orders = new List<CustomerOrder>();

        //        foreach (var order in orderSearchResult.CustomerOrders)
        //        {
        //            this.Context.Customer.Orders.Add(order.AsWebModel());
        //        }
        //    }

        //    return this.View("customers/account");
        //}

        //[Route("account/order/{id}")]
        //public async Task<ActionResult> Order(string id)
        //{
        //    this.Context.Order = await this.Service.GetCustomerOrderAsync(this.Context.Shop.StoreId, this.Context.Customer.Email, id);

        //    return View("customers/order");
        //}

        //public ActionResult LogOff()
        //{
        //    this.AuthenticationManager.SignOut();
        //    return this.RedirectToAction("Index", "Home");
        //}

        //[AllowAnonymous]
        //public ActionResult Login()
        //{
        //    return this.View("customers/login");
        //}

        //public ActionResult Addresses()
        //{
        //    return View("customers/addresses");
        //}

        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginFormModel formModel, string returnUrl)
        //{
        //    var form = this.Service.GetForm(formModel.form_type);

        //    if (!this.ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(v => v.Errors);
        //        form.Errors = errors.Select(e => e.ErrorMessage).ToArray();

        //        return View("customers/login");
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result =
        //        await
        //            this.SignInManager.PasswordSignInAsync(
        //                formModel.Email,
        //                formModel.Password,
        //                false,
        //                false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return this.RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return this.View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        //        case SignInStatus.Failure:
        //        default:
        //            form.Errors = new[] { "Invalid login attempt." };
        //            //ModelState.AddModelError("", "Invalid login attempt.");
        //            return this.View("customers/login");
        //    }
        //}

        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return this.View("customers/register");
        //}

        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterFormModel formModel)
        //{
        //    var form = this.Service.GetForm(formModel.form_type);

        //    if (!ModelState.IsValid)
        //    {
        //        var error = ModelState.Values.SelectMany(v => v.Errors);
        //        form.Errors = error.Select(e => e.ErrorMessage).ToArray();

        //        return View("customers/register");
        //    }

        //    var user = new ApplicationUser
        //    {
        //        UserName = formModel.Email,
        //        Email = formModel.Email,
        //        StoreId = this.Context.Shop.StoreId,
        //        FullName = string.Format("{0} {1}", formModel.FirstName, formModel.LastName)
        //    };

        //    var result = await this.UserManager.CreateAsync(user, formModel.Password);

        //    if (result.Succeeded)
        //    {
        //        await this.SignInManager.PasswordSignInAsync(user.Email, formModel.Password, false, false);

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        form.Errors = result.Errors.ToArray();

        //        return View("customers/register");
        //    }
        //}

        ////
        //// GET: /Account/ResetPassword
        //[AllowAnonymous]
        //public ActionResult ResetPassword(string code)
        //{
        //    return code == null ? this.View("Error") : this.View();
        //}

        ////
        //// POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordFormModel model)
        //{
        //    //if (!this.ModelState.IsValid)
        //    //{
        //    //    return View(model);
        //    //}
        //    //var user = await this.UserManager.FindByNameAsync(model.Email);
        //    //if (user == null)
        //    //{
        //    //    // Don't reveal that the user does not exist
        //    //    return this.RedirectToAction("ResetPasswordConfirmation", "Account");
        //    //}
        //    //var result = await this.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    //if (result.Succeeded)
        //    //{
        //    //    return this.RedirectToAction("ResetPasswordConfirmation", "Account");
        //    //}
        //    //this.AddErrors(result);
        //    //return this.View();
        //}

        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return this.View();
        //}

        ////
        //// POST: /Account/ExternalLogin

        ////
        //// GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await this.SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return this.View("Error");
        //    }
        //    var userFactors = await this.UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions =
        //        userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return
        //        this.View(
        //            new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View();
        //    }

        //    // Generate the token and send it
        //    if (!await this.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return this.View("Error");
        //    }
        //    return this.RedirectToAction(
        //        "VerifyCode",
        //        new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        //}

        //[AllowAnonymous]
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    if (!await this.SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return this.View("Error");
        //    }
        //    return
        //        this.View(
        //            new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result =
        //        await
        //            this.SignInManager.TwoFactorSignInAsync(
        //                model.Provider,
        //                model.Code,
        //                model.RememberMe,
        //                model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return this.RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return this.View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            this.ModelState.AddModelError("", "Invalid code.");
        //            return View(model);
        //    }
        //}
        //#endregion

        //#region Methods
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (this._userManager != null)
        //        {
        //            this._userManager.Dispose();
        //            this._userManager = null;
        //        }

        //        if (this._signInManager != null)
        //        {
        //            this._signInManager.Dispose();
        //            this._signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        //// Used for XSRF protection when adding external logins

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        this.ModelState.AddModelError("", error);
        //    }
        //}

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (this.Url.IsLocalUrl(returnUrl))
        //    {
        //        return this.Redirect(returnUrl);
        //    }
        //    return this.RedirectToAction("Index", "Home");
        //}
        //#endregion

        ////public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        ////{
        ////    UserManager = userManager;
        ////    SignInManager = signInManager;
        ////}

        //// GET: Account

        ////
        //// GET: /Account/ConfirmEmail

        ////
        //// GET: /Account/ExternalLoginCallback

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    #region Constructors and Destructors
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        this.LoginProvider = provider;
        //        this.RedirectUri = redirectUri;
        //        this.UserId = userId;
        //    }
        //    #endregion

        //    #region Public Properties
        //    public string LoginProvider { get; set; }

        //    public string RedirectUri { get; set; }

        //    public string UserId { get; set; }
        //    #endregion

        //    #region Public Methods and Operators
        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
        //        if (this.UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = this.UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
        //    }
        //    #endregion
        //}
    }
}