﻿using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public interface ICartBuilder
    {
        Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Language language, Currency currency);

        Task<CartBuilder> AddItemAsync(Product product, int quantity);

        Task<CartBuilder> ChangeItemQuantityAsync(string lineItemId, int quantity);

        Task<CartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity);

        Task<CartBuilder> ChangeItemsQuantitiesAsync(int[] quantities);

        Task<CartBuilder> RemoveItemAsync(string lineItemId);

        Task<CartBuilder> AddCouponAsync(string couponCode);

        Task<CartBuilder> RemoveCouponAsync();

        Task<CartBuilder> ClearAsync();

        Task<CartBuilder> AddAddressAsync(Address address);

        Task<CartBuilder> AddShipmentAsync(ShippingMethod shippingMethod);

        Task<CartBuilder> AddPaymentAsync(PaymentMethod paymentMethod);

        Task<CartBuilder> MergeWithCartAsync(ShoppingCart cart);

        Task<CartBuilder> RemoveCartAsync();

        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}