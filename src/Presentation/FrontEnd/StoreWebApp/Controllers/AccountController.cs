using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Client.Security.Identity.Configs;
using VirtoCommerce.Web.Client.Services.Security;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;


namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class AccountController.
    /// </summary>
    [Authorize]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// The _catalog client
        /// </summary>
        private readonly CatalogClient _catalogClient;
        /// <summary>
        /// The _country client
        /// </summary>
        private readonly CountryClient _countryClient;
        /// <summary>
        /// The _order client
        /// </summary>
        private readonly OrderClient _orderClient;
        /// <summary>
        /// The _order service
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        /// The _settings client
        /// </summary>
        private readonly SettingsClient _settingsClient;
        /// <summary>
        /// The _user client
        /// </summary>
        private readonly UserClient _userClient;
        /// <summary>
        /// The _web security
        /// </summary>
        private readonly IdentityUserSecurity _identitySecurity;

        private IAuthenticationManager _authenticationManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="catalogClient">The catalog client.</param>
        /// <param name="userClient">The user client.</param>
        /// <param name="countryClient">The country client.</param>
        /// <param name="orderClient">The order client.</param>
        /// <param name="settingsClient">The settings client.</param>
        /// <param name="identitySecurity">The web security.</param>
        /// <param name="orderService">The order service.</param>
        public AccountController(CatalogClient catalogClient,
                                 UserClient userClient,
                                 CountryClient countryClient,
                                 OrderClient orderClient,
                                 SettingsClient settingsClient,
                                 IdentityUserSecurity identitySecurity,
                                 IOrderService orderService)
        {
            _catalogClient = catalogClient;
            _userClient = userClient;
            _countryClient = countryClient;
            _orderClient = orderClient;
            _settingsClient = settingsClient;
            _identitySecurity = identitySecurity;
            _orderService = orderService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? (_signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication);
            }
            private set
            {
                _authenticationManager = value;
            }
        }

        #region Authentication Methods

        /// <summary>
        /// Logs on.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="loginAs">Impersonate user name</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult LogOn(string returnUrl, string loginAs)
        {
            var model = new LogOnModel();
            if (!string.IsNullOrEmpty(loginAs))
            {
                model.ImpersonatedUserName = loginAs;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Logs on asynchronous.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult LogOnAsync()
        {
            return PartialView("LogOnAsync");
        }

        /// <summary>
        /// Logs on asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LogOnAsync(LogOnModel model, string returnUrl)
        {
            string errorMessage = null;
            if (ModelState.IsValid && await _identitySecurity.LoginAsync(model.UserName, model.Password, model.RememberMe) == SignInStatus.Success)
            {
                if (StoreHelper.IsUserAuthorized(model.UserName, out errorMessage))
                {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        await UserHelper.OnPostLogonAsync(model.UserName);
                        return Redirect(returnUrl);
                    }
                    var res = new JavaScriptResult { Script = "location.reload();" };
                    return res;
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", string.IsNullOrEmpty(errorMessage) ? "The user name or password provided is incorrect." : errorMessage);
            return PartialView(model);
        }

        /// <summary>
        /// Logs on.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOn(LogOnModel model, string returnUrl)
        {
            string errorMessage = null;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.ImpersonatedUserName))
                {
                    if (await _identitySecurity.LoginAsync(model.UserName, model.Password, model.RememberMe) == SignInStatus.Success && StoreHelper.IsUserAuthorized(model.UserName, out errorMessage))
                    {
                        await UserHelper.OnPostLogonAsync(model.UserName);
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    errorMessage = await _identitySecurity.LoginAsAsync(model.ImpersonatedUserName, model.UserName, model.Password, model.RememberMe);
                    if (string.IsNullOrEmpty(errorMessage)
                        && StoreHelper.IsUserAuthorized(model.UserName, out errorMessage)
                        && StoreHelper.IsUserAuthorized(model.ImpersonatedUserName, out errorMessage))
                    {
                        await UserHelper.OnPostLogonAsync(model.ImpersonatedUserName, model.UserName);
                        return RedirectToLocal(returnUrl);
                    }
                }
            }


            errorMessage = string.IsNullOrEmpty(errorMessage)
                ? "The user name or password provided is incorrect.".Localize()
                : errorMessage.Localize();
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", errorMessage);
            return View(model);
        }

        /// <summary>
        /// Logs off.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _identitySecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Address / Order Actions

        /// <summary>
        /// Address book.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult AddressBook()
        {
            var user = _userClient.GetCurrentCustomer();

            if (user == null || user.Addresses.Count == 0)
            {
                return RedirectToAction("AddressEdit");
            }

            return View(UserHelper.GetShippingBillingForCustomer(user));
        }

        /// <summary>
        /// Addresses edit.
        /// </summary>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AddressEdit(string addressId, string organizationId)
        {
            var aem = new AddressEditModel();
            var user = _userClient.GetCurrentCustomer();

            aem.OrganizationId = organizationId;

            var existingAddress = !String.IsNullOrEmpty(organizationId)
                              ? _userClient.GetCompanyAddress(addressId, organizationId)
                              : _userClient.GetUserAddress(addressId);

            if (existingAddress == null)
            {
                //fill some entries from contact
                aem.Address.Name = aem.Address.AddressId;

                if (user != null)
                {
                    //aem.Address.MemberId = user.MemberId;

                    if (String.IsNullOrEmpty(aem.OrganizationId))
                    {
                        var names = user.FullName.Split(' ');
                        if (names.Length > 0)
                        {
                            aem.Address.FirstName = names[0];
                        }
                        if (names.Length > 1)
                        {
                            aem.Address.LastName = names[1];
                        }
                    }

                    var firstOrDefault = user.Emails.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        aem.Address.Email = firstOrDefault.Address;
                    }
                }
            }
            else
            {
                aem.Address.InjectFrom(existingAddress);
            }

            aem.Countries = _countryClient.GetAllCountries();
            return View(aem);
        }

        /// <summary>
        /// Address edit post action.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult AddressEdit(AddressEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Address != null)
                {
                    var u = _userClient.GetCurrentCustomer(false);
                    Organization org = null;

                    if (!String.IsNullOrEmpty(model.OrganizationId))
                    {
                        org = _userClient.GetOrganizationById(model.OrganizationId);
                    }

                    var parent = org == null ? u : (Member)org;

                    if (model.IsDefaultBilling)
                    {
                        //remove previous default Billing
                        var address =
                            parent.Addresses.FirstOrDefault(x => x.Name.Contains(UserHelper.DefaultBilling));
                        if (address != null)
                        {
                            address.Name = address.Name.Replace(UserHelper.DefaultBilling, string.Empty);
                        }

                        model.Address.Name += UserHelper.DefaultBilling;
                    }

                    if (model.IsDefaultShipping)
                    {
                        //remove previous default Shipping
                        var address =
                            parent.Addresses.FirstOrDefault(x => x.Name.Contains(UserHelper.DefaultShipping));
                        if (address != null)
                        {
                            address.Name = address.Name.Replace(UserHelper.DefaultShipping, string.Empty);
                        }

                        model.Address.Name += UserHelper.DefaultShipping;
                    }

                    var exisintgAddress = parent.Addresses.FirstOrDefault(p => p.AddressId.Equals(model.Address.AddressId));

                    if (exisintgAddress != null)
                    {
                        exisintgAddress.InjectFrom(model.Address);
                    }
                    else
                    {
                        //create new
                        var newAddress = new Address();
                        newAddress.InjectFrom(model.Address);

                        if (parent.Addresses.Count == 0)
                        {
                            newAddress.Name += String.Format("{0}{1}", UserHelper.DefaultBilling,
                                                                UserHelper.DefaultShipping);
                        }

                        parent.Addresses.Add(newAddress);
                    }

                    _userClient.SaveCustomerChanges(u.MemberId);
                }

                return RedirectToAction(String.IsNullOrEmpty(model.OrganizationId) ? "AddressBook" : "CompanyAddressBook");
            }

            model.Countries = _countryClient.GetAllCountries();
            return View(model);
        }

        /// <summary>
        /// Address delete.
        /// </summary>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AddressDelete(string addressId, string organizationId)
        {
            if (!String.IsNullOrEmpty(addressId))
            {
                var u = _userClient.GetCurrentCustomer(false);
                var org = _userClient.GetOrganizationForCurrentUser();

                var parent = org == null ? u : (Member)org;

                var addr = parent.Addresses.FirstOrDefault(x => x.AddressId == addressId);
                if (addr != null)
                {
                    parent.Addresses.Remove(addr);
                    _userClient.SaveCustomerChanges(u.MemberId);
                }
            }

            return RedirectToAction(String.IsNullOrEmpty(organizationId) ? "AddressBook" : "CompanyAddressBook");
        }

        /// <summary>
        /// Address view.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AddressView(Address address)
        {
            return View(address);
        }

        /// <summary>
        /// Edits this account.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(bool changePassword = false)
        {
            var contact = _userClient.GetCurrentCustomer();
            var model = UserHelper.GetCustomerModel(contact);
            var chModel = new ChangeAccountInfoModel();
            chModel.InjectFrom(model);

            chModel.FullName = chModel.FullName ?? UserHelper.CustomerSession.CustomerName;
            chModel.ChangePassword = changePassword;

            return View(chModel);
        }

        /// <summary>
        /// Edits account information
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ChangeAccountInfoModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                var needToChangePassword = !String.IsNullOrEmpty(model.OldPassword) &&
                                           !String.IsNullOrEmpty(model.NewPassword);
                try
                {
                    changePasswordSucceeded = !needToChangePassword ||
                                              await _identitySecurity.ChangePasswordAsync(UserHelper.CustomerSession.Username, model.OldPassword,
                                                                          model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                var u = _userClient.GetCurrentCustomer(false) ?? _userClient.NewContact();

                u.FullName = model.FullName;

                var primaryEmail = u.Emails.FirstOrDefault(e => e.Type == EmailType.Primary.ToString());
                if (primaryEmail != null)
                {
                    primaryEmail.Address = model.Email;
                }
                else
                {
                    var newEmail = new Email
                    {
                        Address = model.Email,
                        MemberId = u.MemberId,
                        Type = EmailType.Primary.ToString()
                    };
                    u.Emails.Add(newEmail);
                }

                _userClient.SaveCustomerChanges(u.MemberId);

                if (needToChangePassword)
                {
                    if (changePasswordSucceeded)
                    {
                        TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Password was succesfully changed!".Localize() };
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", @"The current password is incorrect or the new password is invalid.");
                }
                else
                {
                    TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Your account was succesfully updated!".Localize() };
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Edit", model);
        }

        /// <summary>
        /// Account hoome page
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            var contact = _userClient.GetCurrentCustomer();
            var model = UserHelper.GetCustomerModel(contact);
            return View(model);
        }

        /// <summary>
        /// View customer orders
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Orders(int? limit)
        {
            var orders = _orderClient.GetAllCustomerOrders(UserHelper.CustomerSession.CustomerId,
                                                           UserHelper.CustomerSession.StoreId, limit);
            return View("Orders", orders != null ? orders.ToArray() : null);
        }

        /// <summary>
        /// View recent orders for current customer
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RecentOrders()
        {
            var orders = _orderClient.GetAllCustomerOrders(UserHelper.CustomerSession.CustomerId,
                                                           UserHelper.CustomerSession.StoreId);

            if ((orders == null) || (!orders.Any()))
            {
                return null;
            }
            return PartialView("RecentOrders", orders.ToArray());
        }

        /// <summary>
        /// Account information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult AccountInfo()
        {
            var model = UserHelper.GetShippingBillingForCustomer(_userClient.GetCurrentCustomer());
            return PartialView("AccountInfo", model);
        }

        /// <summary>
        /// View the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        public ActionResult ViewOrder(string orderId)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, orderId);
            if (order == null || String.IsNullOrEmpty(order.CustomerId))
            {
                throw new UnauthorizedAccessException();
            }

            return View("OrderView", order);
        }

        /// <summary>
        /// Prints the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        public ActionResult PrintOrder(string orderId)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, orderId);
            if (order == null || String.IsNullOrEmpty(order.CustomerId))
            {
                throw new UnauthorizedAccessException();
            }

            return View(order);
        }

        /// <summary>
        /// Reorders the specified order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        public ActionResult Reorder(string orderId)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, orderId);
            if (order == null || String.IsNullOrEmpty(order.CustomerId))
            {
                throw new UnauthorizedAccessException();
            }

            var ch = new CartHelper(CartHelper.CartName);
            ch.ToCart(order);

            return RedirectToAction("Index", "Checkout");
        }

        /// <summary>
        /// Order address.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult OrderAddress(Order order, string type)
        {
            if (order.OrderForms.Count == 0)
            {
                return null;
            }

            var orderAddress = OrderClient.FindAddressByName(order, type);

            return orderAddress == null ? null : PartialView("OrderAddress", orderAddress);
        }

        /// <summary>
        /// Order return.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        [Authorize]
        [HttpGet]
        public ActionResult OrderReturn(string orderId)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, orderId);

            if (order == null || String.IsNullOrEmpty(order.CustomerId))
            {
                throw new UnauthorizedAccessException();
            }

            if (!string.Equals(order.Status, OrderStatus.Completed.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Cannot return items, because order is not completed".Localize());
                return View(new OrderReturns());
            }

            var model = InitReturnsModel(order);

            return View(model);
        }

        /// <summary>
        /// Order return post action.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        [Authorize]
        [HttpPost]
        public ActionResult OrderReturn(OrderReturns model)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, model.OrderId);

            if (order == null || String.IsNullOrEmpty(order.CustomerId))
            {
                throw new UnauthorizedAccessException();
            }

            InitReturnsModel(order, model);

            //Filter validation only for selected items
            var selectedReturnItems = new List<OrderReturnItem>();
            for (var i = 0; i < model.OrderReturnItems.Count; i++)
            {
                if (model.OrderReturnItems[i].IsSelected)
                {
                    selectedReturnItems.Add(model.OrderReturnItems[i]);
                    continue;
                }
                var i1 = i;
                foreach (var k in ModelState.Keys.Where(k => k.StartsWith(string.Format("OrderReturnItems[{0}]", i1))).ToArray())
                {
                    ModelState.Remove(k);
                }
            }

            //No items to return
            if (selectedReturnItems.Count == 0)
            {
                ModelState.AddModelError("", @"Select at least one item to return");
            }

            //If Validation passed create RmaReturns
            if (ModelState.IsValid)
            {
                var request = new RmaRequest
                {
                    Comment = model.Comment,
                    OrderId = model.OrderId,
                    Status = RmaRequestStatus.AwaitingStockReturn.ToString(),
                    ReturnFromAddressId = model.ReturnFromAddressId
                };
                request.AuthorizationCode = request.RmaRequestId;

                foreach (var item in selectedReturnItems)
                {
                    var rmali = new RmaLineItem
                    {
                        LineItemId = item.LineItemId,
                        LineItem = order.OrderForms.SelectMany(of => of.LineItems).Single(li => li.LineItemId == item.LineItemId),
                        ReturnQuantity = item.Quantity
                    };

                    var rmaritem = new RmaReturnItem
                    {
                        ItemState = RmaLineItemState.AwaitingReturn.ToString(),
                        ReturnReason = item.ReturnReason,
                        //ReturnAmount = item.LineItemModel.LineItem.ListPrice
                    };

                    rmaritem.RmaLineItems.Add(rmali);
                    request.RmaReturnItems.Add(rmaritem);
                }

                order.RmaRequests.Add(request);

                //Calculate return totals
                _orderService.ExecuteWorkflow("CalculateReturnTotalsWorkflow", order);
                //Save changes
                _orderClient.SaveChanges();

                return RedirectToAction("RmaReturns", new { orderId = model.OrderId });
            }

            return View(model);
        }

        /// <summary>
        /// Initializes the returns model.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="model">The model.</param>
        /// <returns>OrderReturns.</returns>
        private OrderReturns InitReturnsModel(Order order, OrderReturns model = null)
        {
            model = model ?? new OrderReturns();

            model.OrderId = order.OrderGroupId;
            var rmaLis =
                order.RmaRequests.SelectMany(r => r.RmaReturnItems).SelectMany(r => r.RmaLineItems).ToList();

            if (model.OrderReturnItems.Count == 0)
            {
                foreach (var ori in from li in order.OrderForms.SelectMany(of => of.LineItems)
                                    let item = _catalogClient.GetItem(li.CatalogItemId)
                                    let parentItem = _catalogClient.GetItem(li.ParentCatalogItemId)
                                    where item != null && rmaLis.All(r => r.LineItemId != li.LineItemId)
                                    select new OrderReturnItem(new LineItemModel(li, item, parentItem, order.BillingCurrency)))
                {
                    model.OrderReturnItems.Add(ori);
                }
            }
            else
            {
                foreach (var returnItem in model.OrderReturnItems)
                {
                    var li =
                        order.OrderForms.SelectMany(of => of.LineItems)
                             .First(l => l.LineItemId == returnItem.LineItemId);

                    //Filter already returned items
                    if (rmaLis.Any(r => r.LineItemId == li.LineItemId))
                    {
                        continue;
                    }

                    var item = _catalogClient.GetItem(li.CatalogItemId);
                    var parentItem = _catalogClient.GetItem(li.ParentCatalogItemId);
                    returnItem.LineItemModel = new LineItemModel(li, item, parentItem, order.BillingCurrency);
                }
            }

            //Fill return Reasons
            if (OrderReturns.ReturnReasons == null || OrderReturns.ReturnReasons.Count == 0)
            {
                OrderReturns.ReturnReasons = _settingsClient.GetSettings("ReturnReasons")
                                                            .Select(r => new SelectListItem
                                                            {
                                                                Value = r.ToString(),
                                                                Text = r.ToString()
                                                            }).ToList();
            }

            //Fill address book
            model.Addresses = UserHelper.GetAllCustomerAddresses()
                                        .Select(addr => new SelectListItem
                                        {
                                            Text = addr.ToString(),
                                            Value = addr.AddressId
                                        }).ToList();

            return model;
        }

        /// <summary>
        /// Rmas the returns.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        [Authorize]
        [HttpGet]
        public ActionResult RmaReturns(string orderId)
        {
            var myOrders = new List<Order>();
            if (!string.IsNullOrWhiteSpace(orderId))
            {
                var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, orderId);

                if (order == null || String.IsNullOrEmpty(order.CustomerId))
                {
                    throw new UnauthorizedAccessException();
                }

                myOrders.Add(order);
            }
            else
            {
                var ordersQuery = _orderClient.GetAllCustomerOrders(
                    UserHelper.CustomerSession.CustomerId, UserHelper.CustomerSession.StoreId);

                if (ordersQuery != null)
                {
                    myOrders = ordersQuery.Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem").ToList();
                }
            }

            return View(myOrders.SelectMany(o => o.RmaRequests).OrderByDescending(o => o.Created));
        }

        #endregion

        #region Register Actions

        /// <summary>
        /// Registers new account.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            //Can be set externally from Checkout
            var model = TempData["RegisterModel"] as RegisterModel;
            return model != null ? await Register(model) : View();
        }

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult RegisterAsync()
        {
            return PartialView("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            await Register(model);
            return ModelState.IsValid ?
                (ActionResult)RedirectToAction("Index", "Checkout") :
                View("Register", model);
        }

        /// <summary>
        /// Register post action.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var requireConfirmation = StoreHelper.GetSettingValue("RequireAccountConfirmation", false);

                var result = await UserHelper.RegisterAsync(model, requireConfirmation);

                if (!result.IsSuccess)
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
                else if (requireConfirmation)
                {

                    var linkUrl = Url.Action("ConfirmAccount", "Account", new { result.ConfirmationToken, username = model.Email }, Request.Url.Scheme);

                    if (UserHelper.SendEmail(linkUrl, string.Format("{0} {1}", model.FirstName, model.LastName),
                        model.Email, "confirm-account",
                        emailMessage =>
                        {
                            //Use default template
                            emailMessage.Html =
                                string.Format(
                                    "<b>{0}</b> <br/><br/> To confirm your account, click on the following link:<br/> <br/> <a href='{1}'>{1}</a> <br/>",
                                    string.Format("{0} {1}", model.FirstName, model.LastName),
                                    linkUrl);

                            emailMessage.Subject = "Account confirmation";
                        }))
                    {

                        TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Your account was succesfully created. To confirm your account follow the instruction received in email.".Localize() };
                    }
                    else
                    {
                        TempData[GetMessageTempKey(MessageType.Error)] = new[] { string.Format("Failed to send confirmation email to {0}.".Localize(), model.Email) };
                    }
                    return model.ActionResult ?? RedirectToAction("LogOn");
                }
                else
                {
                    await UserHelper.OnPostLogonAsync(model.Email);
                    return model.ActionResult ?? RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        /// <summary>
        /// Disassociates the specified external login provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string provider, string providerUserId)
        {
            var ownerAccount = await UserManager.FindByIdAsync(providerUserId);

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount != null && ownerAccount.UserName == UserHelper.CustomerSession.Username)
            {
                var hasLocalAccount = await UserManager.HasPasswordAsync(ownerAccount.Id);
                var logins = await UserManager.GetLoginsAsync(ownerAccount.Id);
                if (hasLocalAccount || logins.Count > 1)
                {
                    var removeLogin = logins.FirstOrDefault(x => x.ProviderKey == provider);
                    if (removeLogin != null)
                    {
                        await UserManager.RemoveLoginAsync(ownerAccount.Id, removeLogin);
                    }

                }
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl });
        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // POST: /Account/ExternalLogin

        /// <summary>
        /// Externally login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        /// <summary>
        /// External login callback.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.ProviderDisplayName = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { Email = loginInfo.Email ?? loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        /// <summary>
        /// External login confirmation.
        /// </summary>
        /// <param name="model">The RegisterExternalLoginModel.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {

            var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (UserHelper.CustomerSession.IsRegistered || info == null)
            {
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = _userClient.GetAccountByUserName(model.Email.ToLower());
                var appUser = await UserManager.FindByNameAsync(model.Email);

                //TODO: there is no guarantee that user is connected by userName
                if (user != null && appUser == null || user == null && appUser != null ||
                    user != null && user.AccountId != appUser.Id)
                {
                    return View("ExternalLoginFailure");
                }

                //If user has local account then password must be correct in order to associate it with external account
                if (appUser != null && await UserManager.HasPasswordAsync(appUser.Id))
                {
                    if (user.StoreId != UserHelper.CustomerSession.StoreId)
                    {
                        var store = StoreHelper.StoreClient.GetStoreById(user.StoreId);
                        var storeName = store != null ? store.Name : user.StoreId;
                        ModelState.AddModelError("", string.Format("This user name is already registered with store '{0}'. Use different user name or login to store '{0}'.", storeName).Localize());
                    }
                    else if (string.IsNullOrEmpty(model.NewPassword) || await _identitySecurity.LoginAsync(model.Email, model.NewPassword) != SignInStatus.Success)
                    {
                        ModelState.AddModelError("", "This user name is already used. Use correct password or another user name.".Localize());
                    }
                }
                else if (model.CreateLocalLogin && (string.IsNullOrEmpty(model.NewPassword) || !model.NewPassword.Equals(model.ConfirmPassword)))
                {
                    ModelState.AddModelError("", "You must specifiy a valid password.".Localize());
                }

                if (ModelState.IsValid)
                {
                    // Check if user already exists
                    if (user == null)
                    {
                        var id = Guid.NewGuid().ToString();
                        user = new Account
                        {
                            MemberId = id,
                            UserName = model.Email,
                            StoreId = UserHelper.CustomerSession.StoreId,
                            RegisterType = RegisterType.GuestUser.GetHashCode(),
                            AccountState = AccountState.Approved.GetHashCode()
                        };

                        // Insert a new user into the database
                        _userClient.CreateAccount(user);

                        //Create contact
                        _userClient.CreateContact(new Contact
                        {
                            MemberId = id,
                            FullName = model.Email
                        });
                    }

                    if (appUser == null)
                    {
                        var result = await _identitySecurity.CreateAccountAsync(model.Email, model.CreateLocalLogin ? model.NewPassword : null);
                        if (result.Succeeded)
                        {
                            appUser = await UserManager.FindByNameAsync(model.Email);
                            result = await UserManager.AddLoginAsync(appUser.Id, info.Login);
                            if (result.Succeeded)
                            {
                                await SignInManager.SignInAsync(appUser, isPersistent: false, rememberBrowser: false);
                                await UserHelper.OnPostLogonAsync(model.Email);
                                return RedirectToLocal(returnUrl);
                            }
                        }
                        AddErrors(result);
                    }
                }
            }

            ViewBag.ProviderDisplayName = info.Login.LoginProvider;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        /// <summary>
        /// Externals the login failure.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// Gets external logins list.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", AuthenticationManager.GetExternalAuthenticationTypes());
        }

        /// <summary>
        /// View external login to remove.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {

            var externalLogins = new List<ExternalLogin>();
            var appUser = UserManager.FindByNameAsync(UserHelper.CustomerSession.Username).Result;
            if (appUser != null)
            {
                externalLogins = (UserManager.GetLoginsAsync(appUser.Id).Result)
                    .Select(x => new ExternalLogin
                    {
                        Provider = x.ProviderKey,
                        ProviderUserId = appUser.Id,
                        ProviderDisplayName = x.LoginProvider
                    }).ToList();
                ViewBag.ShowRemoveButton = externalLogins.Count > 1 || UserManager.HasPasswordAsync(appUser.Id).Result;

            }
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #endregion

        #region Change Account / Password actions

        /// <summary>
        /// Forgot password post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _userClient.GetAccountByUserName(model.UserName);

                    if (account == null)
                    {
                        TempData[GetMessageTempKey(MessageType.Error)] = new[] { "Such account does not exist in our database".Localize() };
                        return RedirectToAction("LogOn");
                    }

                    if (account.RegisterType == (int)RegisterType.Administrator || account.RegisterType == (int)RegisterType.SiteAdministrator)
                    {
                        //The message is tricky in purpose so that no one could guess admins username!!!
                        TempData[GetMessageTempKey(MessageType.Error)] = new[] { "Such account does not exist in our database".Localize() };
                        return RedirectToAction("LogOn");
                    }

                    //Get reset token
                    var token = await _identitySecurity.GeneratePasswordResetTokenAsync(model.UserName);

                    //Collect data
                    var contact = _userClient.GetCustomer(account.MemberId);
                    var linkUrl = Url.Action("ResetPassword", "Account", new { token }, Request.Url.Scheme);
                    var userName = contact != null ? contact.FullName : model.UserName;
                    //User name can also be an email in most cases
                    var email = UserHelper.GetCustomerModel(contact).Email ?? model.UserName;

                    UserHelper.SendEmail(linkUrl, userName, email, "forgot-password",
                    emailMessage =>
                    {
                        //Use default template
                        emailMessage.Html =
                            string.Format(
                                "<b>{0}</b> <br/><br/> To change your password, click on the following link:<br/> <br/> <a href='{1}'>{1}</a> <br/>",
                                userName,
                                linkUrl);

                        emailMessage.Subject = "Reset password";
                    });

                    TempData[GetMessageTempKey(MessageType.Success)] = new[] { "The reset password link was generated. Check you email to reset password.".Localize() };
                }
                catch (Exception ex)
                {
                    TempData[GetMessageTempKey(MessageType.Error)] = new[] { ex.Message };
                }
            }
            return RedirectToAction("LogOn");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordModel(token));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmAccount(string token, string username)
        {
            var account = _userClient.GetAccountByUserName(username, RegisterType.GuestUser);

            if (account != null)
            {
                account.AccountState = AccountState.Approved.GetHashCode();

                if (await _identitySecurity.ConfirmAccountEmailAsync(token, username))
                {
                    TempData[GetMessageTempKey(MessageType.Success)] = new[]
                {
                    "Your account was succesfully confirmed. Now you can login".Localize()
                };
                    _userClient.SaveSecurityChanges();
                    return RedirectToAction("LogOn");
                }
            }

            TempData[GetMessageTempKey(MessageType.Error)] = new[]
                {
                    "Failed to confirm account.".Localize()
                };
            return RedirectToAction("LogOn");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _identitySecurity.ResetPasswordWithTokenAsync(model.Token, model.NewPassword, model.Email))
                    {
                        TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Your password has been succesfully changed. You can now login using new password".Localize() };
                        return RedirectToAction("LogOn");
                    }

                    ModelState.AddModelError("Token", "Password reset failed. Either invalid or expired token. Please try to reset password again".Localize());
                }
                catch (Exception ex)
                {
                    TempData[GetMessageTempKey(MessageType.Error)] = new[] { ex.Message };
                }
            }

            return View(model);
        }

        #endregion

        #region Wish List Actions

        /// <summary>
        /// View wish list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult WishList()
        {
            var ch = new CartHelper(CartHelper.WishListName);
            return View(ch.CreateCartModel(true));
        }

        /// <summary>
        /// Updates the wish list.
        /// </summary>
        /// <param name="lineItems">The line items.</param>
        /// <param name="action">Action to perform</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult UpdateWishList(List<LineItemUpdateModel> lineItems, string action)
        {
            var ch = new CartHelper(CartHelper.CartName);
            var helper = new CartHelper(CartHelper.WishListName);

            if (action == UserHelper.AddToCartAction)
            {
                //add all to cart
                foreach (var lineItem in lineItems)
                {
                    var li = helper.LineItems.FirstOrDefault(item => item.LineItemId == lineItem.LineItemId);

                    if (li == null)
                    {
                        continue;
                    }

                    var catalogItem = _catalogClient.GetItem(li.CatalogItemId);
                    var parentItem = _catalogClient.GetItem(li.ParentCatalogItemId);
                    ch.AddItem(catalogItem, parentItem, lineItem.Quantity, false);
                    helper.Remove(li);

                    // If wishlist is empty, remove it from the database
                    if (helper.IsEmpty)
                    {
                        helper.Delete();
                    }
                }

                ch.SaveChanges();
            }
            else
            {
                foreach (var lineItem in lineItems)
                {
                    var li = helper.LineItems.FirstOrDefault(item => item.LineItemId == lineItem.LineItemId);
                    if (li == null)
                    {
                        continue;
                    }

                    li.Comment = lineItem.Comment;
                    li.Quantity = lineItem.Quantity;
                }
            }

            helper.SaveChanges();

            return RedirectToAction("WishList");
        }

        #endregion

        #region CompareList Actions

        /// <summary>
        /// Shows mini compare list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult MiniCompareList()
        {
            var ch = new CartHelper(CartHelper.CompareListName);
            var cm = ch.CreateCompareModel();
            return PartialView(cm);
        }

        /// <summary>
        /// Compares list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Compare()
        {
            var ch = new CartHelper(CartHelper.CompareListName);
            var cm = ch.CreateCompareModel();
            return View(cm);
        }



        #endregion

        #region Company Actions

        //public ActionResult CompanyIndex()
        //{
        //    return View(_userClient.GetOrganizationsForCurrentUser());
        //}

        //public ActionResult CompanyEdit(string companyId)
        //{
        //    var userOrg = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();
        //    if (userOrg == null)
        //        return RedirectToAction("Index");

        //    var model = new CompanyEditModel(userOrg.Name, userOrg.Description);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult CompanyEdit(CompanyEditModel o)
        //{
        //    var org = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();
        //    org.Name = o.Name;
        //    org.Description = o.Description;
        //    _userClient.SaveCustomerChanges(org.MemberId);
        //    return View();
        //}

        //public ActionResult CompanyUsers()
        //{
        //    var model = new CompanyUserListModel();
        //    model.CurrentOrganization = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();
        //    var list = new List<Account>();
        //    return View(new CompanyUserListModel());
        //}

        //public ActionResult CompanyUserNew(string userId)
        //{
        //    /*
        //    if (!String.IsNullOrEmpty(userId))
        //    {
        //        var users = UserHelper.GetUserById(userId);
        //        if (users != null)
        //        {
        //            var model = new CompanyNewUserModel(users);

        //            var list = new List<SelectListItem>();

        //            var userRoles = _userClient.GetAllMemberRoles(userId);
        //            foreach (var role in userRoles)
        //            {
        //                list.Add(new SelectListItem { Text = role.Name, Value = role.RoleId, Selected = false });
        //            }
        //            model.UserRoles = list.ToArray();
        //            var allRoles = _userClient.GetAllRoles();
        //            var list2 = allRoles.Except(userRoles).Select(x => new SelectListItem { Selected = false, Text = x.Name, Value = x.RoleId });
        //            model.AllRoles = list2;

        //            ViewData["List"] = model.AllRoles;
        //            ViewData["UserRoles"] = model.UserRoles;
        //            return View(model);
        //        }
        //    }

        //    return View();
        //     * */

        //    throw new NotImplementedException();
        //}

        //[HttpPost]
        //public ActionResult CompanyUserNew(CompanyNewUserModel model)
        //{
        //    /*
        //    if (ModelState.IsValid)
        //    {
        //        var id = Guid.NewGuid();
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus;

        //        if (!String.IsNullOrEmpty(model.Password))
        //        {
        //            var user = Membership.CreateUser(model.EMail, model.Password, model.EMail, null, null, true, id, out createStatus);

        //            // now create new user member in commerce
        //            var account = new Account();
        //            account.RegisterType = RegisterType.GuestUser.GetHashCode();
        //            account.MemberId = id.ToString();
        //            account.StoreId = UserHelper.CustomerSession.StoreId;
        //            account.AccountState = AccountState.Approved.GetHashCode();

        //            var contact = new Contact();
        //            contact.Email = model.EMail;
        //            contact.FirstName = model.FirstName;
        //            contact.LastName = model.LastName;
        //            contact.FullName = String.Format("{0} {1}", contact.FirstName, contact.LastName);

        //            var repo = UserHelper.CustomerRepository;
        //            repo.Add(contact);
        //            repo.UnitOfWork.Commit();

        //            var repo2 = UserHelper.SecurityRepository;
        //            repo2.Add(account);
        //            repo2.UnitOfWork.Commit();

        //            return RedirectToAction("CompanyUsers");
        //        }
        //        else
        //        {
        //            var u = UserHelper.GetUserById(model.UserId);
        //            var currentOrg = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();

        //            foreach (var assignment in UserHelper.SecurityRepository.RoleAssignments)
        //            {
        //                UserHelper.SecurityRepository.Remove(assignment);
        //            }

        //            foreach (string s in model.GetSelectedUserRoles)
        //            {
        //                var assignment = UserHelper.SecurityRepository.RoleAssignments.Where(x => x.RoleId == s).FirstOrDefault();
        //                if (assignment != null)
        //                {
        //                    assignment.OrganizationId = currentOrg.MemberId;
        //                    assignment.AccountId = u.Account.MemberId;
        //                }
        //            }

        //            u.Contact.Email = model.EMail;
        //            u.Contact.FirstName = model.FirstName;
        //            u.Contact.LastName = model.LastName;

        //            UserHelper.CustomerRepository.UnitOfWork.Commit();

        //            UserHelper.SecurityRepository.UnitOfWork.Commit();

        //            return RedirectToAction("CompanyUsers");
        //            //cm.User = model.CurrentUser;
        //            //todo: Edit

        //        }
        //    }

        //    return View(model);
        //     * */
        //    throw new NotImplementedException();
        //}

        //public ActionResult CompanyAddressBook()
        //{
        //    var org = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();
        //    if (org != null || org.Addresses.Count == 0)
        //        return RedirectToAction("AddressEdit", new { orgId = org.MemberId });

        //    return View(UserHelper.GetShippingBillingForOrganization(org));
        //}

        //public ActionResult CompanyOrders()
        //{
        //    /*
        //    var criteria = new OrderSearchCriteria();
        //    var org = _userClient.GetOrganizationsForCurrentUser().SingleOrDefault();
        //    var orders = new Order[] { };

        //    if (org != null)
        //    {
        //        criteria.CompanyId = org.MemberId;
        //        var groups = OrderHelper.OrderRepository.Orders.Where(x => x.CustomerName == "SampleUnitTest").ToArray();

        //        if (groups != null && groups.Count() > 0)
        //        {
        //            orders = groups.OfType<Order>().ToArray<Order>();
        //        }
        //    }

        //    return base.View("CompanyOrders", orders);
        //    */
        //    throw new NotImplementedException();
        //}
        #endregion

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// Adds the errors.
        /// </summary>
        /// <param name="result">The result.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        /// <summary>
        /// Redirects to local.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Class ExternalLoginResult.
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

    }
}