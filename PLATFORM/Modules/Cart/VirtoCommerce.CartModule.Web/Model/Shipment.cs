using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Shipment : Entity, IAuditable
	{
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

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
