using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum CaseFilterType
	{
		[Description("All unresolved")]
		AllUnresolvedCases,

		[Description("All unassigned")]
		UnassignedCases,

		[Description("Recently updated")]
		RecentlyUpdatedCases,

		[Description("All cases")]
		AllCases
	}
}
