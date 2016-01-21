using System;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteItem
    {
        public QuoteItem()
        {
            ProposalPrices = new List<TierPrice>();
        }

        public Currency Currency { get; set; }

        public Money ListPrice { get; set; }

        public Money SalePrice { get; set; }

        public string ProductId { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public string Sku { get; set; }

        public string TaxType { get; set; }

        public TierPrice SelectedTierPrice { get; set; }

        public ICollection<TierPrice> ProposalPrices { get; set; }


        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string Id { get; set; }
    }
}