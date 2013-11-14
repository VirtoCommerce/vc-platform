using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Factories
{
	internal sealed class MessageInfo
	{
		public Type MessageType { get; internal set; }
		public Type[] DirectConsumers { get; internal set; }
		public Type[] DerivedConsumers { get; internal set; }
		public Type[] AllConsumers { get; internal set; }

		public bool IsDomainMessage { get; internal set; }
	}
}
