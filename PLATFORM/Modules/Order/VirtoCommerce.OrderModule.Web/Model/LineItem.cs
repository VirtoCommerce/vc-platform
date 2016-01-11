using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class LineItem : AuditableEntity, IHaveTaxDetalization, ISupportCancellation
	{
        /// <summary>
        /// Price id which that was used in the formation of this line item
        /// </summary>
        public string PriceId { get; set; }
        /// <summary>
        /// Price with tax and without dicount
        /// </summary>
        public decimal BasePrice { get; set; }
		/// <summary>
		/// Price with tax and discount
		/// </summary>
		public decimal Price { get; set; }
		/// <summary>
		/// discount amount
		/// </summary>
		public decimal DiscountAmount { get; set; }
		/// <summary>
		/// Tax sum
		/// </summary>
		public decimal Tax { get; set; }

		public string Currency { get; set; }
		/// <summary>
		/// Reserve quantity
		/// </summary>
		public int ReserveQuantity { get; set; }
		public int Quantity { get; set; }

		public string ProductId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
        public string Sku { get; set; }
        public string ProductType { get; set; }

        public string Name { get; set; }

		public string ImageUrl { get; set; }

		public string DisplayName { get; set; }

		public bool IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfilmentLocationCode { get; set; }

		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public string TaxType { get; set; }
		#region ISupportCancelation Members
		/// <summary>
		/// Flag represent that line item was canceled
		/// </summary>
		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		/// <summary>
		/// Text representation of cancel reason
		/// </summary>
		public string CancelReason { get; set; }

		#endregion

		public Discount Discount { get; set; }
		public ICollection<TaxDetail> TaxDetails { get; set; }

		/// <summary>
		/// Used for dynamic properties management, contains object type string
		/// </summary>
		public string ObjectType { get; set; }
		/// <summary>
		/// Dynamic properties collections
		/// </summary>
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
	}
}