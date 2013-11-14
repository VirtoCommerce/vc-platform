using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public enum EntryState
	{
		Detached = 1,
		Unchanged = 2,
		Added = 4,
		Deleted = 8,
		Modified = 16,
	}
}
