using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum EmailType
	{
		/// <summary>
		/// Primary email
		/// </summary>
		[Description("Primary")]
		Primary,

		/// <summary>
		/// Secondary email
		/// </summary>
		[Description("Secondary")]
		Secondary
	}
}
