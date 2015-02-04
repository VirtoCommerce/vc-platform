using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class ShoppingCartEntity : Entity, IAuditable
	{
		public ShoppingCartEntity()
		{
			Items = new NullCollection<LineItemEntity>();
			Payments = new NullCollection<PaymentEntity>();
			Addresses = new NullCollection<AddressEntity>();
			Shipments = new NullCollection<ShipmentEntity>();
		}
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Name { get; set; }
		public string SiteId { get; set; }
		public bool IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationId { get; set; }
		public string Currency { get; set; }
		public ICollection<AddressEntity> Addresses { get; set; }
		public ICollection<LineItemEntity> Items { get; set; }
		public ICollection<PaymentEntity> Payments { get; set; }
		public ICollection<ShipmentEntity> Shipments { get; set; }
		public string Coupon { get; set; }
		public string LanguageCode { get; set; }
		public bool TaxIncluded { get; set; }
		public bool IsRecuring { get; set; }
		public string Note { get; set; }

		public decimal Total { get; set; }
		public decimal SubTotal { get; set; }
		public decimal ShippingTotal { get; set; }
		public decimal HandlingTotal { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }
	}
}
