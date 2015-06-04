using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Shipment : Entity
	{
		public string ShipmentMethodCode { get; set; }
		public string  WarehouseLocation { get; set; }
		public Address DeliveryAddress { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public CurrencyCodes Currency { get; set; }
	
		public decimal? VolumetricWeight { get; set; }

		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }


		public bool? TaxIncluded { get; set; }

		public decimal ShippingPrice { get; set; }
		public decimal Total { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
		public decimal ItemSubtotal { get; set; }
		public decimal Subtotal { get; set; }
		
	}
}
