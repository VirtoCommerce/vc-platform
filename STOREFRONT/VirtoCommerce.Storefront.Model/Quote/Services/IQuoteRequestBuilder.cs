using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Quote.Services
{
    public interface IQuoteRequestBuilder
    {
        IQuoteRequestBuilder TakeQuoteRequest(QuoteRequest quoteRequest);

        Task<IQuoteRequestBuilder> GetOrCreateNewTransientQuoteRequestAsync(Store store, CustomerInfo customer, Language language, Currency currency);

        IQuoteRequestBuilder Update(QuoteRequestFormModel quoteRequest);

        IQuoteRequestBuilder AddItem(Product product, long quantity);

        IQuoteRequestBuilder Reject();

        IQuoteRequestBuilder RemoveItem(string quoteItemId);

        Task <IQuoteRequestBuilder> MergeWithQuoteRequest(QuoteRequest otherQuoteRequest);

        Task SaveAsync();

        QuoteRequest QuoteRequest { get; }
    }
}