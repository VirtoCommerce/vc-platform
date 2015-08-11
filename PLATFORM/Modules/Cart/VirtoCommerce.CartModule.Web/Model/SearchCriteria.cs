namespace VirtoCommerce.CartModule.Web.Model
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Count = 20;
        }

        /// <summary>
        /// Gets or sets the value of search criteria keyword
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria skip records count
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria page size
        /// </summary>
        public int Count { get; set; }
    }
}