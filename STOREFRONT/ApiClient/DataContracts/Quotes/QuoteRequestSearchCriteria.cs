using System;

namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class QuoteRequestSearchCriteria
    {
        public string Keyword { get; set; }

        public string CustomerId { get; set; }

        public string StoreId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Start { get; set; }

        public int Count { get; set; }
    }
}