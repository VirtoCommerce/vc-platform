using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Model
{
	public class Shipment : AuditableEntity, IHaveTaxDetalization
	{
		public string ShipmentMethodCode { get; set; }
		public string FulfilmentCenterId { get; set; }
		public Address DeliveryAddress { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }


		public decimal? VolumetricWeight { get; set; }
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }


		public bool TaxIncluded { get; set; }

		public decimal ShippingPrice { get; set; }
		public decimal Total { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
		public decimal ItemSubtotal { get; set; }
		public decimal Subtotal { get; set; }

		public ICollection<Discount> Discounts { get; set; }

		public ICollection<LineItem> Items { get; set; }

		public string TaxType { get; set; }

		#region IHaveTaxDetalization Members
		public ICollection<TaxDetail> TaxDetails { get; set; }
		#endregion
	}
}
