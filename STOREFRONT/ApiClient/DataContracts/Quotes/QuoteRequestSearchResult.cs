using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class QuoteRequestSearchResult
    {
        public ICollection<QuoteRequest> QuoteRequests { get; set; }

        public int TotalCount { get; set; }
    }
}