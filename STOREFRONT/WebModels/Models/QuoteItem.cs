using DotLiquid;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models
{
    public class QuoteItem : Drop
    {
        public QuoteItem()
        {
            ProposalPrices = new List<TierPrice>();
        }

        public string Id { get; set; }

        public decimal ListPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string ProductId { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public TierPrice SelectedTierPrice { get; set; }

        public ICollection<TierPrice> ProposalPrices { get; set; }
    }
}