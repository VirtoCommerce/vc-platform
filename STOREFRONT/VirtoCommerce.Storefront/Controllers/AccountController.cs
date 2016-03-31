using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Order.Events;
using shopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.Storefront.Controllers
{
    [Authorize]
    public class AccountController : StorefrontControllerBase
    {
        private readonly ICommerceCoreModuleApi _commerceCoreApi;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICustomerService _customerService;
        private readonly IOrderModuleApi _orderApi;
        private readonly IEventPublisher<UserLoginEvent> _userLoginEventPublisher;

        public AccountController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICommerceCoreModuleApi commerceCoreApi,
            IAuthenticationManager authenticationManager, IVirtoCommercePlatformApi platformApi,
            ICustomerService customerService, IOrderModuleApi orderApi, IEventPublisher<UserLoginEvent> userLoginEventPublisher)
            : base(workContext, urlBuilder)
        {
            _commerceCoreApi = commerceCoreApi;
            _customerService = customerService;
            _authenticationManager = authenticationManager;
            _platformApi = platformApi;
            _orderApi = orderApi;
            _userLoginEventPublisher = userLoginEventPublisher;
        }

        //GET: /account
        [HttpGet]
        public ActionResult GetAccount()
        {
            //Customer should be already populated in WorkContext middle-ware
            return View("customers/account", WorkContext);
        }


        //POST: /account
        [HttpPost]
        public async Task<ActionResult> UpdateAccount(CustomerInfo customer)
        {
            customer.Id = WorkContext.CurrentCustomer.Id;

            var fullName = string.Join(" ", customer.FirstName, customer.LastName).Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                fullName = customer.Email;
            }

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                customer.FullName = fullName;
            }

            await _customerService.UpdateCustomerAsync(customer);

            WorkContext.CurrentCustomer = await _customerService.GetCustomerByIdAsync(customer.Id);
            return View("customers/account", WorkContext);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrderDetails(string number)
        {
            var order = await _orderApi.OrderModuleGetByNumberAsync(number);

            if (order == null || order.CustomerId != WorkContext.CurrentCustomer.Id)
            {
                return HttpNotFound();
            }

            WorkContext.CurrentOrder = order.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage);
            return View("customers/order", WorkContext);
        }

        [HttpGet]
        public ActionResult GetAddresses()
        {
            return View("customers/addresses", WorkContext);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAddress(string id, shopifyModel.Address formModel)
        {
            var contact = WorkContext.CurrentCustomer;
            var updateContact = false;

            if (contact != null)
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Add new address
                    contact.Addresses.Add(formModel.ToWebModel(WorkContext.AllCountries));
                    updateContact = true;
                }
                else
                {
                    int addressIndex;
                    if (int.TryParse(id, NumberStyles.Integer, CultureInfo.InvariantCulture, out addressIndex))
                    {
                        if (addressIndex > 0 && addressIndex <= contact.Addresses.Count)
                        {
                            if (string.Equals(formModel.Method, "delete", StringComparison.OrdinalIgnoreCase))
                            {
                                // Delete address
                                ((List<Address>)contact.Addresses).RemoveAt(addressIndex - 1);
                                updateContact = true;
                            }
                            else
                            {
                                // Update address
                                ((List<Address>)contact.Addresses)[addressIndex].CopyFrom(formModel, WorkContext.AllCountries);
                                updateContact = true;
                            }
                        }
                    }
                }

                if (updateContact)
                {
                    await _customerService.UpdateCustomerAsync(contact);
                }
            }

            return StoreFrontRedirect("~/account/addresses");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("customers/register", WorkContext);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(Register formModel)
        {
            var user = new VirtoCommercePlatformCoreSecurityApplicationUserExtended
            {
                Email = formModel.Email,
                Password = formModel.Password,
                UserName = formModel.Email,
                UserType = "Customer",
                StoreId = WorkContext.CurrentStore.Id,
            };
            //Register user in VC Platform (create security account)
            var result = await _commerceCoreApi.StorefrontSecurityCreateAsync(user);

            if (result.Succeeded == true)
            {
                //Load newly created account from API
                var storefrontUser = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(user.UserName);

                //Next need create corresponding Customer contact in VC Customers (CRM) module
                //Contacts and account has the same Id.
                var customer = formModel.ToWebModel();
                customer.Id = storefrontUser.Id;
                customer.UserId = storefrontUser.Id;
                customer.UserName = storefrontUser.UserName;
                customer.IsRegisteredUser = true;
                customer.AllowedStores = storefrontUser.AllowedStores;
                await _customerService.CreateCustomerAsync(customer);

                await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(storefrontUser.UserName, formModel.Password);

                var identity = CreateClaimsIdentity(customer);
                _authenticationManager.SignIn(identity);

                //Publish user login event 
                await _userLoginEventPublisher.PublishAsync(new UserLoginEvent(WorkContext, WorkContext.CurrentCustomer, customer));

                return StoreFrontRedirect("~/account");
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/register", WorkContext);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _authenticationManager.SignOut();
            }

            WorkContext.Login = new Login();
            SetUserIdForLoginOnBehalf(Response, userId);

            return View("customers/login", WorkContext);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login formModel, string returnUrl)
        {
            var loginResult = await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(formModel.Email, formModel.Password);

            if (string.Equals(loginResult.Status, "success", StringComparison.InvariantCultureIgnoreCase))
            {
                var user = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(formModel.Email);
                var customer = await GetStorefrontCustomerByUserAsync(user);

                //Check that it's login on behalf request           
                var onBehalfUserId = GetUserIdForLoginOnBehalf(Request);
                if (!string.IsNullOrEmpty(onBehalfUserId) && !string.Equals(onBehalfUserId, customer.UserId) && await _customerService.CanLoginOnBehalfAsync(WorkContext.CurrentStore.Id, customer.UserId))
                {
                    var userOnBehalf = await _commerceCoreApi.StorefrontSecurityGetUserByIdAsync(onBehalfUserId);
                    if (userOnBehalf != null)
                    {
                        var customerOnBehalf = await GetStorefrontCustomerByUserAsync(userOnBehalf);

                        customerOnBehalf.OperatorUserId = customer.UserId;
                        customerOnBehalf.OperatorUserName = customer.UserName;
                        //change the operator login on the customer login
                        customer = customerOnBehalf;
                        //Clear LoginOnBehalf cookies
                        SetUserIdForLoginOnBehalf(Response, null);
                    }
                    // TODO: Configure the reduced login expiration
                }

                //Check that current user can sing in to current store
                if (customer.AllowedStores.IsNullOrEmpty() || customer.AllowedStores.Any(x => string.Equals(x, WorkContext.CurrentStore.Id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var identity = CreateClaimsIdentity(customer);
                    _authenticationManager.SignIn(identity);


                    //Publish user login event 
                    await _userLoginEventPublisher.PublishAsync(new UserLoginEvent(WorkContext, WorkContext.CurrentCustomer, customer));
                    return StoreFrontRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("form", "User cannot login to current store.");
                }

            }

            if (string.Equals(loginResult.Status, "lockedOut", StringComparison.InvariantCultureIgnoreCase))
            {
                return View("lockedout", WorkContext);
            }

            if (string.Equals(loginResult.Status, "requiresVerification", StringComparison.InvariantCultureIgnoreCase))
            {
                return StoreFrontRedirect("~/account/sendcode");
            }

            ModelState.AddModelError("form", "Login attempt failed.");
            return View("customers/login", WorkContext);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string authType, string returnUrl)
        {
            if (string.IsNullOrEmpty(authType))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            return new ChallengeResult(authType, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            CustomerInfo customer;

            var user = await _commerceCoreApi.StorefrontSecurityGetUserByLoginAsync(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey);
            if (user != null)
            {
                customer = await GetStorefrontCustomerByUserAsync(user);
            }
            else
            {
                var newUser = new VirtoCommercePlatformCoreSecurityApplicationUserExtended
                {
                    Email = loginInfo.Email,
                    UserName = string.Join("--", loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey),
                    UserType = "Customer",
                    StoreId = WorkContext.CurrentStore.Id,
                    Logins = new List<VirtoCommercePlatformCoreSecurityApplicationUserLogin>
                    {
                        new VirtoCommercePlatformCoreSecurityApplicationUserLogin
                        {
                            LoginProvider = loginInfo.Login.LoginProvider,
                            ProviderKey = loginInfo.Login.ProviderKey
                        }
                    }
                };
                var result = await _commerceCoreApi.StorefrontSecurityCreateAsync(newUser);

                if (result.Succeeded == true)
                {
                    var storefrontUser = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(newUser.UserName);
                    await _customerService.CreateCustomerAsync(new CustomerInfo
                    {
                        Id = storefrontUser.Id,
                        UserId = storefrontUser.Id,
                        UserName = storefrontUser.UserName,
                        FullName = loginInfo.ExternalIdentity.Name,
                        IsRegisteredUser = true,
                        AllowedStores = storefrontUser.AllowedStores
                    });

                    customer = await GetStorefrontCustomerByUserAsync(storefrontUser);
                }
                else
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
                }
            }

            var identity = CreateClaimsIdentity(customer);
            _authenticationManager.SignIn(identity);

            await _userLoginEventPublisher.PublishAsync(new UserLoginEvent(WorkContext, WorkContext.CurrentCustomer, customer));

            return StoreFrontRedirect(returnUrl);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _authenticationManager.SignOut();
            return StoreFrontRedirect("~/");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPassword formModel)
        {
            var user = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(formModel.Email);

            if (user != null)
            {
                string callbackUrl = Url.Action("ResetPassword", "Account",
                    new { UserId = user.Id, Code = "token" }, protocol: Request.Url.Scheme);

                await _commerceCoreApi.StorefrontSecurityGenerateResetPasswordTokenAsync(user.Id, WorkContext.CurrentStore.Id, WorkContext.CurrentLanguage.CultureName, callbackUrl);
            }
            else
            {
                ModelState.AddModelError("form", "User not found");
            }

            return StoreFrontRedirect("~/account/login#recover");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string userId)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(userId))
            {
                WorkContext.ErrorMessage = "Error in URL format";

                return View("error", WorkContext);
            }

            var user = await _commerceCoreApi.StorefrontSecurityGetUserByIdAsync(userId);
            if (user == null)
            {
                WorkContext.ErrorMessage = "User was not found.";
                return View("error", WorkContext);
            }

            SetCookieValue(Response, StorefrontConstants.PasswordResetTokenCookie, code, new TimeSpan(1, 0, 0, 0));
            SetCookieValue(Response, StorefrontConstants.CustomerIdCookie, userId, new TimeSpan(1, 0, 0, 0));

            return View("customers/reset_password", WorkContext);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword formModel)
        {
            var userId = GetCookieValue(Request, StorefrontConstants.CustomerIdCookie);
            var token = GetCookieValue(Request, StorefrontConstants.PasswordResetTokenCookie);

            if (userId == null && token == null)
            {
                WorkContext.ErrorMessage = "Not enough info for reseting password";
                return View("error", WorkContext);
            }

            var result = await _commerceCoreApi.StorefrontSecurityResetPasswordAsync(userId, token, formModel.Password);

            if (result.Succeeded == true)
            {
                // Remove cookies
                SetCookieValue(Response, StorefrontConstants.CustomerIdCookie);
                SetCookieValue(Response, StorefrontConstants.PasswordResetTokenCookie);

                return View("customers/reset_password_confirmation", WorkContext);
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/reset_password", WorkContext);
        }


        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePassword formModel)
        {
            var changePassword = new VirtoCommercePlatformWebModelSecurityChangePasswordInfo
            {
                OldPassword = formModel.OldPassword,
                NewPassword = formModel.NewPassword,
            };

            var result = await _platformApi.SecurityChangePasswordAsync(WorkContext.CurrentCustomer.UserName, changePassword);

            if (result.Succeeded == true)
            {
                return StoreFrontRedirect("~/account");
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
                return View("customers/account", WorkContext);
            }
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

                if (!string.IsNullOrEmpty(UserId))
                {
                    properties.Dictionary["XsrfId"] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task<CustomerInfo> GetStorefrontCustomerByUserAsync(VirtoCommerceCoreModuleWebModelStorefrontUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await _customerService.GetCustomerByIdAsync(user.Id);

            // User may not have contact record
            if (result == null)
            {
                result = new CustomerInfo
                {
                    Id = user.Id,
                    IsRegisteredUser = true,
                };
            }

            result.UserId = user.Id;
            result.UserName = user.UserName;
            result.AllowedStores = user.AllowedStores;

            return result;
        }

        private static string GetUserIdForLoginOnBehalf(HttpRequestBase request)
        {
            return GetCookieValue(request, StorefrontConstants.LoginOnBehalfUserIdCookie);
        }

        private static void SetUserIdForLoginOnBehalf(HttpResponseBase response, string userId)
        {
            SetCookieValue(response, StorefrontConstants.LoginOnBehalfUserIdCookie, userId, new TimeSpan(0, 10, 0));
        }

        private static string GetCookieValue(HttpRequestBase request, string name)
        {
            var cookie = request.Cookies[name];
            var result = cookie != null ? cookie.Value : null;
            return result;
        }

        private static void SetCookieValue(HttpResponseBase response, string name, string value = null, TimeSpan? expiresAfter = null)
        {
            // Remove cookie if value is empty
            var expires = !string.IsNullOrEmpty(value) && expiresAfter != null
                ? DateTime.UtcNow.Add(expiresAfter.Value)
                : DateTime.UtcNow.AddDays(-1);

            var cookie = new HttpCookie(name, value) { Expires = expires };
            response.Cookies.Add(cookie);
        }

        private static ClaimsIdentity CreateClaimsIdentity(CustomerInfo customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.UserName),
                new Claim(ClaimTypes.NameIdentifier, customer.Id)
            };

            var identity = new ClaimsIdentity(claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            if (!customer.AllowedStores.IsNullOrEmpty())
            {
                identity.AddClaim(new Claim(StorefrontConstants.AllowedStoresClaimType, string.Join(",", customer.AllowedStores)));
            }

            if (!string.IsNullOrEmpty(customer.OperatorUserName))
            {
                identity.AddClaim(new Claim(StorefrontConstants.OperatorUserNameClaimType, customer.OperatorUserName));
            }

            if (!string.IsNullOrEmpty(customer.OperatorUserId))
            {
                identity.AddClaim(new Claim(StorefrontConstants.OperatorUserIdClaimType, customer.OperatorUserId));
            }

            return identity;
        }
    }
}
