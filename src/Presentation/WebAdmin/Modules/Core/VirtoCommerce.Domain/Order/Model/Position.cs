using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Position : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string OperationId { get; set; }
		
		/// <summary>
		/// Price with tax and without dicount
		/// </summary>
		public decimal BasePrice { get; set; }
		/// <summary>
		/// Price with tax and discount
		/// </summary>
		public decimal Price { get; set; }
		/// <summary>
		/// Discount amount
		/// </summary>
		public decimal Discount { get; set; }
		/// <summary>
		/// Tax sum
		/// </summary>
		public decimal Tax { get; set; }

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
