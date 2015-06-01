using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Model
{
	public class LineItem : AuditableEntity
	{
		public string ProductId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductCode { get; set; }

		public string Name { get; set; }
		public int Quantity { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		public string WarehouseLocation { get; set; }
		public string ShipmentMethodCode { get; set; }
		public bool RequiredShipping { get; set; }
		public string ThumbnailImageUrl { get; set; }
		public string ImageUrl { get; set; }

		public bool IsGift { get; set; }

		public ICollection<Discount> Discounts { get; set; }

		public string LanguageCode { get; set; }

		public string Comment { get; set; }

		public bool IsReccuring { get; set; }

		public bool TaxIncluded { get; set; }

		public decimal? VolumetricWeight { get; set; }
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }


		public decimal ListPrice { get; set; }
		public decimal SalePrice { get; set; }
		public decimal PlacedPrice { get; set; }
		public decimal ExtendedPrice { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }

	}
}
