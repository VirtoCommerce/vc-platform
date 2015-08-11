using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class ShippingMethod : Entity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Logo url of shipping method, can be used in UI
		/// </summary>
		public string LogoUrl { get; set; }

		/// <summary>
		/// If true - method can be available on storefront
		/// </summary>
		public bool IsActive { get; set; }

		public int Priority { get; set; }

		/// <summary>
		/// Type of taxes
		/// </summary>
		public string TaxType { get; set; }

		public ICollection<Setting> Settings { get; set; }
	}
}