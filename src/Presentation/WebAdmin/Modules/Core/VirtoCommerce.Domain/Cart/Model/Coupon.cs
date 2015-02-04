using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Coupon : ValueObject<Coupon>
	{
		public string CouponCode { get; set; }
		public string InvalidDescription { get; set; }
	}
}
