using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class TrackingEntry
	{
		public object Entity { get; set; }
		public EntryState EntryState { get; set; }
		internal bool IsSubscribed { get; set; }

		public override string ToString()
		{
			return String.Format("{1} {0}", Entity ?? "null", EntryState);
		}
	}
}
