using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum CaseStatus
	{
		/// <summary>
		/// open
		/// </summary>
		[Description("Open")]
		Open,

		/// <summary>
		/// pending
		/// </summary>
		[Description("Pending")]
		Pending,

		/// <summary>
		/// Resolved
		/// </summary>
		[Description("Resolved")]
		Resolved,

		None
	}
}
