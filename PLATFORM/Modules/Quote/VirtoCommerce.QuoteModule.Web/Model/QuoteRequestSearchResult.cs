using System.Collections.Generic;

namespace VirtoCommerce.QuoteModule.Web.Model
{
    public class QuoteRequestSearchResult
    {
        public int TotalCount { get; set; }
        public ICollection<QuoteRequest> QuoteRequests { get; set; }
    }
}
