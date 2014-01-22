using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks.Email;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Client.Security;
using VirtoCommerce.Web.Client.Services.Emails;
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
        /// The _o authentication security
        /// </summary>
        private readonly IOAuthWebSecurity _oAuthSecurity;
        /// <summary>
        /// The _order client
        /// </summary>
        private readonly OrderClient _orderClient;
        /// <summary>
        /// The _order service
        /// </summary>
        private readonly IOrderService _orderService;

        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;

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
        private readonly IUserSecurity _webSecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="catalogClient">The catalog client.</param>
        /// <param name="userClient">The user client.</param>
        /// <param name="countryClient">The country client.</param>
        /// <param name="orderClient">The order client.</param>
        /// <param name="settingsClient">The settings client.</param>
        /// <param name="webSecurity">The web security.</param>
        /// <param name="oAuthSecurity">The o authentication security.</param>
        /// <param name="orderService">The order service.</param>
        /// <param name="templateService">The template service.</param>
        /// <param name="emailService">The email service.</param>
        public AccountController(CatalogClient catalogClient,
                                 UserClient userClient,
                                 CountryClient countryClient,
                                 OrderClient orderClient,
                                 SettingsClient settingsClient,
                                 IUserSecurity webSecurity,
                                 IOAuthWebSecurity oAuthSecurity,
                                 IOrderService orderService,
                                 ITemplateService templateService, 
                                 IEmailService emailService)
        {
            _catalogClient = catalogClient;
            _userClient = userClient;
            _countryClient = countryClient;
            _orderClient = orderClient;
            _settingsClient = settingsClient;
            _webSecurity = webSecurity;
            _oAuthSecurity = oAuthSecurity;
            _orderService = orderService;
            _templateService = templateService;
            _emailService = emailService;
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
        public ActionResult LogOnAsync(LogOnModel model, string returnUrl)
        {
            string errorMessage = null;
            if (ModelState.IsValid && _webSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                if (StoreHelper.IsUserAuthorized(model.UserName, out errorMessage))
                {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        OnPostLogon(model.UserName);
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
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            string errorMessage = null;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.ImpersonatedUserName))
            {
                    if (_webSecurity.Login(model.UserName, model.Password, model.RememberMe) && StoreHelper.IsUserAuthorized(model.UserName, out errorMessage))
                {
                    OnPostLogon(model.UserName);
                    return RedirectToLocal(returnUrl);
                }
            }
                else
                {
                    if (_webSecurity.LoginAs(model.ImpersonatedUserName, model.UserName, model.Password, out errorMessage, model.RememberMe)
                        && StoreHelper.IsUserAuthorized(model.UserName, out errorMessage) 
                        && StoreHelper.IsUserAuthorized(model.ImpersonatedUserName, out errorMessage))
                    {
                        OnPostLogon(model.ImpersonatedUserName, model.UserName);
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
            _webSecurity.Logout();
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
                        exisintgAddress.City = model.Address.City;
                        exisintgAddress.CountryCode = model.Address.CountryCode;
                        exisintgAddress.DaytimePhoneNumber = model.Address.DaytimePhoneNumber;
                        exisintgAddress.Email = model.Address.Email;
                        exisintgAddress.FaxNumber = model.Address.FaxNumber;
                        exisintgAddress.FirstName = model.Address.FirstName;
                        exisintgAddress.LastName = model.Address.LastName;
                        exisintgAddress.Line1 = model.Address.Line1;
                        exisintgAddress.Line2 = model.Address.Line2;
                        exisintgAddress.RegionName = model.Address.RegionName;
                        exisintgAddress.Name = model.Address.Name;
                        exisintgAddress.PostalCode = model.Address.PostalCode;
                        exisintgAddress.StateProvince = model.Address.StateProvince;
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

                    _userClient.SaveCustomerChanges();
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
                    _userClient.SaveCustomerChanges();
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
        public ActionResult Edit()
        {
            var contact = _userClient.GetCurrentCustomer();
            var model = UserHelper.GetCustomerModel(contact);
            var chModel = new ChangeAccountInfoModel();
            chModel.InjectFrom(model);

            chModel.FullName = chModel.FullName ?? UserHelper.CustomerSession.CustomerName;

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
        public ActionResult Edit(ChangeAccountInfoModel model)
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
                                              _webSecurity.ChangePassword(UserHelper.CustomerSession.Username, model.OldPassword,
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

                _userClient.SaveCustomerChanges();

                if (needToChangePassword)
                {
                    if (changePasswordSucceeded)
                    {
                        TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Password was succesfully changed!".Localize() };
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
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
            ch.Add(order);

            // run workflows
            ch.RunWorkflow("ShoppingCartPrepareWorkflow");

            ch.SaveChanges();

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
                ModelState.AddModelError("", "Select at least one item to return");
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
                                    select new OrderReturnItem(new LineItemModel(li, item, parentItem)))
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
                    returnItem.LineItemModel = new LineItemModel(li, item, parentItem);
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
        public ActionResult Register()
        {
            //Can be set externally from Checkout
            var model = TempData["RegisterModel"] as RegisterModel;
            return model != null ? Register(model) : View();
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
        public ActionResult RegisterAsync(RegisterModel model)
        {
            Register(model);
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
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string error;

                if (!Register(model, out error))
                {
                    ModelState.AddModelError("", error);
                }
                else
                {
                    OnPostLogon(model.Email);
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
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            var ownerAccount = _oAuthSecurity.GetUserName(provider, providerUserId);

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == UserHelper.CustomerSession.Username)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                                                     new TransactionOptions
                                                     {
                                                         IsolationLevel = IsolationLevel.Serializable
                                                     }))
                {
                    var hasLocalAccount = _oAuthSecurity.HasLocalAccount(_webSecurity.GetUserId(UserHelper.CustomerSession.Username));
                    if (hasLocalAccount || _oAuthSecurity.GetAccountsFromUserName(UserHelper.CustomerSession.Username).Count > 1)
                    {
                        _oAuthSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                    }
                }
            }

            return RedirectToAction("Index");
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
            return new ExternalLoginResult(_oAuthSecurity, provider,
                                           Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        /// <summary>
        /// External login callback.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var result =
                _oAuthSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (_oAuthSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                OnPostLogon(result.UserName);
                return RedirectToLocal(returnUrl);
            }

            if (UserHelper.CustomerSession.IsRegistered)
            {
                // If the current user is logged in add the new account
                _oAuthSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, UserHelper.CustomerSession.Username);
                return RedirectToLocal(returnUrl);
            }
            // User is new, ask for their desired membership name
            var loginData = _oAuthSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = _oAuthSecurity.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation",
                        new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
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
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;
            string providerUserId;

            if (UserHelper.CustomerSession.IsRegistered ||
                !_oAuthSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = _userClient.GetAccountByUserName(model.UserName.ToLower());

                //If user has local account then password must be correct in order to associate it with external account
                if (user != null && _oAuthSecurity.HasLocalAccount(user.AccountId.ToString()))
                {
                    if (user.StoreId != UserHelper.CustomerSession.StoreId)
                    {
                        var store = StoreHelper.StoreClient.GetStoreById(user.StoreId);
                        var storeName = store != null ? store.Name : user.StoreId;
                        ModelState.AddModelError("", string.Format("This user name is already registered with store '{0}'. Use different user name or login to store '{0}'.", storeName).Localize());
                    }
                    else if (string.IsNullOrEmpty(model.NewPassword) || !_webSecurity.Login(model.UserName, model.NewPassword))
                    {
                        ModelState.AddModelError("", "This user name is already used. Use correct password or another user name.".Localize());
                    }
                }
                else
                {
                    //If there is any extrenal account associated with given user name, then we cannot allow 
                    //associate any more external logins, because someone could steal account by mapping his own external login
                    var externalAccounts = _oAuthSecurity.GetAccountsFromUserName(model.UserName);

                    if (externalAccounts.Count > 0)
                    {
                        ModelState.AddModelError("", "This user name is already associated with external account. Use different user name.".Localize());
                    }
                    else if (model.CreateLocalLogin)
                    {
                        if (string.IsNullOrEmpty(model.NewPassword) || !model.NewPassword.Equals(model.ConfirmPassword))
                        {
                            ModelState.AddModelError("", "You must specifiy a valid password.".Localize());
                        }
                    }
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
                            UserName = model.UserName,
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
                            FullName = model.UserName
                        });
                    }
                    //Create internal login
                    if (model.CreateLocalLogin && !_oAuthSecurity.HasLocalAccount(user.AccountId.ToString()))
                    {
                        _webSecurity.CreateAccount(model.UserName, model.NewPassword);
                    }

                    //Associate external login with user or create new
                    _oAuthSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);

                    if (_oAuthSecurity.Login(provider, providerUserId, false))
                    {
                        OnPostLogon(model.UserName);
                        return RedirectToLocal(returnUrl);
                    }

                    ModelState.AddModelError("", "Failed to login".Localize());
                }
            }

            ViewBag.ProviderDisplayName = _oAuthSecurity.GetOAuthClientData(provider).DisplayName;
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
            return PartialView("_ExternalLoginsListPartial", _oAuthSecurity.RegisteredClientData);
        }

        /// <summary>
        /// View external login to remove.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var accounts = _oAuthSecurity.GetAccountsFromUserName(UserHelper.CustomerSession.Username);
            var externalLogins = (from account in accounts
                                  let clientData = _oAuthSecurity.GetOAuthClientData(account.Provider)
                                  select new ExternalLogin
                                  {
                                      Provider = account.Provider,
                                      ProviderDisplayName = clientData.DisplayName,
                                      ProviderUserId = account.ProviderUserId,
                                  }).ToList();

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 ||
                                       _oAuthSecurity.HasLocalAccount(_webSecurity.GetUserId(UserHelper.CustomerSession.Username));
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
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _userClient.GetAccountByUserName(model.UserName);

                    if (account == null)
                    {
                         TempData[GetMessageTempKey(MessageType.Error)] = new[]{"Such account does not exist in our database".Localize()};
                        return RedirectToAction("LogOn");
                    }

                    //Get reset token
                    var token = _webSecurity.GeneratePasswordResetToken(model.UserName);

                    //Collect data
                    var contact = _userClient.GetCustomer(account.MemberId);
                    var linkUrl = Url.Action("ResetPassword", "Account", new { token }, Request.Url.Scheme);
                    var userName = contact != null ? contact.FullName : model.UserName;
                    //User name can also be an email in most cases
                    var email = UserHelper.GetCustomerModel(contact).Email ?? model.UserName;

                    //Get template
                    var context = new Dictionary<string, object>() { { "ResetPasswordTemplate", new ResetPasswordTemplate{ Url = linkUrl, Username = userName} } };
                    var template = _templateService.ProcessTemplate("forgot-password", context, CultureInfo.CreateSpecificCulture(UserHelper.CustomerSession.Language));

                    //Create email message
                    var emailMessage = new EmailMessage();
                    emailMessage.To.Add(email);

                
                    if (template != null)
                    {
                        emailMessage.Html = template.Body;
                        emailMessage.Subject = template.Subject;
                    }
                    else
                    {
                        emailMessage.Html =
                            string.Format(
                                "<b>{0}</b> <br/><br/> To change your password, click on the following link:<br/> <br/> <a href='{1}'>{1}</a> <br/>",
                                userName,
                                linkUrl);

                        emailMessage.Subject = "Reset password";
                    }

                    //Send email
                    _emailService.SendEmail(emailMessage);

                    TempData[GetMessageTempKey(MessageType.Success)] = new[]{"The reset password link was generated. Check you email to reset password".Localize()};
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_webSecurity.ResetPasswordWithToken(model.Token, model.NewPassword))
                    {
                        TempData[GetMessageTempKey(MessageType.Success)] = new[] { "Your password has been succesfully changed. You can now login using new password".Localize() };
                        return RedirectToAction("LogOn");
                    }

                    ModelState.AddModelError("Token","Password reset failed. Either invalid or expired token. Please try to reset password again".Localize());
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

        #region Helpers


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
        /// After user has logged in do some actions
        /// </summary>
        private void OnPostLogon(string userName, string csrUserName = null)
        {
            var customerId = _webSecurity.GetUserId(userName);
            var contact = _userClient.GetCustomer(customerId.ToString(CultureInfo.InvariantCulture), false);

            if (!string.IsNullOrEmpty(csrUserName))
            {
                UserHelper.CustomerSession.CsrUsername = csrUserName;
            }

            if (contact != null)
            {
                var lastVisited = contact.ContactPropertyValues.FirstOrDefault(x => x.Name == ContactPropertyValueName.LastVisit);

             
                if (lastVisited != null)
                {
                    lastVisited.DateTimeValue = DateTime.UtcNow;
                }
                else
                {
                    lastVisited = new ContactPropertyValue
                        {
                            Name = ContactPropertyValueName.LastVisit,
                            DateTimeValue = DateTime.UtcNow,
                            ValueType = PropertyValueType.DateTime.GetHashCode()
                        };
                    contact.ContactPropertyValues.Add(lastVisited);
                }

                if (!string.IsNullOrEmpty(csrUserName))
                {
                    var lastVisitedByCsr = new ContactPropertyValue
                    {
                        Name = ContactPropertyValueName.LastVisitCSR,
                        DateTimeValue = DateTime.UtcNow,
                        ShortTextValue = string.Format("CSR username: {0}", csrUserName),
                        ValueType = PropertyValueType.DateTime.GetHashCode()
                    };
                    contact.ContactPropertyValues.Add(lastVisitedByCsr);
                }
                _userClient.SaveCustomerChanges();
            }
        }

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="model">The registration model.</param>
        /// <param name="errorMessage">The error message that occured during regustration.</param>
        /// <returns>true when user is registered and logged in</returns>
        private bool Register(RegisterModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var id = Guid.NewGuid().ToString();

                _webSecurity.CreateUserAndAccount(model.Email, model.Password, new
                {
                    MemberId = id,
                    UserHelper.CustomerSession.StoreId,
                    RegisterType = RegisterType.GuestUser.GetHashCode(),
                    AccountState = AccountState.Approved.GetHashCode(),
                    Discriminator = "Account"
                });

                var contact = new Contact
                {
                    MemberId = id,
                    FullName = String.Format("{0} {1}", model.FirstName, model.LastName)
                };

                contact.Emails.Add(new Email { Address = model.Email, MemberId = id, Type = EmailType.Primary.ToString() });
                foreach (var addr in model.Addresses)
                {
                    contact.Addresses.Add(addr);
                }

                _userClient.CreateContact(contact);

                return _webSecurity.Login(model.Email, model.Password);
            }
            catch (MembershipCreateUserException e)
            {
                errorMessage = ErrorCodeToString(e.StatusCode);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return false;
        }

        /// <summary>
        /// Converts error code to string.
        /// </summary>
        /// <param name="createStatus">The create status.</param>
        /// <returns>System.String.</returns>
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        /// <summary>
        /// Class ExternalLoginResult.
        /// </summary>
        internal class ExternalLoginResult : ActionResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ExternalLoginResult"/> class.
            /// </summary>
            /// <param name="security">The security.</param>
            /// <param name="provider">The provider.</param>
            /// <param name="returnUrl">The return URL.</param>
            public ExternalLoginResult(IOAuthWebSecurity security, string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
                Security = security;
            }

            /// <summary>
            /// Gets the provider.
            /// </summary>
            /// <value>The provider.</value>
            public string Provider { get; private set; }
            /// <summary>
            /// Gets the return URL.
            /// </summary>
            /// <value>The return URL.</value>
            public string ReturnUrl { get; private set; }
            /// <summary>
            /// Gets the security.
            /// </summary>
            /// <value>The security.</value>
            public IOAuthWebSecurity Security { get; private set; }

            /// <summary>
            /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
            /// </summary>
            /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
            public override void ExecuteResult(ControllerContext context)
            {
                Security.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}