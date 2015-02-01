using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class ShoppingCart : Entity, IAuditable
	{
	
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Name { get; set; }
		public string SiteId { get; set; }
		public bool? IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationId { get; set; }
		public CurrencyCodes? Currency { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<Payment> Payments { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public string LanguageCode { get; set; }
		public bool? TaxIncluded { get; set; }
		public bool? IsRecuring { get; set; }
		public string Note { get; set; }

		public Weight Weight { get; set; }
		public decimal? VolumetricWeight { get; set; }
		public Dimension Dimension { get; set; }

		public decimal Total { get; private set; }
		public decimal SubTotal { get; private set; }
		public decimal ShippingTotal { get; private set; }
		public decimal HandlingTotal { get; set; }
		public decimal DiscountTotal { get; private set; }
		public decimal TaxTotal { get; set; }

		public virtual void CalculateTotals()
		{
			if (Items != null)
			{
				foreach (var item in Items)
				{
					item.CalculateTotals();
					DiscountTotal += item.DiscountTotal;
					SubTotal += item.PlacedPrice * item.Quantity;
				}
			}

			if (Shipments != null)
			{
				foreach (var shipment in Shipments)
				{
					shipment.CalculateTotals();
					ShippingTotal += shipment.Total;
					DiscountTotal += shipment.DiscountTotal;
					TaxTotal += shipment.TaxTotal;
				}
			}

			if (Discounts != null)
			{
				foreach (var discount in Discounts)
				{
					DiscountTotal += discount.DiscountAmount;
				}
			}

			Total = SubTotal + ShippingTotal + TaxTotal - DiscountTotal;

		}
	}
}
