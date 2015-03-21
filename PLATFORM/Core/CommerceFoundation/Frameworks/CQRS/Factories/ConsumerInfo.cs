using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Factories
{
	internal sealed class ConsumerInfo
	{
		public readonly Type ConsumerType;
		public readonly Type[] MessageTypes;

		public ConsumerInfo(Type consumerType, Type[] messageTypes)
		{
			ConsumerType = consumerType;
			MessageTypes = messageTypes;
		}
	}
}
