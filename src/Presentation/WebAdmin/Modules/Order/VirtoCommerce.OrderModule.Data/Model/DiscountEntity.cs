using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class DiscountEntity : Entity
	{
		public string PromotionId { get; set; }
		public string PromotionDescription { get; set; }

		public string Currency { get; set; }
		public decimal DiscountAmount { get; set; }
		
		public string CouponCode { get; set; }
		public string CouponInvalidDescription { get; set; }

		public virtual CustomerOrderEntity CustomerOrder { get; set; }
		public virtual LineItemEntity LineItem { get; set; }
		public virtual ShipmentEntity Shipment { get; set; }
		

	}
}
