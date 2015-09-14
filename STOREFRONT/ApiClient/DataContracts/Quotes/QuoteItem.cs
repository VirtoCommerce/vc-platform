using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class QuoteItem
    {
        public string Id { get; set; }

        public string Currency { get; set; }

        public decimal BasePrice { get; set; }

        public decimal Price { get; set; }

        public string ProductId { get; set; }

        public CatalogItem Product { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public TierPrice SelectedPrice { get; set; }

        public ICollection<TierPrice> ProposalPrices { get; set; }
    }
}