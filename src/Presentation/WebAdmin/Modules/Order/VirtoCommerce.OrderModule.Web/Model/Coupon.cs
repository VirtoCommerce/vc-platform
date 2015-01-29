using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class Coupon : ValueObject<Coupon>
	{
		public string Code { get; set; }
		public string InvalidDescription { get; set; }
	}
}