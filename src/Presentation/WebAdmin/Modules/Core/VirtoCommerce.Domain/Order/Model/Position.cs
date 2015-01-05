using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Position 
	{
		/// <summary>
		/// Price with tax and without dicount
		/// </summary>
		Money BasePrice { get; set; }
		/// <summary>
		/// Price with tax and discount
		/// </summary>
		Money Price { get; set; }
		/// <summary>
		/// Discount amount
		/// </summary>
		Money Discount { get; set; }
		/// <summary>
		/// Tax sum
		/// </summary>
		Money Tax { get; set; }

		/// <summary>
		/// Reserve quantity
		/// </summary>
		public long ReserveQuantity { get; set; }
		public long Quantity { get; set; }

		public string ItemId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }

		public string Name { get; set; }
		
		public string ImageUrl { get; set; }
	}
}
