using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Order.Model
{
	public class LineItem : AuditableEntity, IPosition, IHaveTaxDetalization, ISupportCancellation, IHaveDimension, IHasDynamicProperties
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
		/// <summary>
		/// Static discount amount
		/// </summary>
		public decimal DiscountAmount { get; set; }
		/// <summary>
		/// Tax sum
		/// </summary>
		public decimal Tax { get; set; }

		/// <summary>
		/// Tax category or type
		/// </summary>
		public string TaxType { get; set; }

		/// <summary>
		/// Reserve quantity
		/// </summary>
		public int ReserveQuantity { get; set; }
		public int Quantity { get; set; }

		public string ProductId { get; set; }
		public CatalogProduct Product { get; set; }
 		public string CatalogId { get; set; }
		public string CategoryId { get; set; }

		public string Name { get; set; }

		public string Comment { get; set; }

		public string ImageUrl { get; set; }

		public bool? IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfillmentLocationCode { get; set; }

		#region IHaveDimension Members
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; } 
		#endregion

		#region ISupportCancelation Members

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }

		#endregion

		#region IHasDynamicProperties Members
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
		#endregion

		public Discount Discount { get; set; }
		public ICollection<TaxDetail> TaxDetails { get; set; }
	}
}
