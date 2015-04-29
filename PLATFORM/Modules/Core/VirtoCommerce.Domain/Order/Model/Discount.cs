using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Discount : ValueObject<Discount>
	{
		public string PromotionId { get; set; }
		public CurrencyCodes? Currency { get; set; }
		public decimal DiscountAmount { get; set; }
		public Coupon Coupon { get; set; }
		public string Description { get; set; }
	}
}
