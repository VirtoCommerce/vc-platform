using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class QuoteItem
    {
        public string Id { get; set; }

        public decimal ListPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string ProductId { get; set; }

        public CatalogItem Product { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public string TaxType { get; set; }

        public TierPrice SelectedTierPrice { get; set; }

        public ICollection<TierPrice> ProposalPrices { get; set; }
    }
}