using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class LineItem
	{
		public string Id { get; set; }

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

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

		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		/// <summary>
		/// Reserve quantity
		/// </summary>
		public int ReserveQuantity { get; set; }
		public int Quantity { get; set; }

		public string ProductId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }

		public string Name { get; set; }

		public string ImageUrl { get; set; }

		public string DisplayName { get; set; }

		public bool IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfilmentLocationCode { get; set; }

		public Discount Discount { get; set; }


		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }
	}
}