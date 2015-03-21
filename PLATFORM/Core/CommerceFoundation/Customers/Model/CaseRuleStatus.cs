using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum CaseRuleStatus
	{
		/// <summary>
		/// Not active
		/// </summary>
		[Description("Not active")]
		NotActive,

		/// <summary>
		/// Active
		/// </summary>
		[Description("Active")]
		Active
	}
}
