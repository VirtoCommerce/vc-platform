using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Quote.Services
{
    /// <summary>
    /// Represent abstraction for constructing and working with request for quote (RFQ)
    /// </summary>
    public interface IQuoteRequestBuilder
    {
        /// <summary>
        /// Load quotes from service by number or id and capture it for next changes
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<IQuoteRequestBuilder> LoadQuoteRequestAsync(string number, Language language, IEnumerable<Currency> availCurrencies);

        /// <summary>
        /// Capture passed RFQ and all next changes will be implemented on it
        /// </summary>
        /// <param name="quoteRequest"></param>
        /// <returns></returns>
        IQuoteRequestBuilder TakeQuoteRequest(QuoteRequest quoteRequest);

        /// <summary>
        /// Load or created new RFQ for current user and capture it
        /// </summary>
        /// <param name="store"></param>
        /// <param name="customer"></param>
        /// <param name="language"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        Task<IQuoteRequestBuilder> GetOrCreateNewTransientQuoteRequestAsync(Store store, CustomerInfo customer, Language language, Currency currency);

        /// <summary>
        /// Update captured RFQ
        /// </summary>
        /// <param name="quoteRequest"></param>
        /// <returns></returns>
        IQuoteRequestBuilder Update(QuoteRequestFormModel quoteRequest);

        /// <summary>
        /// Adding new item to captured RFQ
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        IQuoteRequestBuilder AddItem(Product product, long quantity);

        /// <summary>
        /// Submit captured RFQ
        /// </summary>
        /// <returns></returns>
        IQuoteRequestBuilder Submit();

        /// <summary>
        /// Reject captured RFQ
        /// </summary>
        /// <returns></returns>
        IQuoteRequestBuilder Reject();

        /// <summary>
        /// Confirm captured RFQ
        /// </summary>
        /// <returns></returns>
        IQuoteRequestBuilder Confirm();

        /// <summary>
        /// Remove item from captured RFQ
        /// </summary>
        /// <param name="quoteItemId"></param>
        /// <returns></returns>
        IQuoteRequestBuilder RemoveItem(string quoteItemId);

        /// <summary>
        /// Merge captured RFQ by other RFQ
        /// </summary>
        /// <param name="otherQuoteRequest"></param>
        /// <returns></returns>
        Task <IQuoteRequestBuilder> MergeFromOtherAsync(QuoteRequest otherQuoteRequest);

        /// <summary>
        /// Calculated totals for current quote
        /// </summary>
        /// <returns></returns>
        Task<IQuoteRequestBuilder> CalculateTotalsAsync();
        /// <summary>
        /// Save captured RFQ changes
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Return captured RFQ
        /// </summary>
        QuoteRequest QuoteRequest { get; }
    }
}