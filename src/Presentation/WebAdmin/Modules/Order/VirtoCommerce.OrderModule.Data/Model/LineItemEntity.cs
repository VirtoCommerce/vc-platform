using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class LineItemEntity : Entity
	{
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
		public decimal BasePrice { get; set; }
		public decimal Price { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal Tax { get; set; }
		public long Quantity { get; set; }
		public string ProductId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public bool IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfilmentLocationCode { get; set; }

		public string DiscountId { get; set; }
		public virtual DiscountEntity Discount { get; set; }

		public virtual CustomerOrderEntity CustomerOrder { get; set; }
		public string CustomerOrderId { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

	}
}
