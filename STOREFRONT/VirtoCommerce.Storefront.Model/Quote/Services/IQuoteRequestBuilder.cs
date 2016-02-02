using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Quote.Services
{
    public interface IQuoteRequestBuilder
    {
        IQuoteRequestBuilder TakeQuoteRequest(QuoteRequest cart);

        Task<IQuoteRequestBuilder> GetOrCreateNewTransientQuoteRequestAsync(Store store, CustomerInfo customer, Language language, Currency currency);

        IQuoteRequestBuilder Update(QuoteRequestFormModel quoteRequest);

        IQuoteRequestBuilder AddItem(Product product, long quantity);

        IQuoteRequestBuilder RemoveItem(string quoteItemId);

        Task <IQuoteRequestBuilder> MergeWithQuoteRequest(QuoteRequest quoteRequest);

        Task SaveAsync();

        QuoteRequest QuoteRequest { get; }
    }
}