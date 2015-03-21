using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum AddressType
	{
		/// <summary>
		/// primary address
		/// </summary>
		[Description("Primary")]
		Primary,

		/// <summary>
		/// billing address
		/// </summary>
		[Description("Billing")]
		Billing,

		/// <summary>
		/// Shipping address
		/// </summary>
		[Description("Shipping")]
		Shipping
	}
}
