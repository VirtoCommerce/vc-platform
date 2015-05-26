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
	public class Shipment : AuditableEntity
	{
		public string ShipmentMethodCode { get; set; }
		public string FulfilmentCenterId { get; set; }
		public Address DeliveryAddress { get; set; }
		public ICollection<Discount> Discounts { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }

		public ICollection<LineItem> Items { get; set; }

		public Weight Weight { get; set; }
		public decimal? VolumetricWeight { get; set; }
		public Dimension Dimension { get; set; }

		public bool TaxIncluded { get; set; }

		public decimal ShippingPrice { get; set; }
		public decimal Total { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
		public decimal ItemSubtotal { get; set; }
		public decimal Subtotal { get; set; }

	}
}
