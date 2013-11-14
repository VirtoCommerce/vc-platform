using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using System.Threading;

namespace VirtoCommerce.Foundation.Search.CQRS
{
	public class InMemoryQueueReader : IQueueReader
	{
		public InMemoryQueueReader()
		{
		}

		public void DeleteMessage(MessageEnvelope envelope)
		{
			//MemoryQueueClient.Queue..Enqueue(envelope);
		}

		public bool TakeMessage(string queueName, CancellationToken token, out MessageEnvelope envelope)
		{
			envelope = MemoryQueueClient.Queue.Dequeue(10);
			return envelope != null;
		}
	}
}
