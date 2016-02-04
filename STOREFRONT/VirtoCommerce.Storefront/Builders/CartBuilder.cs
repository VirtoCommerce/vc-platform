using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using NLog;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Order.Events;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder, IAsyncObserver<UserLoginEvent>
    {
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ILogger _logger;

        private ShoppingCart _cart;
        private const string _cartCacheRegion = "CartRegion";

        [CLSCompliant(false)]
        public CartBuilder(IShoppingCartModuleApi cartApi, IPromotionEvaluator promotionEvaluator, ICatalogSearchService catalogSearchService, ICacheManager<object> cacheManager, ILogger logger)
        {
            _cartApi = cartApi;
            _promotionEvaluator = promotionEvaluator;
            _catalogSearchService = catalogSearchService;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public string CartCaheKey
        {
            get
            {
                if(_cart == null)
                {
                    throw new StorefrontException("Cart is not set");
                }
                return GetCartCacheKey(_cart.StoreId, _cart.CustomerId);
            }
        }

        #region ICartBuilder Members
        public ICartBuilder TakeCart(ShoppingCart cart)
        {
            _cart = cart;

            return this;
        }

        public virtual async Task<ICartBuilder> GetOrCreateNewTransientCartAsync(Store store, CustomerInfo customer, Language language, Currency currency)
        {
            var cacheKey = GetCartCacheKey(store.Id, customer.Id);

            _cart = await _cacheManager.GetAsync(cacheKey, _cartCacheRegion, async () =>
            {
                ShoppingCart retVal;

                var cartSearchResult = await _cartApi.CartModuleGetCurrentCartAsync(store.Id, customer.Id);
                if (cartSearchResult == null)
                {
                    retVal = new ShoppingCart(currency, language);

                    retVal.CustomerId = customer.Id;
                    retVal.Name = "Default";
                    retVal.StoreId = store.Id;

                    if (!customer.IsRegisteredUser)
                    {
                        retVal.CustomerName = StorefrontConstants.AnonymousUsername;
                    }
                    else
                    {
                        retVal.CustomerName = string.Format("{0} {1}", customer.FirstName, customer.LastName);
                    }

                }
                else
                {
                    var detalizedCart = await _cartApi.CartModuleGetCartByIdAsync(cartSearchResult.Id);
                    retVal = detalizedCart.ToWebModel(currency, language);
                }
                retVal.Customer = customer;

                _logger.Trace(string.Format("GetOrCreateNewTransientCartAsync: {0}", retVal));

                return retVal;
            });

            return this;
        }

        public virtual async Task<ICartBuilder> AddItemAsync(Product product, int quantity)
        {
            AddLineItem(product.ToLineItem(_cart.Language, quantity));

            _logger.Trace(string.Format("AddItemAsync:{0} {1} qty: {2}", _cart, product, quantity));

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemQuantityAsync(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                _logger.Trace(string.Format("ChangeItemQuantityAsync: {0} {1} new qty: {2}", _cart, lineItem, quantity));

                if (quantity > 0)
                {
                    lineItem.Quantity = quantity;
                }
                else
                {
                    _cart.Items.Remove(lineItem);
                }

                await EvaluatePromotionsAsync();
            }

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity)
        {
            var lineItem = _cart.Items.ElementAt(lineItemIndex);
            if (lineItem != null)
            {
                _logger.Trace(string.Format("ChangeItemQuantityAsync: {0} {1} new qty: {1}", _cart, lineItem, quantity));
                if (quantity > 0)
                {
                    lineItem.Quantity = quantity;
                }
                else
                {
                    _cart.Items.Remove(lineItem);
                }
            }

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemsQuantitiesAsync(int[] quantities)
        {
            for (var i = 0; i < quantities.Length; i++)
            {
                var lineItem = _cart.Items.ElementAt(i);
                if (lineItem != null && quantities[i] > 0)
                {
                    lineItem.Quantity = quantities[i];
                }
            }

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveItemAsync(string id)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                _logger.Trace(string.Format("RemoveItemAsync: {0} {1}", _cart, lineItem));

                _cart.Items.Remove(lineItem);

                await EvaluatePromotionsAsync();
            }

            return this;
        }

        public virtual async Task<ICartBuilder> ClearAsync()
        {
            _cart.Items.Clear();

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> AddCouponAsync(string couponCode)
        {
            _logger.Trace(string.Format("AddCouponAsync: {0} {1}", _cart, couponCode));

            _cart.Coupon = new Coupon
            {
                Code = couponCode
            };

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveCouponAsync()
        {
            _cart.Coupon = null;

            _logger.Trace(string.Format("RemoveCouponAsync: {0}", _cart));

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> AddOrUpdateShipmentAsync(string shipmentId, Address shippingAddress, ICollection<string> itemIds, string shippingMethodCode)
        {
            var shipment = _cart.Shipments.FirstOrDefault(s => s.Id == shipmentId);
            if (shipment == null)
            {
                shipment = new Shipment(_cart.Currency);
            }

            if (shippingAddress != null)
            {
                shipment.DeliveryAddress = shippingAddress;
            }

            if (itemIds != null)
            {
                foreach (var itemId in itemIds)
                {
                    var cartItem = _cart.Items.FirstOrDefault(i => i.Id == itemId);
                    if (cartItem != null)
                    {
                        var newShipmentItem = cartItem.ToShipmentItem();
                        var shipmentItem = shipment.Items.FirstOrDefault(i => i.LineItem != null && i.LineItem.Id == itemId);
                        if (shipmentItem != null)
                        {
                            shipmentItem = newShipmentItem;
                        }
                        else
                        {
                            shipment.Items.Add(newShipmentItem);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(shippingMethodCode))
            {
                var availableShippingMethods = await GetAvailableShippingMethodsAsync();
                var shippingMethod = availableShippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
                if (shippingMethod != null)
                {
                    shipment.ShipmentMethodCode = shippingMethod.ShipmentMethodCode;
                    shipment.ShippingPrice = shippingMethod.Price;
                    shipment.TaxType = shippingMethod.TaxType;
                }
            }

            if (shipment.IsTransient())
            {
                _logger.Trace(string.Format("AddOrUpdateShipmentAsync: {0} {1}", _cart, shipment.ShipmentMethodCode));

                _cart.Shipments.Add(shipment);
            }

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveShipmentAsync(string shipmentId)
        {
            var shipment = _cart.Shipments.FirstOrDefault(s => s.Id == shipmentId);
            if (shipment != null)
            {
                _logger.Trace(string.Format("RemoveShipmentAsync: {0} {1}", _cart, shipment.ShipmentMethodCode));

                _cart.Shipments.Remove(shipment);
            }

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> AddOrUpdatePaymentAsync(string paymentId, Address billingAddress, string paymentMethodCode, string outerId)
        {
            var payment = _cart.Payments.FirstOrDefault(p => p.Id == paymentId);
            if (payment == null)
            {
                payment = new Payment(_cart.Currency);
            }

            if (billingAddress != null)
            {
                payment.BillingAddress = billingAddress;
            }

            var availablePaymentMethods = await GetAvailablePaymentMethodsAsync();
            var paymentMethod = availablePaymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                payment.PaymentGatewayCode = paymentMethodCode;
            }

            payment.OuterId = outerId;

            if (payment.IsTransient())
            {
                _logger.Trace(string.Format("AddOrUpdatePaymentAsync: {0} {1}", _cart, payment.PaymentGatewayCode));

                _cart.Payments.Add(payment);
            }

            return this;
        }

        public virtual async Task<ICartBuilder> MergeWithCartAsync(ShoppingCart cart)
        {

            _logger.Trace(string.Format("MergeWithCartAsync: {0} -> {1}", cart, _cart));
            foreach (var lineItem in cart.Items)
            {
                AddLineItem(lineItem);
            }

            _cart.Coupon = cart.Coupon;

            _cart.Shipments.Clear();
            _cart.Shipments = cart.Shipments;

            _cart.Payments.Clear();
            _cart.Payments = cart.Payments;

            await EvaluatePromotionsAsync();

            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { cart.Id });
             _cacheManager.Remove(CartCaheKey, _cartCacheRegion);

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveCartAsync()
        {
            var log = LogManager.GetCurrentClassLogger();
            log.Trace(string.Format("RemoveCartAsync {0}", _cart));


            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { _cart.Id });
            _cacheManager.Remove(CartCaheKey, _cartCacheRegion);

            return this;
        }

        public virtual async Task<ICartBuilder> FillFromQuoteRequest(QuoteRequest quoteRequest)
        {
            var productIds = quoteRequest.Items.Select(i => i.ProductId);
            var products = await _catalogSearchService.GetProductsAsync(productIds.ToArray(), ItemResponseGroup.ItemLarge);

            _cart.Items.Clear();
            foreach (var product in products)
            {
                var quoteItem = quoteRequest.Items.FirstOrDefault(i => i.ProductId == product.Id);
                if (quoteItem != null)
                {
                    var lineItem = product.ToLineItem(_cart.Language, (int)quoteItem.SelectedTierPrice.Quantity);
                    lineItem.ListPrice = quoteItem.SelectedTierPrice.Price;
                    lineItem.SalePrice = quoteItem.SelectedTierPrice.Price;

                    AddLineItem(lineItem);
                }
            }

            _cart.Shipments.Clear();
            var shipment = new Shipment(_cart.Currency);

            foreach (var item in _cart.Items)
            {
                shipment.Items.Add(item.ToShipmentItem());
            }

            if (quoteRequest.ShipmentMethod != null)
            {
                var availableShippingMethods = await GetAvailableShippingMethodsAsync();
                if (availableShippingMethods != null)
                {
                    var availableShippingMethod = availableShippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == quoteRequest.ShipmentMethod.ShipmentMethodCode);
                    if (availableShippingMethod != null)
                    {
                        shipment = quoteRequest.ShipmentMethod.ToShipmentModel(_cart.Currency);
                        _cart.Shipments.Add(shipment);
                    }
                }
            }

            _cart.Payments.Clear();
            var payment = new Payment(_cart.Currency);

            if (quoteRequest.Addresses != null)
            {
                var shippingAddress = quoteRequest.Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
                if (shippingAddress != null)
                {
                    shipment.DeliveryAddress = shippingAddress;
                }
                var billingAddress = quoteRequest.Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
                if (billingAddress != null)
                {
                    payment.BillingAddress = billingAddress;
                    _cart.Payments.Add(payment);
                }
            }

            return this;
        }

        public virtual async Task<ICollection<ShippingMethod>> GetAvailableShippingMethodsAsync()
        {
            var availableShippingMethods = new List<ShippingMethod>();

            // TODO: Remake with shipmentId
            var serviceModels = await _cartApi.CartModuleGetShipmentMethodsAsync(_cart.Id);
            foreach (var serviceModel in serviceModels)
            {
                availableShippingMethods.Add(serviceModel.ToWebModel(new Currency[] { _cart.Currency }, _cart.Language));
            }

            return availableShippingMethods;
        }

        public virtual async Task<ICollection<PaymentMethod>> GetAvailablePaymentMethodsAsync()
        {
            var availablePaymentMethods = new List<PaymentMethod>();

            var serviceModels = await _cartApi.CartModuleGetPaymentMethodsAsync(_cart.Id);
            foreach (var serviceModel in serviceModels)
            {
                availablePaymentMethods.Add(serviceModel.ToWebModel());
            }

            return availablePaymentMethods;
        }

        public virtual async Task<ICartBuilder> EvaluatePromotionsAsync()
        {
            var promotionItems = _cart.Items.Select(i => i.ToPromotionItem()).ToList();

            var promotionContext = new PromotionEvaluationContext();
            promotionContext.CartPromoEntries = promotionItems;
            promotionContext.CartTotal = _cart.Total;
            promotionContext.Coupon = _cart.Coupon != null ? _cart.Coupon.Code : null;
            promotionContext.Currency = _cart.Currency;
            promotionContext.CustomerId = _cart.Customer.Id;
            promotionContext.IsRegisteredUser = _cart.Customer.IsRegisteredUser;
            promotionContext.Language = _cart.Language;
            promotionContext.PromoEntries = promotionItems;
            promotionContext.StoreId = _cart.StoreId;

            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, new IDiscountable[] { _cart });

            return this;
        }

        public virtual async Task SaveAsync()
        {
            var cart = _cart.ToServiceModel();

            //Invalidate cart in cache
            _cacheManager.Remove(CartCaheKey, _cartCacheRegion);
            var log = LogManager.GetCurrentClassLogger();
            log.Trace(string.Format("SaveAsync: {0}", _cart));

            if (_cart.IsTransient())
            {
                _cart = (await _cartApi.CartModuleCreateAsync(cart)).ToWebModel(_cart.Currency, _cart.Language);
            }
            else
            {
                await _cartApi.CartModuleUpdateAsync(cart);
            }
        }

        public ShoppingCart Cart
        {
            get
            {
                return _cart;
            }
        }
        #endregion

        #region IObserver<UserLoginEvent> Members
        /// <summary>
        /// Merger anonymous cart by loging event
        /// </summary>
        /// <param name="userLoginEvent"></param>
        public async Task OnNextAsync(UserLoginEvent userLoginEvent)
        {
            if (userLoginEvent == null)
                return;
         
            var workContext = userLoginEvent.WorkContext;
            var prevUser = userLoginEvent.PrevUser;
            var prevUserCart = userLoginEvent.WorkContext.CurrentCart;
            var newUser = userLoginEvent.NewUser;

            //If previous user was anonymous and it has not empty cart need merge anonymous cart to personal
            if (!prevUser.IsRegisteredUser && prevUserCart != null && prevUserCart.Items.Any())
            {

                //we load or create cart for new user
                await GetOrCreateNewTransientCartAsync(workContext.CurrentStore, newUser, workContext.CurrentLanguage, workContext.CurrentCurrency);
                await MergeWithCartAsync(prevUserCart);
                await SaveAsync();
            }
        }

        #endregion

        private void AddLineItem(LineItem lineItem)
        {
            var existingLineItem = _cart.Items.FirstOrDefault(li => li.ProductId == lineItem.ProductId);
            if (existingLineItem != null)
            {
                existingLineItem.Quantity += lineItem.Quantity;
            }
            else
            {
                lineItem.Id = null;
                _cart.Items.Add(lineItem);
            }
        }
       

        private string GetCartCacheKey(string storeId, string customerId)
        {
            return string.Format("Cart-{0}-{1}", storeId, customerId);
        }

      
    }
}