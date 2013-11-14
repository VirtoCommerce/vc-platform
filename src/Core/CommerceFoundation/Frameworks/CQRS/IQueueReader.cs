using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using System.Threading;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public interface IQueueReader
	{
		void DeleteMessage(MessageEnvelope envelope);
		bool TakeMessage(string queueName, CancellationToken token, out MessageEnvelope envelope);
	}
}
