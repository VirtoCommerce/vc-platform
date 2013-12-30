using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;
using VirtoCommerce.Web.Virto.Helpers.Payments;
using VirtoCommerce.Web.Virto.Helpers.Popup;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class CheckoutController.
	/// </summary>
	[Localize]
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
			var store = _storeClient.GetCurrentStore();

			//Get shipping mentods avaialble in current store
			var storeShippingMethods = Ch.ShippingClient.GetAllShippingMethods()
					  .Where(sm => sm.PaymentMethodShippingMethods.Select(x => x.PaymentMethod)
		                   .Any(pm => store.PaymentGateways.Any(pg => pg.PaymentGateway == pm.Name)))
						   .Select(s=>s.ShippingMethodId).ToList();

			var methods = Ch.GetShippingMethods(storeShippingMethods);
            var model = new ShipmentsModel { Shipments = methods };
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
            // reset the cart
            Ch.Reset();

            // Update cart from the post back
            UpdateCart(checkoutModel);

            // run workflow
            Ch.RunWorkflow("ShoppingCartPrepareWorkflow");

            // save changes
            Ch.SaveChanges();

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
            UpdateCart(checkoutModel, true);

            if (!ModelState.IsValid)
            {
                return View("Index", checkoutModel);
            }
       
	        try
	        {
				// run business rules
		        Ch.RunWorkflow("ShoppingCartCheckoutWorkflow");
	        }
	        catch (Exception ex)
	        {
		        ModelState.AddModelError("", ex.Message);
				return View("Index", checkoutModel);
	        }
	        Ch.SaveChanges();

            // Create order
            var order = Ch.SaveAsOrder();

            if (HttpContext.Session != null)
                HttpContext.Session["LatestOrderId"] = order.OrderGroupId;

            if (UserHelper.CustomerSession.IsRegistered)
            {
                var orgs = _userClient.GetOrganizationsForCurrentUser();
                if (orgs != null)
                {
                    var org = orgs.SingleOrDefault();
                    if (org != null)
                        Ch.Cart.OrganizationId = org.MemberId;
                }
            }

            if (UserHelper.CustomerSession.IsRegistered)
            {
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

            //Create account for later use
            if (!UserHelper.CustomerSession.IsRegistered && checkoutModel.CreateAccount)
            {
                var regModel = new RegisterModel
                    {
                        ActionResult = RedirectToAction("ProcessCheckout", "Checkout", new { id = order.OrderGroupId })
                    };

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

                TempData["RegisterModel"] = regModel;
                return RedirectToAction("Register", "Account");
            }

            // display success screen
            return RedirectToAction("ProcessCheckout", "Checkout", new { id = order.OrderGroupId });
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
        private List<PaymentModel> GetPayments(string shippingMethod)
        {
			var paymentsString = _storeClient.GetCurrentStore().PaymentGateways.Select(c => c.PaymentGateway).ToArray();
			var methods = _paymentClient.GetAllPaymentsMethods(paymentsString).
				Where(x => x.PaymentMethodShippingMethods.Any(y => y.ShippingMethodId == shippingMethod));

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