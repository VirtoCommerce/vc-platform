using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class LineItemEntity : AuditableEntity, IPosition
	{
		public LineItemEntity()
		{
			Discounts = new NullCollection<DiscountEntity>();
		}

		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		[Column(TypeName = "Money")]
		public decimal BasePrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal Price { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountAmount { get; set; }
		[Column(TypeName = "Money")]
		public decimal Tax { get; set; }
		public int Quantity { get; set; }
		[Required]
		[StringLength(64)]
		public string ProductId { get; set; }
		[Required]
		[StringLength(64)]
		public string CatalogId { get; set; }
		[Required]
		[StringLength(64)]
		public string CategoryId { get; set; }
		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(2048)]
		public string Comment { get; set; }

		public bool IsReccuring { get; set; }

		[StringLength(1028)]
		public string ImageUrl { get; set; }
		public bool IsGift { get; set; }
		[StringLength(64)]
		public string ShippingMethodCode { get; set; }
		[StringLength(64)]
		public string FulfilmentLocationCode { get; set; }

		public virtual ObservableCollection<DiscountEntity> Discounts { get; set; }

		public virtual CustomerOrderEntity CustomerOrder { get; set; }
		public string CustomerOrderId { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

	}
}
