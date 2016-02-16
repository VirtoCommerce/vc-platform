namespace VirtoCommerce.Storefront.Model.Quote.Events
{
    public class QuoteRequestUpdatedEvent
    {
        public QuoteRequestUpdatedEvent(QuoteRequest quoteRequest)
        {
            QuoteRequest = quoteRequest;
        }

        public QuoteRequest QuoteRequest { get; set; }
    }
}