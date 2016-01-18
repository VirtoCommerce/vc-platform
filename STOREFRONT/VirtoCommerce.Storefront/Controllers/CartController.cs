using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class CartController : StorefrontControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogSearchService _catalogService;
        private readonly IOrderModuleApi _orderApi;
        private readonly IMarketingModuleApi _marketingApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly ICartValidator _cartValidator;

        public CartController(WorkContext workContext, IOrderModuleApi orderApi, IStorefrontUrlBuilder urlBuilder,
                              ICartBuilder cartBuilder, ICatalogSearchService catalogService, IMarketingModuleApi marketingApi, ICommerceCoreModuleApi commerceApi,
                              ICustomerManagementModuleApi customerApi, ICartValidator cartValidator)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
            _orderApi = orderApi;
            _marketingApi = marketingApi;
            _commerceApi = commerceApi;
            _customerApi = customerApi;
            _cartValidator = cartValidator;
        }

        // GET: /cart
        [HttpGet]
        public ActionResult Index()
        {
            return View("cart", WorkContext);
        }

        // GET: /cart/json
        [HttpGet]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> CartJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartValidator.ValidateAsync(_cartBuilder.Cart);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/additem?id=...&quantity=...
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> AddItemJson(string id, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);
            throw new NullReferenceException("ssss");
            //var products = await _catalogService.GetProductsAsync(new string[] { id }, Model.Catalog.ItemResponseGroup.ItemLarge);
            //if (products != null && products.Any())
            //{
            //    await _cartBuilder.AddItemAsync(products.First(), quantity);
            //    await _cartBuilder.SaveAsync();
            //}

            //return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/changeitem?lineItemId=...&quantity=...
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> ChangeItemJson(string lineItemId, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var lineItem = _cartBuilder.Cart.Items.FirstOrDefault(i => i.Id == lineItemId);
            if (lineItem != null)
            {
            await _cartBuilder.ChangeItemQuantityAsync(lineItemId, quantity);
            await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/removeitem?lineItemId=...
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> RemoveItemJson(string lineItemId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItemAsync(lineItemId);
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/clear
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> ClearJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ClearAsync();
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout
        [HttpGet]
        public async Task<ActionResult> Checkout()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            return View("checkout", "checkout_layout", _cartBuilder.Cart);
        }

        // GET: /cart/shippingmethods/json
        [HttpGet]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> CartShippingMethodsJson(string shipmentId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartBuilder.GetAvailableShippingMethodsAsync();

            return Json(shippingMethods, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/paymentmethods/json
        [HttpGet]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> CartPaymentMethodsJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartBuilder.GetAvailablePaymentMethodsAsync();

            return Json(paymentMethods, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/addcoupon/{couponCode}
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> AddCouponJson(string couponCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.AddCouponAsync(couponCode);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart.Coupon, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/removecoupon
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> RemoveCouponJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveCouponAsync();
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/addaddress
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> AddAddressJson(Address address)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.AddAddressAsync(address);
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/shippingmethod?shippingMethodCode=...
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> SetShippingMethodsJson(string shippingMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartBuilder.GetAvailableShippingMethodsAsync();
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipmentAsync(shippingMethod);
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/paymentmethod?paymentMethodCode=...
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> SetPaymentMethodsJson(string paymentMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartBuilder.GetAvailablePaymentMethodsAsync();
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                await _cartBuilder.AddPaymentAsync(paymentMethod);
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/createorder
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> CreateOrderJson(VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var order = await _orderApi.OrderModuleCreateOrderFromCartAsync(_cartBuilder.Cart.Id);

            await _cartBuilder.RemoveCartAsync();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var contact = await _customerApi.CustomerModuleGetContactByIdAsync(WorkContext.CurrentCustomer.Id);

                foreach (var orderAddress in order.Addresses)
                {
                    contact.Addresses.Add(orderAddress.ToCustomerModel());
                }

                await _customerApi.CustomerModuleUpdateContactAsync(contact);
            }

            var processingResult = await GetOrderProcessingResultAsync(order, bankCardInfo);

            return Json(new { order = order, orderProcessingResult = processingResult }, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout/paymentform?orderNumber=...
        [HttpGet]
        public async Task<ActionResult> PaymentForm(string orderNumber)
        {
            var order = await _orderApi.OrderModuleGetByNumberAsync(orderNumber);

            var processingResult = await GetOrderProcessingResultAsync(order, null);

            WorkContext.PaymentFormHtml = processingResult.HtmlForm;

            return View("payment-form", WorkContext);
        }

        // GET: /cart/externalpaymentcallback
        [HttpGet]
        public async Task<ActionResult> ExternalPaymentCallback()
        {
            var callback = new VirtoCommerceCoreModuleWebModelPaymentCallbackParameters
            {
                Parameters = new List<VirtoCommerceCoreModuleWebModelKeyValuePair>()
            };

            foreach (var key in HttpContext.Request.QueryString.AllKeys)
            {
                callback.Parameters.Add(new VirtoCommerceCoreModuleWebModelKeyValuePair
                {
                    Key = key,
                    Value = HttpContext.Request.QueryString[key]
                });
            }

            foreach (var key in HttpContext.Request.Form.AllKeys)
            {
                callback.Parameters.Add(new VirtoCommerceCoreModuleWebModelKeyValuePair
                {
                    Key = key,
                    Value = HttpContext.Request.Form[key]
                });
            }

            var postProcessingResult = await _commerceApi.CommercePostProcessPaymentAsync(callback);
            if (postProcessingResult.IsSuccess.HasValue && postProcessingResult.IsSuccess.Value)
            {
                return StoreFrontRedirect("~/cart/thanks/" + postProcessingResult.OrderId);
            }
            else
            {
                return View("error");
            }
        }

        // GET: /cart/thanks/{orderNumber}
        [HttpGet]
        public async Task<ActionResult> Thanks(string orderNumber)
        {
            var order = await _orderApi.OrderModuleGetByNumberAsync(orderNumber);

            if (order == null || order != null && order.CustomerId != WorkContext.CurrentCustomer.Id)
            {
                return HttpNotFound();
            }

            WorkContext.Order = order.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage);

            return View("thanks", WorkContext);
        }

        private async Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> GetOrderProcessingResultAsync(
            VirtoCommerceOrderModuleWebModelCustomerOrder order, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo)
        {
            if (bankCardInfo == null)
            {
                bankCardInfo = new VirtoCommerceDomainPaymentModelBankCardInfo();
            }

            VirtoCommerceOrderModuleWebModelProcessPaymentResult processingResult = null;
            var incomingPayment = order.InPayments != null ? order.InPayments.FirstOrDefault() : null;
            if (incomingPayment != null)
            {
                processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(bankCardInfo, order.Id, incomingPayment.Id);
            }

            return processingResult;
        }
    }
}