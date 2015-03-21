using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum CasePriority
	{
        /// <summary>
		/// low priority
		/// </summary>
		[Description("low")]
		Low,
		/// <summary>
		/// medium priority
		/// </summary>
		[Description("medium")]
		Medium,
		/// <summary>
		/// high priority
		/// </summary>
		[Description("high")]
		High
	}
}
