using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IMarketingService _marketingService;
        private readonly ICacheManager<object> _cacheManager;

        private Store _store;
        private Customer _customer;
        private Currency _currency;
        private Language _language;
        private ShoppingCart _cart;

        public CartBuilder(IShoppingCartModuleApi cartApi, IMarketingService marketingService, ICacheManager<object> cacheManager)
        {
            _cartApi = cartApi;
            _marketingService = marketingService;
            _cacheManager = cacheManager;
        }

        public async Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Language language, Currency currency)
        {
            _store = store;
            _customer = customer;
            _currency = currency;
            _language = language;

            _cart = await _cacheManager.GetAsync(GetCartCacheKey(store.Id, customer.Id), "CartRegion", async () =>
            {
                ShoppingCart retVal = null;
                var cartSearchResult = await _cartApi.CartModuleGetCurrentCartAsync(_store.Id, _customer.Id);
                if (cartSearchResult == null)
                {
                    retVal = CreateNewTransientCart();
                }
                else
                {
                    var detalizedCart = await _cartApi.CartModuleGetCartByIdAsync(cartSearchResult.Id);
                    retVal = detalizedCart.ToWebModel(_currency, _language);
                }
                return retVal;
            });

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> AddItemAsync(Product product, int quantity)
        {
            AddLineItem(product.ToLineItem(_language, quantity));

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> ChangeItemQuantityAsync(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                if (quantity > 0)
                {
                    lineItem.Quantity = quantity;

                    await EvaluatePromotionsAsync();
                }
            }

            return this;
        }

        public async Task<CartBuilder> RemoveItemAsync(string id)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                _cart.Items.Remove(lineItem);

                await EvaluatePromotionsAsync();
            }

            return this;
        }

        public async Task<CartBuilder> AddCouponAsync(string couponCode)
        {
            _cart.Coupon = new Coupon
            {
                Code = couponCode
            };

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> RemoveCouponAsync()
        {
            _cart.Coupon = null;

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> AddAddressAsync(Address address)
        {
            var existingAddress = _cart.Addresses.FirstOrDefault(a => a.Type == address.Type);
            if (existingAddress != null)
            {
                _cart.Addresses.Remove(existingAddress);
            }

            _cart.Addresses.Add(address);

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> AddShipmentAsync(ShippingMethod shippingMethod)
        {
            var shipment = shippingMethod.ToShipmentModel(_currency);

            _cart.Shipments.Clear();
            _cart.Shipments.Add(shipment);

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> AddPaymentAsync(PaymentMethod paymentMethod)
        {
            var payment = paymentMethod.ToPaymentModel(_cart.Total, _currency);

            _cart.Payments.Clear();
            _cart.Payments.Add(payment);

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> MergeWithCartAsync(ShoppingCart cart)
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

            return this;
        }

        public async Task SaveAsync()
        {
            var cart = _cart.ToServiceModel();

            //Invalidate cart in cache
            _cacheManager.Remove(GetCartCacheKey(cart.StoreId, cart.CustomerId), "CartRegion");

            if (_cart.IsTransient())
            {
                await _cartApi.CartModuleCreateAsync(cart);
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

        private void AddLineItem(LineItem lineItem)
        {
            var existingLineItem = _cart.Items.FirstOrDefault(li => li.Sku == lineItem.Sku);
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

        private ShoppingCart CreateNewTransientCart()
        {
            var cart = new ShoppingCart(_currency, _language);

            cart.CustomerId = _customer.Id;
            cart.Name = "Default";
            cart.StoreId = _store.Id;

            if (_customer.UserName.Equals("Anonymous", StringComparison.OrdinalIgnoreCase))
            {
                cart.CustomerName = "Anonymous";
            }
            else
            {
                cart.CustomerName = string.Format("{0} {1}", _customer.FirstName, _customer.LastName);
            }

            return cart;
        }

        private string GetCartCacheKey(string storeId, string customerId)
        {
            return String.Format("Cart-{0}-{1}", storeId, customerId);
        }

        private async Task EvaluatePromotionsAsync()
        {
            _cart.Discounts.Clear();

            foreach (var lineItem in _cart.Items)
            {
                lineItem.Discounts.Clear();
            }

            foreach (var shipment in _cart.Shipments)
            {
                shipment.Discounts.Clear();
            }

            CalculateTotals();

            var promotionContext = new PromotionEvaluationContext
            {
                CartTotal = (double)_cart.Total.Amount,
                Coupon = _cart.Coupon != null ? _cart.Coupon.Code : null,
                Currency = _cart.Currency,
                CustomerId = _customer.Id,
                StoreId = _store.Id
            };

            promotionContext.CartPromoEntries = _cart.Items.Select(i => i.ToPromotionItem()).ToList();
            promotionContext.PromoEntries = promotionContext.CartPromoEntries;

            var rewards = await _marketingService.EvaluatePromotionRewardsAsync(promotionContext);
            foreach (var reward in rewards)
            {
                if (reward.RewardType == PromotionRewardType.CatalogItemAmountReward && reward.IsValid)
                {
                    var lineItem = _cart.Items.FirstOrDefault(i => i.ProductId == reward.ProductId);
                    if (lineItem != null)
                    {
                        var discount = reward.ToDiscountWebModel(lineItem.ExtendedPrice.Amount, _cart.Currency);
                        lineItem.Discounts.Add(discount);
                    }
                }

                if (reward.RewardType == PromotionRewardType.ShipmentReward && reward.IsValid)
                {
                    var shipment = _cart.Shipments.FirstOrDefault();
                    if (shipment != null)
                    {
                        var discount = reward.ToDiscountWebModel(shipment.ShippingPrice.Amount, _cart.Currency);
                        shipment.Discounts.Add(discount);
                    }
                }

                if (reward.RewardType == PromotionRewardType.CartSubtotalReward && reward.IsValid)
                {
                    var discount = reward.ToDiscountWebModel(_cart.SubTotal.Amount, _cart.Currency);
                    _cart.Discounts.Add(discount);
                }

                if (reward.Promotion.Coupons != null && reward.Promotion.Coupons.Any() && !string.IsNullOrEmpty(promotionContext.Coupon))
                {
                    var discount = reward.ToDiscountWebModel(_cart.SubTotal.Amount, _cart.Currency);
                    _cart.Coupon = new Coupon
                    {
                        Amount = discount.Amount,
                        AppliedSuccessfully = reward.IsValid,
                        Code = promotionContext.Coupon,
                        Description = reward.Promotion.Description,
                        ErrorCode = reward.IsValid ? null : "InvalidCouponCode"
                    };
                }
            }

            CalculateTotals();
        }

        private void CalculateTotals()
        {
            var cartDiscountTotal = new Money(_currency.Code);
            foreach (var discount in _cart.Discounts.ToList())
            {
                cartDiscountTotal += discount.Amount;
            }

            var lineItemsDiscountTotal = new Money(_currency.Code);
            foreach (var lineItem in _cart.Items.ToList())
            {
                foreach (var discount in lineItem.Discounts.ToList())
                {
                    lineItemsDiscountTotal += discount.Amount;
                }
            }

            var shipmentsDiscountTotal = new Money(_currency.Code);
            foreach (var shipment in _cart.Shipments.ToList())
            {
                foreach (var discount in shipment.Discounts.ToList())
                {
                    shipmentsDiscountTotal += discount.Amount;
                }
            }

            _cart.DiscountTotal = cartDiscountTotal + lineItemsDiscountTotal + shipmentsDiscountTotal;
            _cart.ShippingTotal = new Money(_cart.Shipments.Sum(s => s.ShippingPrice.Amount), _currency.Code);
            _cart.SubTotal = new Money(_cart.Items.Sum(i => i.ExtendedPrice.Amount), _currency.Code) - lineItemsDiscountTotal;
            _cart.Total = _cart.SubTotal + _cart.ShippingTotal + _cart.TaxTotal - _cart.DiscountTotal;
        }
    }
}