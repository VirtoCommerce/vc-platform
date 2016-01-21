using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Cart.Services
{
    public interface ICartBuilder
    {
        Task<ICartBuilder> GetOrCreateNewTransientCartAsync(Store store, CustomerInfo customer, Language language, Currency currency);

        Task<ICartBuilder> AddItemAsync(Product product, int quantity);

        Task<ICartBuilder> ChangeItemQuantityAsync(string lineItemId, int quantity);

        Task<ICartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity);

        Task<ICartBuilder> ChangeItemsQuantitiesAsync(int[] quantities);

        Task<ICartBuilder> RemoveItemAsync(string lineItemId);

        Task<ICartBuilder> AddCouponAsync(string couponCode);

        Task<ICartBuilder> RemoveCouponAsync();

        Task<ICartBuilder> ClearAsync();

        Task<ICartBuilder> AddAddressAsync(Address address);

        Task<ICartBuilder> AddShipmentAsync(ShippingMethod shippingMethod);

        Task<ICartBuilder> AddPaymentAsync(PaymentMethod paymentMethod);

        Task<ICartBuilder> MergeWithCartAsync(ShoppingCart cart);

        Task<ICartBuilder> RemoveCartAsync();

        Task<ICollection<ShippingMethod>> GetAvailableShippingMethodsAsync();

        Task<ICollection<PaymentMethod>> GetAvailablePaymentMethodsAsync();

        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}