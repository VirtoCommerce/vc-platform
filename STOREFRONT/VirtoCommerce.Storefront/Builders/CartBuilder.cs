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
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICacheManager<object> _cacheManager;

        private Store _store;
        private Customer _customer;
        private Currency _currency;
        private Language _language;
        private ShoppingCart _cart;
        private string _cartCacheKey;
        private const string _cartCacheRegion = "CartRegion";

        [CLSCompliant(false)]
        public CartBuilder(IShoppingCartModuleApi cartApi, IPromotionEvaluator promotionEvaluator, ICacheManager<object> cacheManager)
        {
            _cartApi = cartApi;
            _promotionEvaluator = promotionEvaluator;
            _cacheManager = cacheManager;
        }

        public async Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Language language, Currency currency)
        {
            _store = store;
            _customer = customer;
            _currency = currency;
            _language = language;
            _cartCacheKey = GetCartCacheKey(store.Id, customer.Id);

            _cart = await _cacheManager.GetAsync(_cartCacheKey, _cartCacheRegion, async () =>
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

            _cart.Items.OrderBy(i => i.CreatedDate);

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
                }
                else
                {
                    _cart.Items.Remove(lineItem);
                }

                await EvaluatePromotionsAsync();
            }

            return this;
        }

        public async Task<CartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity)
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

        public async Task<CartBuilder> ChangeItemsQuantitiesAsync(int[] quantities)
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

        public async Task<CartBuilder> ClearAsync()
        {
            _cart.Items.Clear();

            await EvaluatePromotionsAsync();

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
            _cacheManager.Remove(_cartCacheKey, _cartCacheRegion);

            return this;
        }

        public async Task<CartBuilder> RemoveCartAsync()
        {
            await _cartApi.CartModuleDeleteCartsAsync(new List<string> { _cart.Id });
            _cacheManager.Remove(_cartCacheKey, _cartCacheRegion);

            return this;
        }

        public async Task SaveAsync()
        {
            var cart = _cart.ToServiceModel();

            //Invalidate cart in cache
            _cacheManager.Remove(_cartCacheKey, _cartCacheRegion);

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

        private ShoppingCart CreateNewTransientCart()
        {
            var cart = new ShoppingCart(_currency, _language);

            cart.CustomerId = _customer.Id;
            cart.Name = "Default";
            cart.StoreId = _store.Id;

            if (_customer.UserName.Equals(StorefrontConstants.AnonymousUsername, StringComparison.OrdinalIgnoreCase))
            {
                cart.CustomerName = StorefrontConstants.AnonymousUsername;
            }
            else
            {
                cart.CustomerName = String.Format("{0} {1}", _customer.FirstName, _customer.LastName);
            }

            return cart;
        }

        private string GetCartCacheKey(string storeId, string customerId)
        {
            return String.Format("Cart-{0}-{1}", storeId, customerId);
        }

        private async Task EvaluatePromotionsAsync()
        {
            var promotionItems = _cart.Items.Select(i => i.ToPromotionItem()).ToList();

            var promotionContext = new PromotionEvaluationContext();
            promotionContext.CartPromoEntries = promotionItems;
            promotionContext.CartTotal = _cart.Total;
            promotionContext.Coupon = _cart.Coupon != null ? _cart.Coupon.Code : null;
            promotionContext.Currency = _cart.Currency;
            promotionContext.CustomerId = _customer.Id;
            promotionContext.IsRegisteredUser = _customer.HasAccount;
            promotionContext.Language = _language;
            promotionContext.PromoEntries = promotionItems;
            promotionContext.StoreId = _store.Id;

            var owners = new List<IDiscountable>();
            owners.Add(_cart);

            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, new IDiscountable[] { _cart });

            CalculateTotals();
        }

        private void CalculateTotals()
        {
            foreach (var lineItem in _cart.Items)
            {
                decimal lineItemDiscountTotal = lineItem.Discounts.Sum(d => d.Amount.Amount);
                lineItem.DiscountTotal = new Money(lineItemDiscountTotal, _cart.Currency.Code);

                decimal lineItemTaxTotal = lineItem.TaxDetails.Sum(td => td.Amount.Amount);
                lineItem.TaxTotal = new Money(lineItemTaxTotal, _cart.Currency.Code);

                decimal placedPrice = lineItem.SalePrice.Amount - lineItemDiscountTotal;
                lineItem.PlacedPrice = new Money(placedPrice, _cart.Currency.Code);

                decimal extendedPrice = placedPrice * lineItem.Quantity;
                lineItem.ExtendedPrice = new Money(extendedPrice, _cart.Currency.Code);
            }

            foreach (var shipment in _cart.Shipments)
            {
                decimal shipmentDiscountTotal = shipment.Discounts.Sum(d => d.Amount.Amount);
                shipment.DiscountTotal = new Money(shipmentDiscountTotal, _cart.Currency.Code);

                decimal shipmentTaxTotal = shipment.TaxDetails.Sum(td => td.Amount.Amount);
                shipment.TaxTotal = new Money(shipmentTaxTotal, _cart.Currency.Code);

                decimal shipmentItemsSubtotal = shipment.Items.Sum(i => i.ExtendedPrice.Amount);
                shipment.ItemSubtotal = new Money(shipmentItemsSubtotal, _cart.Currency.Code);

                shipment.Subtotal = shipment.ShippingPrice - shipmentDiscountTotal;
                shipment.Total = shipment.Subtotal + shipment.TaxTotal;
            }

            decimal cartDiscountsTotal = _cart.Discounts.Sum(d => d.Amount.Amount);
            _cart.DiscountTotal = new Money(cartDiscountsTotal, _cart.Currency.Code);

            decimal cartSubtotal = _cart.Items.Sum(i => i.ExtendedPrice.Amount);
            _cart.SubTotal = new Money(cartSubtotal, _cart.Currency.Code);

            decimal cartShipmentsTotal = _cart.Shipments.Sum(s => s.Total.Amount);
            _cart.ShippingTotal = new Money(cartShipmentsTotal, _cart.Currency.Code);

            //decimal cartTaxTotal = _cart.TaxDetails.Sum(td => td.Amount.Amount);
            //_cart.TaxTotal = new Money(cartTaxTotal, _cart.Currency.Code);

            _cart.Total = _cart.SubTotal + _cart.ShippingTotal + _cart.TaxTotal - _cart.DiscountTotal;
        }
    }
}