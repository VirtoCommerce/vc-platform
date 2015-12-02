using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IMarketingModuleApi _marketingApi;

        private Store _store;
        private Customer _customer;
        private Currency _currency;

        private ShoppingCart _cart;

        public CartBuilder(
            IShoppingCartModuleApi cartApi,
            IMarketingModuleApi marketinApi)
        {
            _cartApi = cartApi;
            _marketingApi = marketinApi;
        }

        public async Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Currency currency)
        {
            VirtoCommerceCartModuleWebModelShoppingCart cart = null;

            _store = store;
            _customer = customer;
            _currency = currency;

            cart = await _cartApi.CartModuleGetCurrentCartAsync(_store.Id, _customer.Id);
            if (cart == null)
            {
                _cart = new ShoppingCart(_store.Id, _customer.Id, _customer.Name, "Default", _currency.Code);
            }
            else
            {
                _cart = cart.ToWebModel();
            }

            await EvaluatePromotionsAsync();

            return this;
        }

        public async Task<CartBuilder> AddItemAsync(Product product, int quantity)
        {
            AddLineItem(product.ToLineItem(quantity));

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
                AppliedSuccessfully = false,
                Code = couponCode
            };

            var validRewards = await EvaluatePromotionsAsync();

            var couponReward = validRewards.FirstOrDefault(r => r.Promotion != null && r.Promotion.Coupons.Any());
            if (couponReward != null)
            {
                _cart.Coupon.Amount = new Money(couponReward.Amount ?? 0, _currency.Code);
                _cart.Coupon.AppliedSuccessfully = true;
            }

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

            return this;
        }

        public async Task SaveAsync()
        {
            var cart = _cart.ToServiceModel();

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

        private async Task<IEnumerable<VirtoCommerceMarketingModuleWebModelPromotionReward>> EvaluatePromotionsAsync()
        {
            var promotionContext = new VirtoCommerceDomainMarketingModelPromotionEvaluationContext
            {
                Coupon = _cart.Coupon != null ? _cart.Coupon.Code : null,
                CustomerId = _customer.Id,
                StoreId = _store.Id
            };

            promotionContext.CartPromoEntries = _cart.Items.Select(i => i.ToPromotionItem()).ToList();
            promotionContext.PromoEntries = promotionContext.CartPromoEntries;

            var rewards = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(promotionContext);
            var validRewards = rewards.Where(pr => pr.IsValid.HasValue && pr.IsValid.Value);
            _cart.Discounts = validRewards.Select(r => r.ToDiscountWebModel(_currency)).ToList();

            return validRewards;
        }
    }
}