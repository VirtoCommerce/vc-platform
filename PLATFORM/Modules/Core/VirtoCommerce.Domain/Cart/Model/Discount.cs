using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;


namespace VirtoCommerce.Domain.Cart.Model
{
	public class Discount : Entity
	{
		public string PromotionId { get; set; }
		public string Currency { get; set; }
		public decimal DiscountAmount { get; set; }
        public string Coupon { get; set; }
		public string Description { get; set; }
	}
}
