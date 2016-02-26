using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model.Common;
using System.Web.Mvc;
using System.Net;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Order.Events;
using System;

namespace VirtoCommerce.Storefront.Controllers.Api
{

    [HandleJsonError]
    public class ApiCartController : StorefrontControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IOrderModuleApi _orderApi;
        private readonly ICartValidator _cartValidator;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly IEventPublisher<OrderPlacedEvent> _orderPlacedEventPublisher;
 
        public ApiCartController(WorkContext workContext, ICatalogSearchService catalogSearchService, ICartBuilder cartBuilder, 
                                 IShoppingCartModuleApi cartApi, IOrderModuleApi orderApi, ICartValidator cartValidator, IStorefrontUrlBuilder urlBuilder,
                                 IEventPublisher<OrderPlacedEvent> orderPlacedEventPublisher)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _cartApi = cartApi;
            _orderApi = orderApi;
            _cartValidator = cartValidator;
            _catalogSearchService = catalogSearchService;
            _orderPlacedEventPublisher = orderPlacedEventPublisher;
        }

        // Get current user shopping cart
        // GET: storefrontapi/cart
        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            EnsureThatCartExist();

            await _cartBuilder.EvaluatePromotionsAsync();
            await _cartBuilder.EvaluateTaxAsync();
            await _cartValidator.ValidateAsync(_cartBuilder.Cart);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // GET: storefrontapi/cart/itemscount
        [HttpGet]
        public ActionResult GetCartItemsCount()
        {
            EnsureThatCartExist();

            return Json(_cartBuilder.Cart.ItemsCount, JsonRequestBehavior.AllowGet);
        }

        // POST: storefrontapi/cart/items?id=...&quantity=...
        [HttpPost]
        public async Task<ActionResult> AddItemToCart(string id, int quantity = 1)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(id)).LockAsync())
            {
                var products = await _catalogSearchService.GetProductsAsync(new string[] { id }, Model.Catalog.ItemResponseGroup.ItemLarge);
                if (products != null && products.Any())
                {
                    await _cartBuilder.AddItemAsync(products.First(), quantity);
                    await _cartBuilder.SaveAsync();
                }
            }
            return Json(new { ItemsCount = _cartBuilder.Cart.ItemsCount });
        }

        // PUT: storefrontapi/cart/items?lineItemId=...&quantity=...
        [HttpPut]
        public async Task<ActionResult> ChangeCartItem(string lineItemId, int quantity)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {

                var lineItem = _cartBuilder.Cart.Items.FirstOrDefault(i => i.Id == lineItemId);
                if (lineItem != null)
                {
                    await _cartBuilder.ChangeItemQuantityAsync(lineItemId, quantity);
                    await _cartBuilder.SaveAsync();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // DELETE: storefrontapi/cart/items?lineItemId=...
        [HttpDelete]
        public async Task<ActionResult> RemoveCartItem(string lineItemId)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.RemoveItemAsync(lineItemId);
                await _cartBuilder.SaveAsync();
            }

            return Json(new { ItemsCount = _cartBuilder.Cart.ItemsCount });
        }

        // POST: storefrontapi/cart/clear
        [HttpPost]
        public async Task<ActionResult> ClearCart()
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.ClearAsync();
                await _cartBuilder.SaveAsync();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: storefrontapi/cart/shipments/{shipmentId}/shippingmethods
        [HttpGet]
        public async Task<ActionResult> GetCartShipmentAvailShippingMethods(string shipmentId)
        {
            EnsureThatCartExist();

            var shippingMethods = await _cartBuilder.GetAvailableShippingMethodsAsync();
            return Json(shippingMethods, JsonRequestBehavior.AllowGet);
        }

        // GET: storefrontapi/cart/paymentmethods
        [HttpGet]
        public async Task<ActionResult> GetCartAvailPaymentMethods()
        {
            EnsureThatCartExist();

            var paymentMethods = await _cartBuilder.GetAvailablePaymentMethodsAsync();
            return Json(paymentMethods, JsonRequestBehavior.AllowGet);
        }

        // POST: storefrontapi/cart/coupons/{couponCode}
        [HttpPost]
        public async Task<ActionResult> AddCartCoupon(string couponCode)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.AddCouponAsync(couponCode);
                await _cartBuilder.SaveAsync();
            }
            return Json(_cartBuilder.Cart.Coupon);
        }


        // DELETE: storefrontapi/cart/coupons
        [HttpDelete]
        public async Task<ActionResult> RemoveCartCoupon()
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.RemoveCouponAsync();
                await _cartBuilder.SaveAsync();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: storefrontapi/cart/shipments
        [HttpPost]
        public async Task<ActionResult> AddOrUpdateCartShipment(ShipmentUpdateModel shipment)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.AddOrUpdateShipmentAsync(shipment);
                await _cartBuilder.SaveAsync();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: storefrontapi/cart/payments
        [HttpPost]
        public async Task<ActionResult> AddOrUpdateCartPayment(PaymentUpdateModel payment)
        {
            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                await _cartBuilder.AddOrUpdatePaymentAsync(payment);
                await _cartBuilder.SaveAsync();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: storefrontapi/cart/createorder
        [HttpPost]
        public async Task<ActionResult> CreateOrder(VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo)
        {
            if(bankCardInfo == null)
            {
                throw new ArgumentNullException("bankCardInfo");
            }

            EnsureThatCartExist();

            //Need lock to prevent concurrent access to same cart
            using (var lockObject = await AsyncLock.GetLockByKey(GetAsyncLockCartKey(WorkContext.CurrentCart.Id)).LockAsync())
            {
                var order = await _orderApi.OrderModuleCreateOrderFromCartAsync(_cartBuilder.Cart.Id);

                //Raise domain event
                await _orderPlacedEventPublisher.PublishAsync(new OrderPlacedEvent(order.ToWebModel(base.WorkContext.AllCurrencies, base.WorkContext.CurrentLanguage), _cartBuilder.Cart));

                await _cartBuilder.RemoveCartAsync();

                VirtoCommerceOrderModuleWebModelProcessPaymentResult processingResult = null;
                var incomingPayment = order.InPayments != null ? order.InPayments.FirstOrDefault() : null;
                if (incomingPayment != null)
                {
                     processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(bankCardInfo, order.Id, incomingPayment.Id);
                }

                return Json(new { order = order, orderProcessingResult = processingResult });
            }
        }
      
        private static string GetAsyncLockCartKey(string cartId)
        {
            return "Cart:" + cartId;
        }

        private void EnsureThatCartExist()
        {
            if (WorkContext.CurrentCart == null)
            {
                throw new StorefrontException("Cart not found");
            }
            _cartBuilder.TakeCart(WorkContext.CurrentCart);
        }
    }
}