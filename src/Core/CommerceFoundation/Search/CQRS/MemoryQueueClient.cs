using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using System.Threading;

namespace VirtoCommerce.Foundation.Search.CQRS
{
    public class MemoryQueueClient
    {
        private static readonly MemoryQueue<MessageEnvelope> _queue = new MemoryQueue<MessageEnvelope>();

        public static MemoryQueue<MessageEnvelope> Queue
        {
            get
            {
				return _queue;
            }
        }
    }
}
