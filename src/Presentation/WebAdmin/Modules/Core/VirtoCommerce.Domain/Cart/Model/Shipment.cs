using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Shipment : Entity
	{
		public string ShipmentMethodCode { get; set; }
		public string  WarehouseLocation { get; set; }
		public Address DeliveryAddress { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public CurrencyCodes? Currency { get; set; }
		public Weight Weight { get; set; }
		public decimal? VolumetricWeight { get; set; }
		public Dimension Dimension { get; set; }

		public bool? TaxIncluded { get; set; }

		public decimal ShippingPrice { get; set; }
		public decimal Total { get; private set; }
		public decimal DiscountTotal { get; private set; }
		public decimal TaxTotal { get; set; }
		public decimal ItemSubtotal { get; private set; }
		public decimal Subtotal { get; private set; }

		public void CalculateTotals()
		{
			if (Discounts != null)
			{
				foreach (var discount in Discounts)
				{
					DiscountTotal += discount.DiscountAmount;
				}
			}

			if (Items != null)
			{
				foreach (var item in Items)
				{
					ItemSubtotal += item.PlacedPrice * item.Quantity;
				}
			}

			Subtotal = ShippingPrice + TaxTotal;
			Total = Subtotal - DiscountTotal;
	
		}
	}
}
