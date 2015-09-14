using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Web.Model
{
	public class QuoteItem : AuditableEntity
	{
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes Currency { get; set; }

        /// <summary>
        /// Price with tax and without dicount
        /// </summary>
        public decimal BasePrice { get; set; }
        /// <summary>
        /// Price with tax and discount
        /// </summary>
        public decimal Price { get; set; }

		public string ProductId { get; set; }
		public CatalogProduct Product { get; set; }

		public string CatalogId { get; set; }
		public string CategoryId { get; set; }

		public string Name { get; set; }

		public string Comment { get; set; }

		public string ImageUrl { get; set; }

        /// <summary>
        /// Selected proposal tier price
        /// </summary>
        public TierPrice SelectedPrice { get; set; }
        /// <summary>
        /// Proposal tier prices
        /// </summary>
		public ICollection<TierPrice> ProposalPrices { get; set; }
	}
}
