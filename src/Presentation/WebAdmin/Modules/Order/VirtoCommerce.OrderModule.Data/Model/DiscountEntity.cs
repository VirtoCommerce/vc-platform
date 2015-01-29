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

		public virtual ICollection<CustomerOrderEntity> CustomerOrders { get; set; }
		public virtual ICollection<LineItemEntity> LineItems { get; set; }
		public virtual ICollection<ShipmentEntity> Shipments { get; set; }
		

	}
}
