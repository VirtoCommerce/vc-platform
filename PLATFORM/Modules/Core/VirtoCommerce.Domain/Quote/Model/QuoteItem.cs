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
		public CurrencyCodes Currency { get; set; }
		/// <summary>
		/// Price with tax and without discount
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

		public ICollection<TierPrice> TierPrices { get; set; }
	}
}
