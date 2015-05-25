using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class PaymentMethod
	{
		public string Code { get; set; }
		public string Description { get; set; }
		public string LogoUrl { get; set; }
		public bool IsActive { get; set; }
		public int Priority { get; set; }

		public ICollection<Setting> Settings { get; set; }
	}
}