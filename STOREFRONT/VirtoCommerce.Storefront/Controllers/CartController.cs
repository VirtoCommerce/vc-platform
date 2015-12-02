using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
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

        public CartController(WorkContext workContext, IShoppingCartModuleApi cartApi, IOrderModuleApi orderApi, IStorefrontUrlBuilder urlBuilder,
                              ICartBuilder cartBuilder, ICatalogSearchService catalogService, IMarketingModuleApi marketingApi)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
            _cartApi = cartApi;
            _orderApi = orderApi;
            _marketingApi = marketingApi;
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

        // POST: /cart/add_item?productId=...&quantity=...
        [HttpPost]
        [Route("add_item")]
        public async Task<ActionResult> AddItemJson(string productId, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var product = await _catalogService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (product != null)
            {
                await _cartBuilder.AddItem(product, quantity).SaveAsync();
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

            await _cartBuilder.UpdateItem(lineItemId, quantity).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/remove_item?lineItemId=...
        [HttpPost]
        [Route("remove_item")]
        public async Task<ActionResult> RemoveItemJson(string lineItemId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItem(lineItemId).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout
        [HttpGet]
        [Route("checkout")]
        public async Task<ActionResult> Checkout()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            if (WorkContext.CurrentCart.Items.Count == 0)
            {
                return StoreFrontRedirect("~/cart");
            }

            return View("checkout", "checkout_layout", _cartBuilder.Cart);
        }

        // POST: /cart/add_coupon/{couponCode}
        [HttpPost]
        [Route("add_coupon/{couponCode}")]
        public async Task<ActionResult> AddCouponJson(string couponCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var validPromotionRewards = await EvaluatePromotionsAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, _cartBuilder.Cart, couponCode);

            await _cartBuilder.UpdateDiscounts(validPromotionRewards).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/remove_coupon
        [HttpPost]
        [Route("remove_coupon")]
        public async Task<ActionResult> RemoveCouponJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var validPromotionRewards = await EvaluatePromotionsAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, _cartBuilder.Cart, null);

            await _cartBuilder.UpdateDiscounts(validPromotionRewards).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/add_address
        [HttpPost]
        [Route("add_address")]
        public async Task<ActionResult> AddAddressJson(Address address)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.AddAddress(address).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/shipping_method?shippingMethodCode=...
        [HttpPost]
        [Route("shipping_method")]
        public async Task<ActionResult> SetShippingMethodsJson(string shippingMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(WorkContext.CurrentCart.Id);
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipment(shippingMethod.ToWebModel()).SaveAsync();
            }

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/payment_method?paymentMethodCode=...&billingAddress=...
        [HttpPost]
        [Route("payment_method")]
        public async Task<ActionResult> SetPaymentMethodsJson(string paymentMethodCode, Address billingAddress)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            _cartBuilder.AddAddress(billingAddress);

            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(WorkContext.CurrentCart.Id);
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                await _cartBuilder.AddPayment(paymentMethod.ToWebModel()).SaveAsync();
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

        // POST: /cart/process_payment?orderId=...&paymentId=...
        [HttpPost]
        [Route("process_payment")]
        public async Task<ActionResult> ProcessPaymentJson(string orderId, string paymentId)
        {
            var bankCardInfo = new Client.Model.VirtoCommerceDomainPaymentModelBankCardInfo();

            var processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(bankCardInfo, orderId, paymentId);

            return Json(processingResult, JsonRequestBehavior.AllowGet);
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

        private async Task<IEnumerable<VirtoCommerceMarketingModuleWebModelPromotionReward>> EvaluatePromotionsAsync(Store store, Customer customer, ShoppingCart cart, string couponCode)
        {
            var promotionContext = new VirtoCommerceDomainMarketingModelPromotionEvaluationContext
            {
                CustomerId = customer.Id,
                Coupon = couponCode,
                StoreId = store.Id
            };

            promotionContext.CartPromoEntries = new List<VirtoCommerceDomainMarketingModelProductPromoEntry>();
            foreach (var lineItem in cart.Items)
            {
                promotionContext.CartPromoEntries.Add(lineItem.ToPromotionItem());
            }

            promotionContext.PromoEntries = new List<VirtoCommerceDomainMarketingModelProductPromoEntry>();
            foreach (var lineItem in cart.Items)
            {
                promotionContext.PromoEntries.Add(lineItem.ToPromotionItem());
            }

            var promotionResult = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(promotionContext);

            return promotionResult.Where(pr => pr.IsValid.HasValue && pr.IsValid.Value);
        }
    }
}