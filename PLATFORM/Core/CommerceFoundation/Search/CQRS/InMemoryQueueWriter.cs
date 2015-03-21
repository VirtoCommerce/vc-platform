using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;

namespace VirtoCommerce.Foundation.Search.CQRS
{
    public class InMemoryQueueWriter : IQueueWriter
    {
        public InMemoryQueueWriter()
        {
        }

        public void PutMessage(MessageEnvelope envelope)
        {
            MemoryQueueClient.Queue.Enqueue(envelope);
        }
    }
}
