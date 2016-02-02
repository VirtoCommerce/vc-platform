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
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Cart.Services;
using CacheManager.Core;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Controllers
{
    [Authorize]
    public class AccountController : StorefrontControllerBase
    {
        private readonly ICommerceCoreModuleApi _commerceCoreApi;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICartBuilder _cartBuilder;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ICustomerService _customerService;
        private readonly IOrderModuleApi _orderApi;
        private readonly IQuoteService _quoteService;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;

        public AccountController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICommerceCoreModuleApi commerceCoreApi,
            IAuthenticationManager authenticationManager, IVirtoCommercePlatformApi platformApi,
            ICartBuilder cartBuilder, ICustomerService customerService, IOrderModuleApi orderApi, IQuoteService quoteService,
            IQuoteRequestBuilder quoteRequestBuilder, ICacheManager<object> cacheManager)
            : base(workContext, urlBuilder)
        {
            _commerceCoreApi = commerceCoreApi;
            _customerService = customerService;
            _authenticationManager = authenticationManager;
            _platformApi = platformApi;
            _cartBuilder = cartBuilder;
            _cacheManager = cacheManager;
            _orderApi = orderApi;
            _quoteService = quoteService;
            _quoteRequestBuilder = quoteRequestBuilder;
        }

        //GET: /account
        [HttpGet]
        public ActionResult GetAccount()
        {
            //Customer should be already populated in WorkContext middleware
            return View("customers/account", WorkContext);
        }

        // GET: /account/quote-requests
        [HttpGet]
        public async Task<ActionResult> QuoteRequests(int? p)
        {
            var page = p ?? 1;
            var pageSize = 10;

            var quoteRequests = await _quoteService.GetQuoteRequestsAsync(WorkContext.CurrentStore.Id, WorkContext.CurrentCustomer.Id, (page - 1) * pageSize, pageSize, null);
            WorkContext.CurrentCustomer.QuoteRequests = quoteRequests;

            return View("customers/quote-requests", WorkContext);
        }

        // GET: /account/quote-request/{number}
        [HttpGet]
        public async Task<ActionResult> QuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            WorkContext.QuoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentStore.Id, number);

            if (WorkContext.QuoteRequest == null)
            {
                return HttpNotFound();
            }

            return View("customers/quote-request", WorkContext);
        }

        // GET: /account/quote-request/{number}/edit
        [HttpGet]
        public async Task<ActionResult> EditQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            var quoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentCustomer.Id, number);
            if (quoteRequest == null)
            {
                return HttpNotFound();
            }

            quoteRequest.Tag = "actual";
            await _quoteService.UpdateQuoteRequestAsync(quoteRequest);

            RemoveCurrentQuoteRequestFromCache();

            return StoreFrontRedirect("~/quoterequest");
        }

        // GET: /account/quote-request/{number}/confirm
        [HttpGet]
        public async Task<ActionResult> ConfirmQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            var quoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentCustomer.Id, number);

            if (quoteRequest == null)
            {
                return HttpNotFound();
            }

            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);
            await _cartBuilder.FillFromQuoteRequest(quoteRequest);
            await _cartBuilder.SaveAsync();

            return StoreFrontRedirect("~/cart/checkout");
        }

        // GET: /account/quote-request/{number}/reject
        [HttpGet]
        public async Task<ActionResult> RejectQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            var quoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentCustomer.Id, number);

            if (quoteRequest == null)
            {
                return HttpNotFound();
            }

            quoteRequest.Status = "Rejected";
            await _quoteService.UpdateQuoteRequestAsync(quoteRequest);

            RemoveCurrentQuoteRequestFromCache();

            return StoreFrontRedirect("~/account/quote-requests");
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

            if (order == null || order != null && order.CustomerId != WorkContext.CurrentCustomer.Id)
            {
                return HttpNotFound();
            }

            WorkContext.Order = order.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage);
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
            var anonymousShoppingCart = WorkContext.CurrentCart;
            var anonymousQuoteRequest = WorkContext.CurrentQuoteRequest;

            var user = new VirtoCommercePlatformCoreSecurityApplicationUserExtended
            {
                Email = formModel.Email,
                Password = formModel.Password,
                UserName = formModel.Email,
                UserType = "Customer"
            };
            //Register user in VC Platform (create security account)
            var result = await _commerceCoreApi.StorefrontSecurityCreateAsync(user);

            if (result.Succeeded == true)
            {
                //Load newly created account from API
                user = await _commerceCoreApi.StorefrontSecurityGetUserByNameAsync(user.UserName);

                //Next need create corresponding Customer contact in VC Customers (CRM) module
                //Contacts and account has a same Id.
                var contact = formModel.ToWebModel();
                contact.Id = user.Id;
                contact.IsRegisteredUser = true;
                await _customerService.CreateCustomerAsync(contact);

                await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(formModel.Email, formModel.Password);

                var identity = CreateClaimsIdentity(formModel.Email, user.Id);
                _authenticationManager.SignIn(identity);

                await MergeShoppingCartsAsync(contact, anonymousShoppingCart);
                await MergeQuoteRequestsAsync(contact, anonymousQuoteRequest);

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
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login formModel, string returnUrl)
        {
            var anonymousShoppingCart = WorkContext.CurrentCart;
            var anonymousQuoteRequest = WorkContext.CurrentQuoteRequest;

            var loginResult = await _commerceCoreApi.StorefrontSecurityPasswordSignInAsync(formModel.Email, formModel.Password);

            switch (loginResult.Status)
            {
                case "success":
                    var user = await _platformApi.SecurityGetUserByNameAsync(formModel.Email);
                    var customer = await _customerService.GetCustomerByIdAsync(user.Id);
                    var identity = CreateClaimsIdentity(formModel.Email, user.Id);
                    _authenticationManager.SignIn(identity);
                    await MergeShoppingCartsAsync(customer, anonymousShoppingCart);
                    await MergeQuoteRequestsAsync(customer, anonymousQuoteRequest);
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

            var tokenCookie = new HttpCookie(StorefrontConstants.PasswordResetTokenCookie, code);
            tokenCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(tokenCookie);

            var customerIdCookie = new HttpCookie(StorefrontConstants.CustomerIdCookie, userId);
            customerIdCookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(customerIdCookie);

            return View("customers/reset_password", WorkContext);
        }

        [HttpPost]
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

        // GET: /account/json
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetCurrentCustomer()
        {
            return Json(WorkContext.CurrentCustomer, JsonRequestBehavior.AllowGet);
        }

        private async Task MergeShoppingCartsAsync(CustomerInfo customer, ShoppingCart anonymousShoppingCart)
        {
            if (anonymousShoppingCart.ItemsCount > 0)
            {
                await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, customer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);
                await _cartBuilder.MergeWithCartAsync(anonymousShoppingCart);
                await _cartBuilder.SaveAsync();
            }
        }

        private async Task MergeQuoteRequestsAsync(CustomerInfo customer, QuoteRequest anonymousQuoteRequest)
        {
            if (anonymousQuoteRequest.ItemsCount > 0)
            {
                await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(WorkContext.CurrentStore, customer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);
                await _quoteRequestBuilder.MergeWithQuoteRequest(anonymousQuoteRequest);
                await _quoteRequestBuilder.SaveAsync();
            }
        }

        private ClaimsIdentity CreateClaimsIdentity(string userName, string userId)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.Sid, userId));

            var identity = new ClaimsIdentity(claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }

        private void RemoveCurrentQuoteRequestFromCache()
        {
            var quoteRequestCacheKey = string.Format("QuoteRequest-{0}-{1}", WorkContext.CurrentStore.Id, WorkContext.CurrentCustomer.Id);
            var quoteRequestCacheRegion = "QuoteRequestRegion";
            _cacheManager.Remove(quoteRequestCacheKey, quoteRequestCacheRegion);
        }
    }
}