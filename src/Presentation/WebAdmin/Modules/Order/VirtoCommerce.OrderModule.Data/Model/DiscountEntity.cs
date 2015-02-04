using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class DiscountEntity : Entity
	{
		[StringLength(64)]
		public string PromotionId { get; set; }
		[StringLength(1024)]
		public string PromotionDescription { get; set; }
		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountAmount { get; set; }
		[StringLength(64)]
		public string CouponCode { get; set; }
		[StringLength(1024)]
		public string CouponInvalidDescription { get; set; }

		public CustomerOrderEntity CustomerOrder { get; set; }
		public string CustomerOrderId { get; set; }

		public ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

		public LineItemEntity LineItem { get; set; }
		public string LineItemId { get; set; }
		

	}
}
