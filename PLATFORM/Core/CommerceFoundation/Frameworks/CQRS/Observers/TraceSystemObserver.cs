using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Observers
{
	public sealed class TraceSystemObserver : ISystemObserver
	{
		public void Notify(ISystemEvent systemEvent)
		{
			Trace.WriteLine(systemEvent);
			Trace.Flush();
		}
	}
}
