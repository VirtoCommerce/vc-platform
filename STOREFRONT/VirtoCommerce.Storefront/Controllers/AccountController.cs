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
using shopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("account")]
    [Authorize]
    public class AccountController : StorefrontControllerBase
    {
        private readonly ICommerceCoreModuleApi _commerceCoreApi;
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly IOrderModuleApi _orderApi;

        public AccountController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICommerceCoreModuleApi commerceCoreApi, ICustomerManagementModuleApi customerApi, IAuthenticationManager authenticationManager, IVirtoCommercePlatformApi platformApi, IOrderModuleApi orderApi)
            : base(workContext, urlBuilder)
        {
            _commerceCoreApi = commerceCoreApi;
            _customerApi = customerApi;
            _authenticationManager = authenticationManager;
            _platformApi = platformApi;
            _orderApi = orderApi;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index(int page = 1)
        {
            if (page < 1)
                page = 1;

            var ordersResponse = await _orderApi.OrderModuleSearchAsync(criteriaCustomerId: WorkContext.CurrentCustomer.Id);
            WorkContext.CurrentCustomer.OrdersCount = ordersResponse.TotalCount.Value;
            WorkContext.CurrentCustomer.Orders = ordersResponse.CustomerOrders.Select(o => o.ToWebModel()).ToList();
            return View("customers/account", WorkContext);
        }

        [HttpGet]
        [Route("addresses")]
        public ActionResult GetAddresses()
        {
            return View("customers/addresses", WorkContext);
        }

        [HttpPost]
        [Route("addresses/{id?}")]
        public async Task<ActionResult> UpdateAddress(string id, shopifyModel.Address formModel)
        {
            var contact = await _customerApi.CustomerModuleGetContactByIdAsync(WorkContext.CurrentCustomer.Id);
            var updateContact = false;

            if (contact != null)
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Add new address
                    contact.Addresses.Add(formModel.ToServiceModel(WorkContext.AllCountries));
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
                                contact.Addresses.RemoveAt(addressIndex - 1);
                                updateContact = true;
                            }
                            else
                            {
                                // Update address
                                contact.Addresses[addressIndex].CopyFrom(formModel, WorkContext.AllCountries);
                                updateContact = true;
                            }
                        }
                    }
                }

                if (updateContact)
                {
                    await _customerApi.CustomerModuleUpdateContactAsync(contact);
                }
            }

            return StoreFrontRedirect("~/account/addresses");
        }

        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("customers/register", WorkContext);
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

            var result = await _commerceCoreApi.StorefrontSecurityCreateAsync(user);

            if (result.Succeeded == true)
            {
                user = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(user.UserName);

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

                await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(formModel.Email, formModel.Password);

                var identity = CreateClaimsIdentity(formModel.Email);
                _authenticationManager.SignIn(identity);
                return StoreFrontRedirect("~/account");
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/register", WorkContext);
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                _authenticationManager.SignOut();
            }

            WorkContext.Login = new Login();

            return View("customers/login", WorkContext);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login formModel, string returnUrl)
        {
            var loginResult = await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(formModel.Email, formModel.Password);

            switch (loginResult.Status)
            {
                case "success":
                    var identity = CreateClaimsIdentity(formModel.Email);
                    _authenticationManager.SignIn(identity);
                    return StoreFrontRedirect(returnUrl);
                case "lockedOut":
                    return View("lockedout", WorkContext);
                case "requiresVerification":
                    return StoreFrontRedirect("~/account/sendcode");
                case "failure":
                default:
                    ModelState.AddModelError("form", "Login attempt failed.");
                    return View("customers/login", WorkContext);
            }
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            _authenticationManager.SignOut();
            return StoreFrontRedirect("~/");
        }

        [HttpPost]
        [Route("forgotpassword")]
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
        [Route("resetpassword")]
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

            var tokenCookie = new HttpCookie(StorefrontConstants.PasswordResetTokenCookie, code);
            tokenCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(tokenCookie);

            var customerIdCookie = new HttpCookie(StorefrontConstants.CustomerIdCookie, userId);
            customerIdCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(customerIdCookie);

            return View("customers/reset_password", WorkContext);
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword formModel)
        {
            var customerIdCookie = HttpContext.Request.Cookies[StorefrontConstants.CustomerIdCookie];
            string userId = customerIdCookie != null ? customerIdCookie.Value : null;

            var tokenCookie = HttpContext.Request.Cookies[StorefrontConstants.PasswordResetTokenCookie];
            string token = tokenCookie != null ? tokenCookie.Value : null;

            if (userId == null && token == null)
            {
                WorkContext.ErrorMessage = "Not enough info for reseting password";
                return View("error", WorkContext);
            }

            var result = await _commerceCoreApi.StorefrontSecurityResetPasswordAsync(userId, token, formModel.Password);

            if (result.Succeeded == true)
            {
                HttpContext.Response.Cookies.Add(new HttpCookie(StorefrontConstants.CustomerIdCookie) { Expires = DateTime.UtcNow.AddDays(-1) });
                HttpContext.Response.Cookies.Add(new HttpCookie(StorefrontConstants.PasswordResetTokenCookie) { Expires = DateTime.UtcNow.AddDays(-1) });

                return View("customers/reset_password_confirmation", WorkContext);
            }
            else
            {
                ModelState.AddModelError("form", result.Errors.First());
            }

            return View("customers/reset_password", WorkContext);
        }

        [HttpPost]
        [Route("profile")]
        public async Task<ActionResult> UpdateProfile(Profile formModel)
        {
            var contact = new VirtoCommerceCustomerModuleWebModelContact
            {
                Id = WorkContext.CurrentCustomer.Id
            };

            var fullName = string.Join(" ", formModel.FirstName, formModel.LastName).Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                fullName = formModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                contact.FullName = fullName;
            }

            if (!string.IsNullOrWhiteSpace(formModel.Email))
            {
                contact.Emails = new List<string> { formModel.Email };
            }

            await _customerApi.CustomerModuleUpdateContactAsync(contact);

            return View("customers/account", WorkContext);
        }

        [HttpPost]
        [Route("password")]
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


        private ClaimsIdentity CreateClaimsIdentity(string userName)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));

            var identity = new ClaimsIdentity(claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }
    }
}
