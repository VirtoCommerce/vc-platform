using System.Collections.Generic;


namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
	public class Shipment
	{
		public string Id { get; set; }
		public string ShipmentMethodCode { get; set; }
		public string  WarehouseLocation { get; set; }
		public Address RecipientAddress { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public string Currency { get; set; }
		public Weight Weight { get; set; }
		public decimal? VolumetricWeight { get; set; }
		public Dimension Dimension { get; set; }
		public bool? TaxIncluded { get; set; }
		public decimal ShippingPrice { get; set; }
		public decimal Total { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
		public decimal ItemSubtotal { get; set; }
		public decimal Subtotal { get; set; }

	}
}
