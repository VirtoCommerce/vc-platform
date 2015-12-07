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

        public CartController(WorkContext workContext, IShoppingCartModuleApi cartApi, IOrderModuleApi orderApi, IStorefrontUrlBuilder urlBuilder,
                              ICartBuilder cartBuilder, ICatalogSearchService catalogService, IMarketingModuleApi marketingApi, ICommerceCoreModuleApi commerceApi)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
            _cartApi = cartApi;
            _orderApi = orderApi;
            _marketingApi = marketingApi;
            _commerceApi = commerceApi;
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
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            _cartBuilder.Cart.Items.OrderBy(i => i.CreatedDate);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/add_item?productId=...&quantity=...
        [HttpPost]
        [Route("add_item")]
        public async Task<ActionResult> AddItemJson(string productId, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var product = await _catalogService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (product != null)
            {
                await _cartBuilder.AddItemAsync(product, quantity);
                await _cartBuilder.SaveAsync();
                await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);
            }

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/change_item?lineItemId=...&quantity=...
        [HttpPost]
        [Route("change_item")]
        public async Task<ActionResult> ChangeItemJson(string lineItemId, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemQuantityAsync(lineItemId, quantity);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/remove_item?lineItemId=...
        [HttpPost]
        [Route("remove_item")]
        public async Task<ActionResult> RemoveItemJson(string lineItemId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItemAsync(lineItemId);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout
        [HttpGet]
        [Route("checkout")]
        public async Task<ActionResult> Checkout()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            return View("checkout", "checkout_layout", _cartBuilder.Cart);
        }

        // GET: /cart/{cartId}/shipping_methods/json
        [HttpGet]
        [Route("{cartId}/shipping_methods/json")]
        public async Task<ActionResult> CartShippingMethodsJson(string cartId)
        {
            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(cartId);

            return Json(shippingMethods.Select(sm => sm.ToWebModel()), JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/{cartId}/payment_methods/json
        [HttpGet]
        [Route("{cartId}/payment_methods/json")]
        public async Task<ActionResult> CartPaymentMethodsJson(string cartId)
        {
            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(cartId);

            return Json(paymentMethods.Select(pm => pm.ToWebModel()), JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/add_coupon/{couponCode}
        [HttpPost]
        [Route("add_coupon/{couponCode}")]
        public async Task<ActionResult> AddCouponJson(string couponCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.AddCouponAsync(couponCode);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/remove_coupon
        [HttpPost]
        [Route("remove_coupon")]
        public async Task<ActionResult> RemoveCouponJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveCouponAsync();
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/add_address
        [HttpPost]
        [Route("add_address")]
        public async Task<ActionResult> AddAddressJson(Address address)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.AddAddressAsync(address);
            await _cartBuilder.SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/shipping_method?shippingMethodCode=...?isPreview=...
        [HttpPost]
        [Route("shipping_method")]
        public async Task<ActionResult> SetShippingMethodsJson(string shippingMethodCode, bool isPreview)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(WorkContext.CurrentCart.Id);
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipmentAsync(shippingMethod.ToWebModel());

                if (!isPreview)
                {
                    await _cartBuilder.SaveAsync();
                }
            }

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/payment_method?paymentMethodCode=
        [HttpPost]
        [Route("payment_method")]
        public async Task<ActionResult> SetPaymentMethodsJson(string paymentMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(WorkContext.CurrentCart.Id);
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                await _cartBuilder.AddPaymentAsync(paymentMethod.ToWebModel());
                await _cartBuilder.SaveAsync();
            }

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/{cartId}/create_order
        [HttpPost]
        [Route("{cartId}/create_order")]
        public async Task<ActionResult> CreateOrderJson(string cartId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var order = await _orderApi.OrderModuleCreateOrderFromCartAsync(cartId);
            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { cartId });

            return Json(order.ToWebModel(), JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/process_payment?orderId=...&paymentId=...&bankCardInfo=...
        [HttpPost]
        [Route("process_payment")]
        public async Task<ActionResult> ProcessPaymentJson(string orderId, string paymentId, BankCardInfo bankCardInfo)
        {
            var cardInfo = new BankCardInfo();

            var processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(cardInfo.ToServiceModel(), orderId, paymentId);

            return Json(processingResult, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout/external-payment-callback
        [HttpGet]
        [Route("checkout/external-payment-callback")]
        public ActionResult ExternalPaymentCallback()
        {
            // TODO: Process callback from external payment gateway

            return StoreFrontRedirect("~/cart/checkout/thanks");
        }

        // GET: /cart/checkout/thanks?id=...
        [HttpGet]
        [Route("checkout/thanks")]
        public async Task<ActionResult> Thanks(string id)
        {
            var order = await _orderApi.OrderModuleGetByIdAsync(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View("thanks", base.WorkContext);
        }
    }
}