using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class ShoppingCartEntity : AuditableEntity
	{
		public ShoppingCartEntity()
		{
			Items = new NullCollection<LineItemEntity>();
			Payments = new NullCollection<PaymentEntity>();
			Addresses = new NullCollection<AddressEntity>();
			Shipments = new NullCollection<ShipmentEntity>();
		}
	
		[StringLength(64)]
		public string Name { get; set; }
		[Required]
		[StringLength(64)]
		public string StoreId { get; set; }
		[StringLength(64)]
		public string ChannelId { get; set; }

		public bool IsAnonymous { get; set; }
		[Required]
		[StringLength(64)]
		public string CustomerId { get; set; }
		[StringLength(128)]
		public string CustomerName { get; set; }
		[StringLength(64)]
		public string OrganizationId { get; set; }
		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		[StringLength(64)]
		public string Coupon { get; set; }
		[StringLength(16)]
		public string LanguageCode { get; set; }
		public bool TaxIncluded { get; set; }
		public bool IsRecuring { get; set; }
		[StringLength(2048)]
		public string Comment { get; set; }
		[Column(TypeName = "Money")]
		public decimal Total { get; set; }
		[Column(TypeName = "Money")]
		public decimal SubTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal ShippingTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal HandlingTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal TaxTotal { get; set; }

		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<PaymentEntity> Payments { get; set; }
		public virtual ObservableCollection<ShipmentEntity> Shipments { get; set; }
	}
}
