using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Observers
{
	public sealed class NullSystemObserver : ISystemObserver
	{
		public void Notify(ISystemEvent systemEvent)
		{

		}
	}
}
