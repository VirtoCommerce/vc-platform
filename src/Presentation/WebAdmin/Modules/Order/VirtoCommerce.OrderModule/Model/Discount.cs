using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Model
{
	public class Discount : ValueObject<Discount>
	{
		public string PromotionId { get; set; }
		public Money DiscountAmount { get; set; }
		public Coupon Coupon { get; set; }
		public string Description { get; set; }
	}
}
