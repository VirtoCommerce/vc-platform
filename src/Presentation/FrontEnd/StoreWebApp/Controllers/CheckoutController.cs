using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web.Mvc;
using Omu.ValueInjecter;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;
using VirtoCommerce.Web.Virto.Helpers.Payments;
using VirtoCommerce.Web.Virto.Helpers.Popup;
using AddressType = PayPal.PayPalAPIInterfaceService.Model.AddressType;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class CheckoutController.
    /// </summary>
    public class CheckoutController : ControllerBase
    {
        /// <summary>
        /// The _country client
        /// </summary>
        private readonly CountryClient _countryClient;
        /// <summary>
        /// The _order client
        /// </summary>
        private readonly OrderClient _orderClient;
        /// <summary>
        /// The _payment client
        /// </summary>
        private readonly PaymentClient _paymentClient;
        /// <summary>
        /// The _payment options
        /// </summary>
        private readonly IPaymentOption[] _paymentOptions;
        /// <summary>
        /// The _store client
        /// </summary>
        private readonly StoreClient _storeClient;
        /// <summary>
        /// The _user client
        /// </summary>
        private readonly UserClient _userClient;

        /// <summary>
        /// The _cart
        /// </summary>
        private CartHelper _cart;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController"/> class.
        /// </summary>
        /// <param name="storeClient">The store client.</param>
        /// <param name="paymentClient">The payment client.</param>
        /// <param name="userClient">The user client.</param>
        /// <param name="countryClient">The country client.</param>
        /// <param name="orderClient">The order client.</param>
        /// <param name="paymentOptions">The payment options.</param>
        public CheckoutController(StoreClient storeClient,
                                  PaymentClient paymentClient,
                                  UserClient userClient,
                                  CountryClient countryClient,
                                  OrderClient orderClient,
                                  IPaymentOption[] paymentOptions)
        {
            _storeClient = storeClient;
            _paymentClient = paymentClient;
            _userClient = userClient;
            _countryClient = countryClient;
            _orderClient = orderClient;
            _paymentOptions = paymentOptions;
        }

        /// <summary>
        /// Gets the cart helper.
        /// </summary>
        /// <value>The ch.</value>
        private CartHelper Ch
        {
            get { return _cart ?? (_cart = new CartHelper(CartHelper.CartName)); }
        }

        /// <summary>
        /// Checkout home page
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            if (Ch.IsEmpty)
                return RedirectToAction("Index", "Cart");

            var model = PrepareCheckoutModel(new CheckoutModel());

            return View(model);
        }

        /// <summary>
        /// Displays the cart.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult DisplayCart()
        {
            return PartialView("Cart", Ch.CreateCartModel(true));
        }

        /// <summary>
        /// Displays the payments.
        /// </summary>
        /// <param name="shippingMethod">The shipping method.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult DisplayPayments(string shippingMethod)
        {
            var payments = new PaymentsModel { Payments = GetPayments(shippingMethod).ToArray() };
            return PartialView("Payments", payments);
        }

        /// <summary>
        /// Displays the shipments.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult DisplayShipments()
        {

            var model = new ShipmentsModel { Shipments = GetShipinngMethodModels() };
            return PartialView("Shipments", model);
        }


        /// <summary>
        /// Submits the changes.
        /// </summary>
        /// <param name="checkoutModel">The checkout model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult SubmitChanges(CheckoutModel checkoutModel)
        {
            RecalculateCart(checkoutModel);
            return DisplayCart();
        }


        /// <summary>
        /// Processes the checkout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        [HttpGet]
        public ActionResult ProcessCheckout(string id)
        {
            var order = _orderClient.GetCustomerOrder(UserHelper.CustomerSession.CustomerId, id);
            if (order == null || String.IsNullOrEmpty(order.CustomerId))
                throw new UnauthorizedAccessException();
            return View("Success", order);
        }

        /// <summary>
        /// Processes the checkout.
        /// </summary>
        /// <param name="checkoutModel">The checkout model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ProcessCheckout(CheckoutModel checkoutModel)
        {
            //Need to submit changes again to make sure cart is still valid
            SubmitChanges(checkoutModel);

            if (!ModelState.IsValid)
            {
                return View("Index", checkoutModel);
            }

            if (checkoutModel.PaymentMethod.Equals("PayPal", StringComparison.OrdinalIgnoreCase))
            {
                return PaypalExpress(checkoutModel, SolutionTypeType.MARK);
            }


            if (UserHelper.CustomerSession.IsRegistered)
            {
                var orgs = _userClient.GetOrganizationsForCurrentUser();
                if (orgs != null)
                {
                    var org = orgs.SingleOrDefault();
                    if (org != null)
                        Ch.Cart.OrganizationId = org.MemberId;
                }

                var user = _userClient.GetCurrentCustomer(false);


                // Save addresses to customer address book
                if (checkoutModel.AddressBook.SaveBillingAddress)
                {
                    var billing = ConvertToCustomerAddress(checkoutModel.BillingAddress.Address);
                    billing.AddressId = Guid.NewGuid().ToString();
                    user.Addresses.Add(billing);
                }
                if (checkoutModel.AddressBook.SaveShippingAddress)
                {
                    var shipping = ConvertToCustomerAddress(checkoutModel.ShippingAddress.Address);
                    shipping.AddressId = Guid.NewGuid().ToString();
                    user.Addresses.Add(shipping);
                }

                //Save last ordered date&time to customer profile
                var lastOrdered = user.ContactPropertyValues.FirstOrDefault(x => x.Name == ContactPropertyValueName.LastOrder);

                if (lastOrdered != null)
                {
                    lastOrdered.DateTimeValue = DateTime.UtcNow;
                }
                else
                {
                    user.ContactPropertyValues.Add(new ContactPropertyValue
                    {
                        DateTimeValue = DateTime.UtcNow,
                        Name = ContactPropertyValueName.LastOrder,
                        ValueType = PropertyValueType.DateTime.GetHashCode()
                    });
                }

                _userClient.SaveCustomerChanges();
            }
            else if (checkoutModel.CreateAccount)
            {
                var regModel = new RegisterModel();

                regModel.InjectFrom(checkoutModel, checkoutModel.BillingAddress.Address);

                //Save billing address to book
                var billing = ConvertToCustomerAddress(checkoutModel.BillingAddress.Address);
                billing.AddressId = Guid.NewGuid().ToString();
                regModel.Addresses.Add(billing);

                //save shipping address to book
                if (!checkoutModel.UseForShipping)
                {
                    var shipping = ConvertToCustomerAddress(checkoutModel.ShippingAddress.Address);
                    shipping.AddressId = Guid.NewGuid().ToString();
                    regModel.Addresses.Add(shipping);
                }

                string message;

                if (!UserHelper.Register(regModel, out message))
                {
                    ModelState.AddModelError("", message);
                    return View("Index", checkoutModel);
                }

                UserHelper.OnPostLogon(regModel.Email);
            }

            if (DoCheckout())
            {
                return RedirectToAction("ProcessCheckout", "Checkout", new { id = Ch.Cart.OrderGroupId });
            }


            return View("Index", checkoutModel);
        }

        public ActionResult PaypalExpress(CheckoutModel model, SolutionTypeType solutionType)
        {
            // Create request object
            var request = new SetExpressCheckoutRequestType();
            var ecDetails = new SetExpressCheckoutRequestDetailsType
            {
                CallbackTimeout = "3",
                ReturnURL = Url.Action("PaypalExpressSuccess", "Checkout", null, "http"),
                CancelURL = Url.Action("Index", "Checkout", null, "http"),
                SolutionType = solutionType
            };

            var currency = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), Ch.CustomerSession.Currency);
            model = PrepareCheckoutModel(model);
            model.Payments = model.Payments ?? GetPayments().ToArray();
            var payment = _paymentClient.GetPaymentMethod(model.PaymentMethod ?? "Paypal");
            var configMap = payment.CreateSettings();

            //Create Shipping methods
            var shippingMethods = GetShipinngMethodModels();
            var noShippingMethod = !shippingMethods.Any(x => x.IsCurrent);
            string currentShippingOption = null;

            for (var i = 0; i < shippingMethods.Length; i++)
            {
                var shipping = shippingMethods[i];
                var shippingOptionIsDefault = "0";
                if (shipping.IsCurrent || noShippingMethod && i == 0)
                {
                    shippingOptionIsDefault = "1";
                    currentShippingOption = shipping.Method.Name;
                }

                ecDetails.FlatRateShippingOptions.Add(new ShippingOptionType
                {
                    ShippingOptionAmount = new BasicAmountType(currency, FormatMoney(shipping.Price)),
                    ShippingOptionIsDefault = shippingOptionIsDefault,
                    ShippingOptionName = shipping.Method.Name,
                });
            }

            if (noShippingMethod)
            {
                model.ShippingMethod = currentShippingOption;
                //Must recalculate cart as prices could have changed
            }

            if (string.IsNullOrEmpty(model.PaymentMethod))
            {
                model.PaymentMethod = payment.Name;
            }

            RecalculateCart(model);

            // (Optional) Email address of the buyer as entered during checkout. PayPal uses this value to pre-fill the PayPal membership sign-up portion on the PayPal pages.
            if (model.BillingAddress != null && !string.IsNullOrEmpty(model.BillingAddress.Address.Email))
            {
                ecDetails.BuyerEmail = model.BillingAddress.Address.Email;
            }

            ecDetails.NoShipping = "2";

            //paymentDetails.OrderDescription = Ch.Cart.Name;

            AddressModel modelAddress = null;

            if (!model.UseForShipping && model.ShippingAddress != null && DoValidateModel(model.ShippingAddress.Address))
            {
                modelAddress = model.ShippingAddress.Address;
            }
            else if (model.BillingAddress != null && DoValidateModel(model.BillingAddress.Address))
            {
                modelAddress = model.BillingAddress.Address;
            }

            if (modelAddress != null)
            {
                ecDetails.AddressOverride = "1";

                var shipAddress = new AddressType
                {
                    Name = string.Format("{0} {1}", modelAddress.FirstName, modelAddress.LastName),
                    Street1 = modelAddress.Line1,
                    Street2 = modelAddress.Line2,
                    CityName = modelAddress.City,
                    StateOrProvince = modelAddress.StateProvince,
                    Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), modelAddress.CountryCode.Substring(0, 2)),
                    PostalCode = modelAddress.PostalCode,
                    Phone = modelAddress.DaytimePhoneNumber
                };
                ecDetails.PaymentDetails[0].ShipToAddress = shipAddress;
            }

            ecDetails.PaymentDetails.Add(GetPaypalPaymentDetail(currency, PaymentActionCodeType.SALE));
            ecDetails.MaxAmount = new BasicAmountType(currency, FormatMoney(Ch.Cart.Total));
            ecDetails.LocaleCode = new RegionInfo(Thread.CurrentThread.CurrentUICulture.LCID).TwoLetterISORegionName;
            request.SetExpressCheckoutRequestDetails = ecDetails;


            // Invoke the API
            var wrapper = new SetExpressCheckoutReq { SetExpressCheckoutRequest = request };

            // Create the PayPalAPIInterfaceServiceService service object to make the API call
            var service = new PayPalAPIInterfaceServiceService(configMap);
            var setEcResponse = service.SetExpressCheckout(wrapper);

            // Check for API return status
            if (setEcResponse.Ack.Equals(AckCodeType.FAILURE) ||
               (setEcResponse.Errors != null && setEcResponse.Errors.Count > 0))
            {
                ModelState.AddModelError("", @"Paypal failure");

                foreach (var error in setEcResponse.Errors)
                {
                    ModelState.AddModelError("", error.LongMessage);
                }
            }
            else
            {
                var redirectUrl = string.Format(configMap.ContainsKey("URL") ? configMap["URL"] : "https://www.sandbox.paypal.com/webscr&amp;cmd={0}", "_express-checkout&token=" + setEcResponse.Token);
                Session["checkout_" + setEcResponse.Token] = model;
                return Redirect(redirectUrl);
            }

            return View("Index", model);
        }

        public ActionResult PaypalExpressSuccess(string token, string payerID)
        {

            var model = (CheckoutModel)Session["checkout_" + token] ?? PrepareCheckoutModel(new CheckoutModel());
            model.Payments = model.Payments ?? GetPayments().ToArray();

            var payment = _paymentClient.GetPaymentMethod(model.PaymentMethod ?? "Paypal");
            var configMap = payment.CreateSettings();

            var service = new PayPalAPIInterfaceServiceService(configMap);

            var getEcWrapper = new GetExpressCheckoutDetailsReq
            {
                GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token)
            };
            var getEcResponse = service.GetExpressCheckoutDetails(getEcWrapper);

            if (getEcResponse.Ack.Equals(AckCodeType.FAILURE) ||
                (getEcResponse.Errors != null && getEcResponse.Errors.Count > 0))
            {
                ModelState.AddModelError("", @"Paypal failure");
                foreach (var error in getEcResponse.Errors)
                {
                    ModelState.AddModelError("", error.LongMessage);
                }
            }
            else
            {

                var details = getEcResponse.GetExpressCheckoutDetailsResponseDetails;
                var paymentDetails = details.PaymentDetails[0];
                var orderId = HttpContext.Session != null ? HttpContext.Session["LatestOrderId"] : Ch.Cart.OrderGroupId;

                if (details.CheckoutStatus.Equals("PaymentActionCompleted", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ProcessCheckout", "Checkout", new { id = orderId });
                }

                model.PaymentMethod = payment.Name;
                model.ShippingMethod = details.UserSelectedOptions.ShippingOptionName;
                model.BillingAddress.Address = ConvertToPaypalAddress(paymentDetails.ShipToAddress, "Billing");
                model.BillingAddress.Address.Email = details.PayerInfo.Payer;
                model.ShippingAddress.Address = ConvertToPaypalAddress(paymentDetails.ShipToAddress, "Shipping");
                model.ShippingAddress.Address.Email = details.PayerInfo.Payer;

                Ch.Reset();
                UpdateCart(model, true);
                Ch.RunWorkflow("ShoppingCartPrepareWorkflow");

                var cartPayment =
                    Ch.OrderForm.Payments.FirstOrDefault(
                        x => x.PaymentMethodName.Equals(payment.Name, StringComparison.OrdinalIgnoreCase));

                if (cartPayment == null)
                {
                    ModelState.AddModelError("", @"Shopping cart failure!");
                }
                else if (ModelState.IsValid)
                {
                    cartPayment.ContractId = payerID;
                    cartPayment.AuthorizationCode = token;
                    cartPayment.Amount = Ch.Cart.Total;

                    if (decimal.Parse(paymentDetails.OrderTotal.value) != Ch.Cart.Total)
                    {
                        ModelState.AddModelError("", @"Paypal payment total does not match cart total!");
                    }

                    Ch.SaveChanges();

                    if (DoCheckout())
                    {
                        return RedirectToAction("ProcessCheckout", "Checkout", new { id = orderId });
                    }
                }

            }

            return View("Index", model);

        }

        private PaymentDetailsType GetPaypalPaymentDetail(CurrencyCodeType currency, PaymentActionCodeType paymentAction)
        {
            var paymentDetails = new PaymentDetailsType { PaymentAction = paymentAction };
            paymentDetails.PaymentDetailsItem.AddRange(GetPaypalPaymentDetailsItemTypes(currency));
            paymentDetails.ShippingTotal = new BasicAmountType(currency, FormatMoney(Ch.Cart.ShippingTotal));
            paymentDetails.HandlingTotal = new BasicAmountType(currency, FormatMoney(Ch.Cart.HandlingTotal));
            paymentDetails.TaxTotal = new BasicAmountType(currency, FormatMoney(Ch.Cart.TaxTotal));
            paymentDetails.OrderTotal = new BasicAmountType(currency, FormatMoney(Ch.Cart.Total));
            paymentDetails.ItemTotal = new BasicAmountType(currency, FormatMoney(Ch.LineItems.Sum(x => x.ExtendedPrice)));
            return paymentDetails;
        }

        private string FormatMoney(decimal amount)
        {
            return amount.ToString("F2", new CultureInfo("en-US"));
        }

        private IEnumerable<PaymentDetailsItemType> GetPaypalPaymentDetailsItemTypes(CurrencyCodeType currency)
        {
            var detais = new List<PaymentDetailsItemType>();

            foreach (var li in Ch.LineItems)
            {
                var itemDetails = new PaymentDetailsItemType
                {
                    Name = li.DisplayName,
                    Amount = new BasicAmountType(currency, FormatMoney(li.PlacedPrice)),
                    Quantity = (int)li.Quantity,
                    ItemCategory = ItemCategoryType.PHYSICAL,
                    Tax = new BasicAmountType(currency, FormatMoney(li.TaxTotal)),
                    Description = li.Description,
                    Number = li.CatalogItemCode,
                    ItemURL = Url.ItemUrl(li.CatalogItemId, li.ParentCatalogItemId)
                };

                detais.Add(itemDetails);
            }

            var discount = Ch.Cart.OrderForms.Sum(c => c.DiscountAmount)
                + Ch.Cart.OrderForms.SelectMany(c => c.LineItems).Sum(c => c.LineItemDiscountAmount)
                + Ch.Cart.OrderForms.SelectMany(c => c.Shipments).Sum(c => c.ShippingDiscountAmount);

            if (discount > 0)
            {
                var itemDetails = new PaymentDetailsItemType
                {
                    Name = "Discounts",
                    Amount = new BasicAmountType(currency, FormatMoney(-discount)),
                    Quantity = 1,
                    ItemCategory = ItemCategoryType.PHYSICAL,
                    Description = "Discounts applied",
                    PromoCode = Ch.CustomerSession.CouponCode
                };

                detais.Add(itemDetails);
            }

            return detais;
        }

        private void RecalculateCart(CheckoutModel checkoutModel)
        {
            // reset the cart
            Ch.Reset();

            // Update cart from the post back
            UpdateCart(checkoutModel);

            // run workflow
            Ch.RunWorkflow("ShoppingCartPrepareWorkflow");

            // save changes
            Ch.SaveChanges();
        }

        private bool DoCheckout()
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    // run business rules
                    Ch.RunWorkflow("ShoppingCartCheckoutWorkflow");
                    // Create order
                    var order = Ch.SaveAsOrder();
                    if (HttpContext.Session != null)
                    {
                        HttpContext.Session["LatestOrderId"] = order.OrderGroupId;
                    }
                    transaction.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return false;
            }
        }

        private ShippingMethodModel[] GetShipinngMethodModels()
        {
            var store = _storeClient.GetCurrentStore();

            //Get shipping mentods avaialble in current store
            var storeShippingMethods = Ch.ShippingClient.GetAllShippingMethods()
                      .Where(sm => sm.PaymentMethodShippingMethods.Select(x => x.PaymentMethod)
                           .Any(pm => store.PaymentGateways.Any(pg => pg.PaymentGateway == pm.Name)))
                           .Select(s => s.ShippingMethodId).ToList();

            return Ch.GetShippingMethods(storeShippingMethods);
        }

        /// <summary>
        /// Updates the cart.
        /// </summary>
        /// <param name="model">The checkout model.</param>
        /// <param name="isFinalStep">if set to <c>true</c> [is final step].</param>
        private void UpdateCart(CheckoutModel model, bool isFinalStep = false)
        {
            PrepareCheckoutModel(model);

            var errors = new List<string>();

            var form = Ch.OrderForm;

            #region Process billing address

            var billingAddress = Ch.FindAddressByName("Billing");
            if (billingAddress == null)
            {
                billingAddress = new OrderAddress();
                Ch.Cart.OrderAddresses.Add(billingAddress);
            }

            var orderAddressId = billingAddress.OrderAddressId;

            // if not empty, then we use existing customer address
            if (!String.IsNullOrEmpty(model.AddressBook.BillingAddressId))
            {
                // load address from the customer address book
                var userAddress = _userClient.GetUserAddress(model.AddressBook.BillingAddressId);
                if (userAddress != null)
                {
                    billingAddress.InjectFrom(userAddress);
                }
            }
            else
            {
                billingAddress.InjectFrom(
                    new IgnorePropertiesInjection("OrderGroupId", "OrderGroup", "OrderAddressId", "Created"),
                    model.BillingAddress.Address);
            }

            billingAddress.Name = "Billing";
            billingAddress.OrderAddressId = orderAddressId;
            model.BillingAddress.Address = ConvertToAddressModel(billingAddress);

            var hasValidBillingAddress = DoValidateModel(billingAddress);
            if (hasValidBillingAddress)
            {
                Ch.Cart.AddressId = orderAddressId;
                Ch.OrderForm.BillingAddressId = orderAddressId;
            }
            else
            {
                //Remove invalid address from cart if not final step
                //otherwise saving cart will fail
                if (!isFinalStep)
                {
                    Ch.Cart.OrderAddresses.Remove(billingAddress);
                }

                errors.Add("Billing address is invalid");
            }

            #endregion

            #region Process shipping address

            var shippingAddress = Ch.FindAddressByName("Shipping");
            if (shippingAddress == null)
            {
                shippingAddress = new OrderAddress();
                Ch.Cart.OrderAddresses.Add(shippingAddress);
            }

            var shippingAddressId = shippingAddress.OrderAddressId;

            if (model.UseForShipping)
            {
                shippingAddress.InjectFrom(billingAddress);
            }
            else if (!String.IsNullOrEmpty(model.AddressBook.ShippingAddressId))
            {
                // load address from the customer address book
                var userAddress = _userClient.GetUserAddress(model.AddressBook.ShippingAddressId);
                if (userAddress != null)
                {
                    shippingAddress.InjectFrom(userAddress);
                }
            }
            else
            {
                shippingAddress.InjectFrom(
                    new IgnorePropertiesInjection("OrderGroupId", "OrderGroup", "OrderAddressId", "Created"),
                    model.ShippingAddress.Address);
            }

            shippingAddress.Name = "Shipping";
            shippingAddress.OrderAddressId = shippingAddressId;
            model.ShippingAddress.Address = ConvertToAddressModel(shippingAddress);

            var hasValidShippingAddress = DoValidateModel(shippingAddress);
            if (!hasValidShippingAddress)
            {
                //Remove invalid address from cart if not final step
                //otherwise saving cart will fail
                if (!isFinalStep)
                {
                    Ch.Cart.OrderAddresses.Remove(shippingAddress);
                }

                errors.Add("Shipping address is invalid");
            }

            #endregion

            #region Update payment info

            var paymentmethod = model.PaymentMethod;
            var paymentCreated = false;

            if (!String.IsNullOrEmpty(paymentmethod))
            {
                var paymentModel = model.Payments.Single(p => p.Name.Equals(paymentmethod, StringComparison.OrdinalIgnoreCase));
                var payment = CreatePayment(form, paymentmethod, paymentModel);

                form.Payments.Clear();

                if (DoValidateModel(payment))
                {
                    form.Payments.Add(payment);
                    paymentCreated = true;
                }
            }

            if (!paymentCreated)
            {
                errors.Add("Failed to create payment");
            }

            #endregion

            #region Update shipment delivery method

            if (!string.IsNullOrEmpty(model.ShippingMethod))
            {
                foreach (var lineItem in form.LineItems)
                {
                    var shippingMethod = Ch.GetShippingMethods(new List<string> { model.ShippingMethod }).First();
                    lineItem.ShippingMethodName = shippingMethod.DisplayName;
                    lineItem.ShippingMethodId = shippingMethod.Id;
                }
            }

            #endregion

            #region Update Shipment ShipmentAddressId

            //If ShipmentSplitActivity is not called ShipmentAddressId must be updated manually
            if (hasValidShippingAddress)
            {
                foreach (var shipment in form.Shipments)
                {
                    shipment.ShippingAddressId = model.ShippingAddress.Address.AddressId;
                }
                foreach (var lineItem in form.LineItems)
                {
                    lineItem.ShippingAddressId = model.ShippingAddress.Address.AddressId;
                }
            }

            #endregion

            foreach (var err in errors)
            {
                ModelState.AddModelError("", err);
            }
        }

        /// <summary>
        /// Creates the payment.
        /// </summary>
        /// <param name="form">The order form.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="model">The payment  model.</param>
        /// <returns>Payment.</returns>
        private Payment CreatePayment(OrderForm form, string methodName, PaymentModel model)
        {
            var paymentOption =
                (from o in _paymentOptions where o.Key.Equals(methodName, StringComparison.OrdinalIgnoreCase) select o)
                    .SingleOrDefault();

            Payment payment;

            if (paymentOption != null)
            {
                payment = paymentOption.PreProcess(form, model);
            }
            else
            {
                payment = new OtherPayment
                    {
                        PaymentType = PaymentType.Other.GetHashCode(),
                        BillingAddressId = form.BillingAddressId,
                        PaymentMethodName = model.DisplayName,
                        PaymentMethodId = model.Id,
                        Amount = form.Total
                    };
            }
            //Common Properties
            payment.Status = PaymentStatus.Pending.ToString();
            payment.OrderForm = form;
            payment.TransactionType = TransactionType.Sale.ToString();

            return payment;
        }

        /// <summary>
        /// Prepares the checkout model.
        /// </summary>
        /// <param name="model">The checkout model.</param>
        /// <returns>CheckoutModel.</returns>
        private CheckoutModel PrepareCheckoutModel(CheckoutModel model)
        {
            model.ButtonText = "Already registered? Click here to login.";
            model.Type = PopupType.LinkButton;
            model.ViewName = "~/Account/LogOnAsync";
            model.PopupTitle = "Login or Create New Account";

            var countries = _countryClient.GetAllCountries();

            // Bind addresses
            if (model.BillingAddress == null)
            {
                model.BillingAddress = new CheckoutAddressModel { Address = new AddressModel { Name = "Billing" } };
            }
            if (model.ShippingAddress == null)
            {
                model.ShippingAddress = new CheckoutAddressModel { Address = new AddressModel { Name = "Shipping" } };
            }

            model.BillingAddress.Countries = countries;
            model.ShippingAddress.Countries = countries;

            if (model.AddressBook == null)
            {
                model.AddressBook = new AddressBook();
            }

            if (UserHelper.CustomerSession.IsRegistered)
            {
                model.AddressBook.Addresses = UserHelper.GetAllCustomerAddresses().ToArray();
            }

            return model;
        }


        /// <summary>
        /// Gets the payments.
        /// </summary>
        /// <param name="shippingMethod">The shipping method.</param>
        /// <returns>List{PaymentModel}.</returns>
        private List<PaymentModel> GetPayments(string shippingMethod = null)
        {
            var paymentsString = _storeClient.GetCurrentStore().PaymentGateways.Select(c => c.PaymentGateway).ToArray();
            var methods = _paymentClient.GetAllPaymentsMethods(paymentsString).
                Where(x => shippingMethod == null || x.PaymentMethodShippingMethods.Any(y => y.ShippingMethodId == shippingMethod));

            var methodModels = new List<PaymentModel>();

            var cardTypes = new List<ListModel>
                {
                    new ListModel("--Please Select--".Localize(), ""),
                    new ListModel("American Express", "AE"),
                    new ListModel("Visa", "VI"),
                    new ListModel("Master Card", "MC"),
                    new ListModel("Discover", "DI")
                };

            var months = new List<ListModel>
                {
                    new ListModel("Month".Localize(), ""),
                    new ListModel("01 - January".Localize(), "1"),
                    new ListModel("02 - February".Localize(), "2"),
                    new ListModel("03 - March".Localize(), "3"),
                    new ListModel("04 - April".Localize(), "4"),
                    new ListModel("05 - May".Localize(), "5"),
                    new ListModel("06 - June".Localize(), "6"),
                    new ListModel("07 - July".Localize(), "7"),
                    new ListModel("08 - August".Localize(), "8"),
                    new ListModel("09 - September".Localize(), "9"),
                    new ListModel("10 - October".Localize(), "10"),
                    new ListModel("11 - November".Localize(), "11"),
                    new ListModel("12 - December".Localize(), "12")
                };

            var years = new List<ListModel> { new ListModel("Year".Localize(), "") };
            for (var index = DateTime.Now.Year; index <= DateTime.Now.Year + 10; index++)
            {
                years.Add(new ListModel(index.ToString(CultureInfo.InvariantCulture),
                                        index.ToString(CultureInfo.InvariantCulture)));
            }

            foreach (var method in methods.OrderBy(m => m.Priority).ThenBy(m => m.Name))
            {
                if (method.Name.Equals("Credit", StringComparison.OrdinalIgnoreCase))
                {
                    if (!UserHelper.CustomerSession.IsRegistered)
                        continue;
                }

                var paymentMethodLanguage =
                    method.PaymentMethodLanguages.FirstOrDefault(
                        pml => pml.LanguageCode == UserHelper.CustomerSession.Language);

                var model = new PaymentModel
                    {
                        Id = method.PaymentMethodId,
                        Name = method.Name,
                        DisplayName = paymentMethodLanguage != null
                                          ? paymentMethodLanguage.DisplayName
                                          : method.Description,
                        Months = months.ToArray(),
                        Years = years.ToArray(),
                        CardTypes = cardTypes.ToArray()
                    };

                if (Ch.OrderForm.Payments != null)
                {
                    var payment = Ch.OrderForm.Payments.FirstOrDefault(p => p.PaymentMethodId == method.PaymentMethodId);

                    if (payment != null)
                    {
                        model.IsCurrent = true;

                        if (payment is CreditCardPayment)
                        {
                            var crPayment = payment as CreditCardPayment;
                            model.CardNumber = crPayment.CreditCardNumber;
                            model.CardType = crPayment.CreditCardType;
                            model.CustomerName = crPayment.CreditCardCustomerName;
                            model.ExpirationMonth = crPayment.CreditCardExpirationMonth;
                            model.ExpirationYear = crPayment.CreditCardExpirationYear;
                        }
                    }

                }
                methodModels.Add(model);
            }

            return methodModels;
        }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool DoValidateModel(object model)
        {
            //Must clear model state before calling manual validation
            //otherwise it allways return false if !ModelState.IsValid
            ModelState.Clear();

            var isValid = TryValidateModel(model);

            ModelState.Clear();

            return isValid;
        }

        /// <summary>
        /// Converts to customer address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Address.</returns>
        private static Address ConvertToCustomerAddress(AddressModel address)
        {
            var addr = new Address();
            addr.InjectFrom(address);
            return addr;
        }

        private static AddressModel ConvertToPaypalAddress(AddressType address, string name)
        {
            return new AddressModel
            {
                Name = name,
                FirstName = address.Name,
                LastName = address.Name,
                City = address.CityName,
                CountryCode = address.Country.HasValue
                    ? address.Country.Value.ToString()
                    : "USA",
                DaytimePhoneNumber = address.Phone ?? "PHONE", //This field is required
                CountryName = address.CountryName,
                Line1 = address.Street1,
                Line2 = address.Street2,
                PostalCode = address.PostalCode,
                StateProvince = address.StateOrProvince
            };
        }

        /// <summary>
        /// Converts to address model.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>AddressModel.</returns>
        private static AddressModel ConvertToAddressModel(OrderAddress address)
        {
            var addr = new AddressModel();
            addr.InjectFrom(address);
            addr.AddressId = address.OrderAddressId;
            return addr;
        }
    }
}