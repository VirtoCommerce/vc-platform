using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICacheManager<object> _cacheManager;

        private ShoppingCart _cart;
        private const string _cartCacheRegion = "CartRegion";

       
        public CartBuilder(IShoppingCartModuleApi cartApi, IPromotionEvaluator promotionEvaluator, ICacheManager<object> cacheManager)
        {
            _cartApi = cartApi;
            _promotionEvaluator = promotionEvaluator;
            _cacheManager = cacheManager;
        }

        public string CartCaheKey
        {
            get
            {
                if(_cart == null)
                {
                    throw new StorefrontException("Cart not set");
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

                return retVal;
            });

            return this;
        }

        public virtual async Task<ICartBuilder> AddItemAsync(Product product, int quantity)
        {
            AddLineItem(product.ToLineItem(_cart.Language, quantity));

            await EvaluatePromotionsAsync();

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemQuantityAsync(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
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
                _cart.Payments.Add(payment);
            }

            return this;
        }

        public virtual async Task<ICartBuilder> MergeWithCartAsync(ShoppingCart cart)
        {
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
            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { _cart.Id });
            _cacheManager.Remove(CartCaheKey, _cartCacheRegion);

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