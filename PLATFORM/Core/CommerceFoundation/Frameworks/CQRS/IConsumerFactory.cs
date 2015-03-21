using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public interface IConsumerFactory
	{
		IEnumerable<IConsume> GetMessageConsumers(IMessage message);
	}
}
