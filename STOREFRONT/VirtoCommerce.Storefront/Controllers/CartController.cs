using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : StorefrontControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogSearchService _catalogService;
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IOrderModuleApi _orderApi;
        private readonly IMarketingModuleApi _marketingApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly ICustomerManagementModuleApi _customerApi;

        public CartController(WorkContext workContext, IShoppingCartModuleApi cartApi, IOrderModuleApi orderApi, IStorefrontUrlBuilder urlBuilder,
                              ICartBuilder cartBuilder, ICatalogSearchService catalogService, IMarketingModuleApi marketingApi, ICommerceCoreModuleApi commerceApi,
                              ICustomerManagementModuleApi customerApi)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
            _cartApi = cartApi;
            _orderApi = orderApi;
            _marketingApi = marketingApi;
            _commerceApi = commerceApi;
            _customerApi = customerApi;
        }

        // GET: /cart
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("cart", WorkContext);
        }
     
        // GET: /cart/json
        [HttpGet]
        [Route("json")]
        public async Task<ActionResult> CartJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            _cartBuilder.Cart.Items.OrderBy(i => i.CreatedDate);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/additem?productId=...&quantity=...
        [HttpPost]
        [Route("additem")]
        public async Task<ActionResult> AddItemJson(string productId, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var product = await _catalogService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (product != null)
            {
                await _cartBuilder.AddItemAsync(product, quantity);
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/changeitem?lineItemId=...&quantity=...
        [HttpPost]
        [Route("changeitem")]
        public async Task<ActionResult> ChangeItemJson(string lineItemId, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemQuantityAsync(lineItemId, quantity);
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/removeitem?lineItemId=...
        [HttpPost]
        [Route("removeitem")]
        public async Task<ActionResult> RemoveItemJson(string lineItemId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItemAsync(lineItemId);
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout
        [HttpGet]
        [Route("checkout")]
        public async Task<ActionResult> Checkout()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            return View("checkout", "checkout_layout", _cartBuilder.Cart);
        }

        // GET: /cart/shippingmethods/json
        [HttpGet]
        [Route("shippingmethods/json")]
        public async Task<ActionResult> CartShippingMethodsJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(_cartBuilder.Cart.Id);

            return Json(shippingMethods.Select(sm => sm.ToWebModel()), JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/paymentmethods/json
        [HttpGet]
        [Route("paymentmethods/json")]
        public async Task<ActionResult> CartPaymentMethodsJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(_cartBuilder.Cart.Id);

            return Json(paymentMethods.Select(pm => pm.ToWebModel()), JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/addcoupon/{couponCode}
        [HttpPost]
        [Route("addcoupon/{couponCode}")]
        public async Task<ActionResult> AddCouponJson(string couponCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.AddCouponAsync(couponCode);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart.Coupon, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/removecoupon
        [HttpPost]
        [Route("removecoupon")]
        public async Task<ActionResult> RemoveCouponJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveCouponAsync();
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/addaddress
        [HttpPost]
        [Route("addaddress")]
        public async Task<ActionResult> AddAddressJson(Address address)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.AddAddressAsync(address);
            await _cartBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/shippingmethod?shippingMethodCode=...
        [HttpPost]
        [Route("shippingmethod")]
        public async Task<ActionResult> SetShippingMethodsJson(string shippingMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(WorkContext.CurrentCart.Id);
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipmentAsync(shippingMethod.ToWebModel());
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/paymentmethod?paymentMethodCode=
        [HttpPost]
        [Route("paymentmethod")]
        public async Task<ActionResult> SetPaymentMethodsJson(string paymentMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(WorkContext.CurrentCart.Id);
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                await _cartBuilder.AddPaymentAsync(paymentMethod.ToWebModel());
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/createorder
        [HttpPost]
        [Route("createorder")]
        public async Task<ActionResult> CreateOrderJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var order = await _orderApi.OrderModuleCreateOrderFromCartAsync(_cartBuilder.Cart.Id);
            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { _cartBuilder.Cart.Id });

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var contact = await _customerApi.CustomerModuleGetContactByIdAsync(WorkContext.CurrentCustomer.Id);

                foreach (var orderAddress in order.Addresses)
                {
                    contact.Addresses.Add(orderAddress.ToCustomerModel());
                }

                await _customerApi.CustomerModuleUpdateContactAsync(contact);
            }

            return Json(order.ToWebModel(), JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/processpayment?orderId=...&paymentId=...&bankCardInfo=...
        [HttpPost]
        [Route("processpayment")]
        public async Task<ActionResult> ProcessPaymentJson(string orderId, string paymentId, BankCardInfo bankCardInfo)
        {
            var cardInfo = new BankCardInfo();

            var processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(cardInfo.ToServiceModel(), orderId, paymentId);

            return Json(processingResult, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/externalpaymentcallback?orderId=...
        [HttpGet]
        [Route("externalpaymentcallback")]
        public async Task<ActionResult> ExternalPaymentCallback(string orderId)
        {
            var processingResult = await _commerceApi.CommercePostProcessPaymentAsync(orderId);

            if (!processingResult.IsSuccess.HasValue || !processingResult.IsSuccess.Value)
            {
                return View("error");
            }

            var order = await _orderApi.OrderModuleGetByIdAsync(orderId);

            return StoreFrontRedirect("~/cart/checkout/thanks");
        }

        // GET: /cart/thanks?orderId=...
        [HttpGet]
        [Route("thanks")]
        public async Task<ActionResult> Thanks(string orderId)
        {
            var order = await _orderApi.OrderModuleGetByIdAsync(orderId);

            if (order == null)
            {
                return HttpNotFound();
            }

            WorkContext.Order = order.ToWebModel();

            return View("thanks", WorkContext);
        }
    }
}