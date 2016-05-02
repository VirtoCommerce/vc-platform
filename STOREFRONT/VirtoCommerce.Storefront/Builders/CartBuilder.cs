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
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ILocalCacheManager _cacheManager;

        private ShoppingCart _cart;
        private const string _cartCacheRegion = "CartRegion";

        [CLSCompliant(false)]
        public CartBuilder(IShoppingCartModuleApi cartApi, IPromotionEvaluator promotionEvaluator, ICatalogSearchService catalogSearchService, ICommerceCoreModuleApi commerceApi, ILocalCacheManager cacheManager)
        {
            _cartApi = cartApi;
            _promotionEvaluator = promotionEvaluator;
            _catalogSearchService = catalogSearchService;
            _cacheManager = cacheManager;
            _commerceApi = commerceApi;
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

                var cart = await _cartApi.CartModuleGetCurrentCartAsync(store.Id, customer.Id);
                if (cart == null)
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
                    retVal = cart.ToWebModel(currency, language);
                }
                retVal.Customer = customer;

        
                return retVal;
            });

            return this;
        }

        public virtual async Task<ICartBuilder> AddItemAsync(Product product, int quantity)
        {
            AddLineItem(product.ToLineItem(_cart.Language, quantity));
            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemQuantityAsync(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                await InnerChangeItemQuantityAsync(lineItem, quantity);            
                await EvaluatePromotionsAndTaxes();
            }

            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity)
        {
            var lineItem = _cart.Items.ElementAt(lineItemIndex);
            if (lineItem != null)
            {
                await InnerChangeItemQuantityAsync(lineItem, quantity);
                await EvaluatePromotionsAndTaxes();
            }
            return this;
        }

        public virtual async Task<ICartBuilder> ChangeItemsQuantitiesAsync(int[] quantities)
        {
            for (var i = 0; i < quantities.Length; i++)
            {
                var lineItem = _cart.Items.ElementAt(i);
                if (lineItem != null && quantities[i] > 0)
                {
                    await InnerChangeItemQuantityAsync(lineItem, quantities[i]);
                }
            }
            await EvaluatePromotionsAndTaxes();
            return this;
        }

        public virtual async Task<ICartBuilder> RemoveItemAsync(string id)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {

                _cart.Items.Remove(lineItem);

                await EvaluatePromotionsAndTaxes();
            }

            return this;
        }

        public virtual async Task<ICartBuilder> ClearAsync()
        {
            _cart.Items.Clear();

            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> AddCouponAsync(string couponCode)
        {

            _cart.Coupon = new Coupon
            {
                Code = couponCode
            };

            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveCouponAsync()
        {
            _cart.Coupon = null;

            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> AddOrUpdateShipmentAsync(ShipmentUpdateModel updateModel)
        {
            var changedShipment = updateModel.ToShipmentModel(_cart.Currency);
            foreach (var updateItemModel in updateModel.Items)
            {
                var cartItem = _cart.Items.FirstOrDefault(i => i.Id == updateItemModel.LineItemId);
                if (cartItem != null)
                {
                    var shipmentItem = cartItem.ToShipmentItem();
                    shipmentItem.Quantity = updateItemModel.Quantity;
                    changedShipment.Items.Add(shipmentItem);
                }
            }

            Shipment shipment = null;
            if (!string.IsNullOrEmpty(changedShipment.Id))
            {
                shipment = _cart.Shipments.FirstOrDefault(s => s.Id == changedShipment.Id);
                if (shipment == null)
                {
                    throw new StorefrontException(string.Format("Shipment with {0} not found", changedShipment.Id));
                }
            }
            else
            {
                shipment = new Shipment(_cart.Currency);
                _cart.Shipments.Add(shipment);
            }

            if (changedShipment.DeliveryAddress != null)
            {
                shipment.DeliveryAddress = changedShipment.DeliveryAddress;
            }

            //Update shipment items
            if (changedShipment.Items != null)
            {
                Action<EntryState, CartShipmentItem, CartShipmentItem> pathAction = (changeState, sourceItem, targetItem) =>
                {
                    if (changeState == EntryState.Added)
                    {
                        var cartLineItem = _cart.Items.FirstOrDefault(i => i.Id == sourceItem.LineItem.Id);
                        if (cartLineItem != null)
                        {
                            var newShipmentItem = cartLineItem.ToShipmentItem();
                            newShipmentItem.Quantity = sourceItem.Quantity;
                            shipment.Items.Add(newShipmentItem);
                        }
                    }
                    else if (changeState == EntryState.Modified)
                    {
                        targetItem.Quantity = sourceItem.Quantity;
                    }
                    else if (changeState == EntryState.Deleted)
                    {
                        shipment.Items.Remove(sourceItem);
                    }
                };

                var shipmentItemComparer = AnonymousComparer.Create((CartShipmentItem x) => x.LineItem.Id);
                changedShipment.Items.CompareTo(shipment.Items, shipmentItemComparer, pathAction);
            }

            if (!string.IsNullOrEmpty(changedShipment.ShipmentMethodCode))
            {
                var availableShippingMethods = await GetAvailableShippingMethodsAsync();
                var shippingMethod = availableShippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == changedShipment.ShipmentMethodCode);
                if (shippingMethod == null)
                {
                    throw new StorefrontException("Unknown shipment method " + changedShipment.ShipmentMethodCode);
                }

                shipment.ShipmentMethodCode = shippingMethod.ShipmentMethodCode;
                shipment.ShippingPrice = shippingMethod.Price;
                shipment.TaxType = shippingMethod.TaxType;
            }

            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> RemoveShipmentAsync(string shipmentId)
        {
            var shipment = _cart.Shipments.FirstOrDefault(s => s.Id == shipmentId);
            if (shipment != null)
            {
                _cart.Shipments.Remove(shipment);
            }

            await EvaluatePromotionsAndTaxes();

            return this;
        }

        public virtual async Task<ICartBuilder> AddOrUpdatePaymentAsync(PaymentUpdateModel updateModel)
        {
            Payment payment = null;
            if (!string.IsNullOrEmpty(updateModel.Id))
            {
                payment = _cart.Payments.FirstOrDefault(s => s.Id == updateModel.Id);
                if (payment == null)
                {
                    throw new StorefrontException(String.Format("Payment with {0} not found", updateModel.Id));
                }
            }
            else
            {
                payment = new Payment(_cart.Currency);
                _cart.Payments.Add(payment);
            }
         
            if (updateModel.BillingAddress != null)
            {
                payment.BillingAddress = updateModel.BillingAddress;
            }

            if (!string.IsNullOrEmpty(updateModel.PaymentGatewayCode))
            {
                var availablePaymentMethods = await GetAvailablePaymentMethodsAsync();
                var paymentMethod = availablePaymentMethods.FirstOrDefault(pm => string.Equals(pm.GatewayCode, updateModel.PaymentGatewayCode, StringComparison.InvariantCultureIgnoreCase));
                if (paymentMethod == null)
                {
                    throw new StorefrontException("Unknown payment method " + updateModel.PaymentGatewayCode);
                }
                payment.PaymentGatewayCode = paymentMethod.GatewayCode;
            }
  
            payment.OuterId = updateModel.OuterId;
            payment.Amount = _cart.Total;

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

            await EvaluatePromotionsAndTaxes();

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
                    lineItem.ListPrice = quoteItem.ListPrice;
                    lineItem.SalePrice = quoteItem.SelectedTierPrice.Price;
                    lineItem.ValidationType = ValidationType.None;

                    AddLineItem(lineItem);
                }
            }

            if (quoteRequest.RequestShippingQuote)
            {
                _cart.Shipments.Clear();
                var shipment = new Shipment(_cart.Currency);

                foreach (var item in _cart.Items)
                {
                    shipment.Items.Add(item.ToShipmentItem());
                }

                if (quoteRequest.ShippingAddress != null)
                {
                    shipment.DeliveryAddress = quoteRequest.ShippingAddress;
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
                        }
                    }
                }

                _cart.Shipments.Add(shipment);
            }

            _cart.Payments.Clear();
            var payment = new Payment(_cart.Currency);

            if (quoteRequest.BillingAddress != null)
            {
                payment.BillingAddress = quoteRequest.BillingAddress;
            }

            payment.Amount = quoteRequest.Totals.GrandTotalInclTax;

            _cart.Payments.Add(payment);

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

        /// <summary>
        /// Evaluate taxes  for captured cart
        /// </summary>
        /// <returns></returns>
        public async Task<ICartBuilder> EvaluateTaxAsync()
        {
            var taxResult = await _commerceApi.CommerceEvaluateTaxesAsync(_cart.StoreId, _cart.ToTaxEvalContext());
            if(taxResult != null)
            {
                _cart.ApplyTaxRates(taxResult.Select(x => x.ToWebModel(_cart.Currency)));
            }
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

        private async Task InnerChangeItemQuantityAsync(LineItem lineItem, int quantity)
        {
            if (lineItem != null)
            {
                var product = (await _catalogSearchService.GetProductsAsync(new[] { lineItem.ProductId }, ItemResponseGroup.ItemWithPrices)).FirstOrDefault();
                if(product != null)
                {
                    lineItem.SalePrice = product.Price.GetTierPrice(quantity).Price;
                }
                if (quantity > 0)
                {
                    lineItem.Quantity = quantity;
                }
                else
                {
                    _cart.Items.Remove(lineItem);
                }
            }
        }

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
       
        private async Task EvaluatePromotionsAndTaxes()
        {
            await EvaluatePromotionsAsync();
            await EvaluateTaxAsync();
        }

        private string GetCartCacheKey(string storeId, string customerId)
        {
            return string.Format("Cart-{0}-{1}", storeId, customerId);
        }

      
    }
}