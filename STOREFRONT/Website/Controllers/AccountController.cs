using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
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

        private SecurityClient _sucurityClient
        {
            get
            {
                return ClientContext.Clients.CreateSecurityClient();
            }
        }

        private ApplicationSignInManager _signInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        private ApplicationUserManager _userManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager _authenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var forms = Session["Forms"] as ICollection<SubmitForm>;

            if (forms == null)
            {
                forms = new[]
                {
                    new SubmitForm
                    {
                        ActionLink = VirtualPathUtility.ToAbsolute("~/account/login"),
                        FormType = "customer_login",
                        Id = "customer_login",
                        PasswordNeeded = true
                    },
                    new SubmitForm
                    {
                        ActionLink = VirtualPathUtility.ToAbsolute("~/account/externallogin"),
                        FormType = "external_login",
                        Id = "external_login"
                    },
                    new SubmitForm
                    {
                        ActionLink = VirtualPathUtility.ToAbsolute("~/account/forgotpassword"),
                        FormType = "recover_customer_password",
                        Id = "recover_customer_password"
                    }
                };
            }

            UpdateForms(forms);

            return View("customers/login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginFormModel formModel, string returnUrl)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.Errors = null;
                    form.PostedSuccessfully = true;

                    var loginResult = await _signInManager.PasswordSignInAsync(
                        formModel.Email, formModel.Password, false, true);

                    switch (loginResult)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("lockedout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", "Account");
                        case SignInStatus.Failure:
                        default:
                            form.Errors = new SubmitFormErrors("form", "Login attempt fails.");
                            form.PostedSuccessfully = false;
                            UpdateForms(new[] { form });
                            return View("customers/login");
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    UpdateForms(new[] { form });

                    return View("customers/login");
                }
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
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
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                Context.ErrorMessage = "External login info was not found.";

                return View("error");
            }

            var loginResult = await _signInManager.ExternalSignInAsync(loginInfo, false);

            switch (loginResult)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("lockedout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", "Account");
                case SignInStatus.Failure:
                default:
                    return RedirectToAction("ExternalLoginConfirmation", "Account",
                        new { ReturnUrl = returnUrl, LoginProvider = loginInfo.Login.LoginProvider });
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

            Session["Forms"] = null;
            
            var forms = new[]
            {
                new SubmitForm
                {
                    ActionLink = VirtualPathUtility.ToAbsolute("~/account/externalloginconfirmation"),
                    FormType = "confirm_external_login",
                    Id = "confirm_external_login",
                    Properties = {
                        { "return_url", returnUrl },
                        { "login_provider", loginProvider }
                    }
                }
            };

            UpdateForms(forms);

            return View("external_login_confirmation");
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.Errors = null;
                    form.PostedSuccessfully = true;

                    var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();

                    if (loginInfo == null)
                    {
                        Context.ErrorMessage = "External login info was not found";

                        return View("error");
                    }

                    var user = new ApplicationUser { UserName = formModel.Email, Email = formModel.Email };

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByNameAsync(formModel.Email);
                        result = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);

                        if (result.Succeeded)
                        {
                            await CustomerService.CreateCustomerAsync(
                                formModel.Email, formModel.Email, null, user.Id, null);

                            await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            return RedirectToLocal(formModel.ReturnUrl);
                        }
                        else
                        {
                            form.Errors = new SubmitFormErrors("form", result.Errors.First());
                            form.PostedSuccessfully = false;
                            UpdateForms(new[] { form });
                            return View("external_login_confirmation");
                        }
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;
                        UpdateForms(new[] { form });
                        return View("external_login_confirmation");
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    UpdateForms(new[] { form });

                    return View("external_login_confirmation");
                }
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
        }

        //
        // POST: /Accout/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.Errors = null;
                    form.PostedSuccessfully = true;

                    var user = await _userManager.FindByNameAsync(formModel.Email);

                    if (user != null)
                    {
                        var securityMessage = new VirtoCommerce.ApiClient.DataContracts.Security.SecurityMessage
                        {
                            CallbackUrl = Url.Action("ResetPassword", "Account",
                                new { UserId = user.Id, Code = "token" }, protocol: Request.Url.Scheme),
                            Language = Context.Language,
                            SendingMethod = "Email",
                            StoreId = Context.StoreId,
                            UserId = user.Id
                        };

                        UpdateForms(new[] { form });

                        await _sucurityClient.SendMessageAsync(securityMessage);
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", "User not found");
                        form.PostedSuccessfully = false;

                        UpdateForms(new[] { form });
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    UpdateForms(new[] { form });
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
            if (string.IsNullOrEmpty(code))
            {
                return View("error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Context.ErrorMessage = "User was not found.";

                return View("error");
            }

            var forms = new[]
            {
                new SubmitForm
                {
                    ActionLink = VirtualPathUtility.ToAbsolute("~/account/resetpassword"),
                    FormType = "reset_customer_password",
                    Id = "reset_customer_password",
                    PasswordNeeded = true
                }
            };

            UpdateForms(forms, true);

            Session["ResetPassword_UserId"] = userId;
            Session["ResetPassword_Token"] = code;

            return View("customers/reset_password");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                string userId = Session["ResetPassword_UserId"] as string;
                string token = Session["ResetPassword_Token"] as string;

                if (userId == null && userId == null)
                {
                    Context.ErrorMessage = "Not eonough info for reseting password";

                    return View("error");
                }

                if (formErrors == null)
                {
                    var result = await _sucurityClient.ResetPasswordAsync(userId, token, formModel.Password);

                    if (result.Succeeded)
                    {
                        Session.Remove("ResetPassword_UserId");
                        Session.Remove("ResetPassword_Token");

                        return View("password_reseted");
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;

                        UpdateForms(new[] { form });
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    UpdateForms(new[] { form });
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
            _authenticationManager.SignOut();

            return Redirect("~");
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            var forms = new[]
            {
                new SubmitForm
                {
                    ActionLink = VirtualPathUtility.ToAbsolute("~/account/register"),
                    FormType = "create_customer",
                    Id = "create_customer",
                    PasswordNeeded = true
                }
            };

            UpdateForms(forms, true);

            return View("customers/register");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.Errors = null;
                    form.PostedSuccessfully = true;

                    var user = new ApplicationUser
                    {
                        FullName = string.Format("{0} {1}", formModel.FirstName.Trim(), formModel.LastName.Trim()).Trim(),
                        Email = formModel.Email,
                        StoreId = this.Context.StoreId,
                        UserName = formModel.Email
                    };

                    var result = await _userManager.CreateAsync(user, formModel.Password);

                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByNameAsync(user.UserName);

                        this.Context.Customer = await this.CustomerService.CreateCustomerAsync(formModel.Email, formModel.FirstName, formModel.LastName, user.Id, null);

                        await _signInManager.PasswordSignInAsync(formModel.Email, formModel.Password, isPersistent: false, shouldLockout: false);

                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        form.Errors = new SubmitFormErrors("form", result.Errors.First());
                        form.PostedSuccessfully = false;

                        UpdateForms(new[] { form });
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    UpdateForms(new[] { form });
                }
            }
            else
            {
                return View("error");
            }

            return View("customers/register");
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
            var forms = new List<SubmitForm>();

            foreach (var address in this.Context.Customer.Addresses)
            {
                var addressForm = new SubmitForm();

                addressForm.ActionLink = "/Account/EditAddress?id=" + address.Id;
                addressForm.Id = address.Id;
                addressForm.FormType = "customer_address";
                addressForm.Properties["address1"] = address.Address1;
                addressForm.Properties["address2"] = address.Address2;
                addressForm.Properties["city"] = address.City;
                addressForm.Properties["company"] = address.Company;
                addressForm.Properties["country"] = address.Country;
                addressForm.Properties["country_code"] = address.CountryCode;
                addressForm.Properties["first_name"] = address.FirstName;
                addressForm.Properties["id"] = address.Id;
                addressForm.Properties["last_name"] = address.LastName;
                addressForm.Properties["phone"] = address.Phone;
                addressForm.Properties["province"] = address.Province;
                addressForm.Properties["province_code"] = address.ProvinceCode;
                addressForm.Properties["zip"] = address.Zip;

                forms.Add(addressForm);
            }

            var newAddress = new SubmitForm
            {
                ActionLink = "/Account/NewAddress",
                Id = "address_form_new",
                FormType = "customer_address"
            };

            forms.Add(newAddress);

            UpdateForms(forms.ToArray());

            return this.View("customers/addresses");
        }

        //
        // POST: /Account/NewAddress
        [HttpPost]
        public async Task<ActionResult> NewAddress(NewAddressFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            //if (!this.ModelState.IsValid)
            //{
            //    var errors = this.ModelState.Values.SelectMany(v => v.Errors);
            //    form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

            //    return this.View("customers/addresses");
            //}

            var customer = this.Context.Customer;
            customer.Addresses.Add(formModel.AsWebModel());

            await this.CustomerService.UpdateCustomerAsync(customer);

            return this.View("customers/addresses");
        }

        //
        // POST: /Account/EditAddress
        [HttpPost]
        public async Task<ActionResult> EditAddress(CustomerAddressFormModel formModel, string id)
        {
            var form = GetForm(formModel.form_type);

            //if (!this.ModelState.IsValid)
            //{
            //    var errors = this.ModelState.Values.SelectMany(v => v.Errors);
            //    form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

            //    return this.View("customers/addresses");
            //}

            var customer = this.Context.Customer;
            var customerAddress = customer.Addresses.FirstOrDefault(a => a.Id == id);

            if (customerAddress != null)
            {
                customer.Addresses.Remove(customerAddress);
                customer.Addresses.Add(formModel.AsWebModel());

                await this.CustomerService.UpdateCustomerAsync(customer);
            }

            return RedirectToAction("Addresses", "Account");
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

            return Redirect("~");
        }
    }
}