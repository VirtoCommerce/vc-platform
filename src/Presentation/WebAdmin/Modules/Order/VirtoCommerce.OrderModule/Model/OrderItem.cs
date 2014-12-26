using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Model
{
	public class OrderItem : AuditableEntityBase<OrderItem>
	{
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
		public string DisplayName { get; set; }
		public int Quantity { get; set; }
		public string PreviewImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
		public decimal Weight { get; set; }
		public bool IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfilmentLocationCode { get; set; }

		/// <summary>
		/// Base price
		/// </summary>
		public Money ListPrice { get; set; }
		/// <summary>
		/// Real price for sale
		/// </summary>
		public Money SalePrice { get; set; }
		/// <summary>
		/// Overrided price
		/// </summary>
		public Money PlacedPrice { get; set; }
		/// <summary>
		/// Price of the single item times the quanity  without sales tax, shipping costs, and other fees
		/// </summary>
		public Money ExtPrice { get; set; }
		/// <summary>
		/// Total is used to indicate the monetary, estimated total items with sales tax, shipping costs, and other fees and discount. 
		/// </summary>
		public Money Total { get; set; }
		/// <summary>
		/// Estimated amount of the items without sales tax, shipping costs, discounts and other fees. 
		/// </summary>
		public Money SubTotal { get; set; }
		/// <summary>
		/// Estimated amount of the discounts
		/// </summary>
		public Money DiscountTotal { get; set; }
		public Money TaxTotal { get; set; }
		public Money ShippingTotal { get; set; }

		public ICollection<Discount> Discounts { get; set; }

	}
}
