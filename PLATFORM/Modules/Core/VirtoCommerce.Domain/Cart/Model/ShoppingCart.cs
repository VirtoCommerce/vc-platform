using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class ShoppingCart : AuditableEntity
	{
		public string Name { get; set; }
		public string StoreId { get; set; }
		public string ChannelId { get; set; }
		public bool IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationId { get; set; }
		public CurrencyCodes Currency { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<Payment> Payments { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public Coupon Coupon { get; set; }
		public string LanguageCode { get; set; }
		public bool? TaxIncluded { get; set; }
		public bool? IsRecuring { get; set; }
		public string Comment { get; set; }

		public Weight Weight { get; set; }
		public decimal? VolumetricWeight { get; set; }
		public Dimension Dimension { get; set; }

		public decimal Total { get; set; }
		public decimal SubTotal { get; set; }
		public decimal ShippingTotal { get; set; }
		public decimal HandlingTotal { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
	
	}
}
