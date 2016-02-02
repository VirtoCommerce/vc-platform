using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote;

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

        Task<ICartBuilder> AddOrUpdateShipmentAsync(string shipmentId, Address shippingAddress, ICollection<string> itemIds, string shippingMethodCode);

        Task<ICartBuilder> RemoveShipmentAsync(string shipmentId);

        Task<ICartBuilder> AddOrUpdatePaymentAsync(string paymentId, Address billingAddress, string paymentMethodCode, string outerId);

        Task<ICartBuilder> MergeWithCartAsync(ShoppingCart cart);

        Task<ICartBuilder> RemoveCartAsync();

        Task<ICartBuilder> FillFromQuoteRequest(QuoteRequest quoteRequest);

        Task<ICollection<ShippingMethod>> GetAvailableShippingMethodsAsync();

        Task<ICollection<PaymentMethod>> GetAvailablePaymentMethodsAsync();

        Task<ICartBuilder> EvaluatePromotionsAsync();

        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}