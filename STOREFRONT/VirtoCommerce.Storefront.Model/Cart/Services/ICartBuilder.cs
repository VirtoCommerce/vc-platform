using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Model.Cart.Services
{
    /// <summary>
    /// Represent abstraction for working with customer shopping cart
    /// </summary>
    public interface ICartBuilder
    {
        /// <summary>
        ///  Capture passed cart and all next changes will be implemented on it
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        ICartBuilder TakeCart(ShoppingCart cart);

        /// <summary>
        /// Load or created new cart for current user and capture it
        /// </summary>
        /// <param name="store"></param>
        /// <param name="customer"></param>
        /// <param name="language"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        Task<ICartBuilder> GetOrCreateNewTransientCartAsync(Store store, CustomerInfo customer, Language language, Currency currency);

        /// <summary>
        /// Add new product to cart
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<ICartBuilder> AddItemAsync(Product product, int quantity);

        /// <summary>
        /// Change cart item qty by product index
        /// </summary>
        /// <param name="lineItemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<ICartBuilder> ChangeItemQuantityAsync(string lineItemId, int quantity);

        /// <summary>
        /// Change cart item qty by item id
        /// </summary>
        /// <param name="lineItemIndex"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<ICartBuilder> ChangeItemQuantityAsync(int lineItemIndex, int quantity);

        Task<ICartBuilder> ChangeItemsQuantitiesAsync(int[] quantities);

        /// <summary>
        /// Remove item from cart by id
        /// </summary>
        /// <param name="lineItemId"></param>
        /// <returns></returns>
        Task<ICartBuilder> RemoveItemAsync(string lineItemId);

        /// <summary>
        /// Apply marketing coupon to captured cart
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        Task<ICartBuilder> AddCouponAsync(string couponCode);

        /// <summary>
        /// remove exist coupon from cart
        /// </summary>
        /// <returns></returns>
        Task<ICartBuilder> RemoveCouponAsync();

        /// <summary>
        /// Clear cart remove all items and shipments and payments
        /// </summary>
        /// <returns></returns>
        Task<ICartBuilder> ClearAsync();

        /// <summary>
        /// Add or update shipment to cart
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="shippingAddress"></param>
        /// <param name="itemIds"></param>
        /// <param name="shippingMethodCode"></param>
        /// <returns></returns>
        Task<ICartBuilder> AddOrUpdateShipmentAsync(ShipmentUpdateModel updateModel);

        /// <summary>
        /// Remove exist shipment from cart
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        Task<ICartBuilder> RemoveShipmentAsync(string shipmentId);

        /// <summary>
        /// Add or update payment in cart
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="billingAddress"></param>
        /// <param name="paymentMethodCode"></param>
        /// <param name="outerId"></param>
        /// <returns></returns>
        Task<ICartBuilder> AddOrUpdatePaymentAsync(PaymentUpdateModel updateModel);

        /// <summary>
        /// Merge other cart with captured
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        Task<ICartBuilder> MergeWithCartAsync(ShoppingCart cart);

        /// <summary>
        /// Remove cart from service
        /// </summary>
        /// <returns></returns>
        Task<ICartBuilder> RemoveCartAsync();

        /// <summary>
        /// Fill current captured cart from RFQ
        /// </summary>
        /// <param name="quoteRequest"></param>
        /// <returns></returns>
        Task<ICartBuilder> FillFromQuoteRequest(QuoteRequest quoteRequest);

        /// <summary>
        /// Returns all available shipment methods for current cart
        /// </summary>
        /// <returns></returns>
        Task<ICollection<ShippingMethod>> GetAvailableShippingMethodsAsync();

        /// <summary>
        /// Returns all available payment methods for current cart
        /// </summary>
        /// <returns></returns>
        Task<ICollection<PaymentMethod>> GetAvailablePaymentMethodsAsync();

        /// <summary>
        /// Evaluate marketing discounts for captured cart
        /// </summary>
        /// <returns></returns>
        Task<ICartBuilder> EvaluatePromotionsAsync();

        /// <summary>
        /// Evaluate taxes  for captured cart
        /// </summary>
        /// <returns></returns>
        Task<ICartBuilder> EvaluateTaxAsync();

        //Save cart changes
        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}