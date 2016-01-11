using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class QuoteItem : AuditableEntity
	{
		public string Currency { get; set; }
        /// <summary>
        /// Base catalog price
        /// </summary>
        public decimal ListPrice { get; set; }
        /// <summary>
        /// Sale price for buyer
        /// </summary>
        public decimal SalePrice { get; set; }

        public string Sku { get; set; }
        public string ProductId { get; set; }
		public CatalogProduct Product { get; set; }

		public string CatalogId { get; set; }
		public string CategoryId { get; set; }

		public string Name { get; set; }

		public string Comment { get; set; }

		public string ImageUrl { get; set; }

        public string TaxType { get; set; }

        private TierPrice _selectedTierPrice;
        public TierPrice SelectedTierPrice
        {
            get
            {
                if(_selectedTierPrice == null && ProposalPrices != null)
                {
                    _selectedTierPrice = ProposalPrices.FirstOrDefault();
                }
                return _selectedTierPrice;
            }

            set
            {
                _selectedTierPrice = value;
            }
        }

		public ICollection<TierPrice> ProposalPrices { get; set; }
	}
}
