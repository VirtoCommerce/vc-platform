using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Coupon : ValueObject<Coupon>
	{
		public string CouponCode { get; set; }
		public string InvalidDescription { get; set; }
	}
}
