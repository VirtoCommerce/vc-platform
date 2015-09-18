namespace VirtoCommerce.Web.Models
{
    public class QuoteRequestSearchCriteria
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public string StoreId { get; set; }

        public string CustomerId { get; set; }

        public string Tag { get; set; }

        public string Status { get; set; }
    }
}