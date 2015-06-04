using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Coupon : ValueObject<Coupon>
	{
		public string Code { get; set; }
		public bool IsValid
		{
			get
			{
				return String.IsNullOrEmpty(InvalidDescription);
			}
		}
		public string InvalidDescription { get; set; }
	}
}
