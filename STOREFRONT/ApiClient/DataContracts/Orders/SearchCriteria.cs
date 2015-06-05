namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class SearchCriteria
    {
        public ResponseGroup ResponseGroup { get; set; }

        public string Keyword { get; set; }

        public string CustomerId { get; set; }

        public string StoreId { get; set; }

        public int Start { get; set; }

        public int Count { get; set; }
    }
}