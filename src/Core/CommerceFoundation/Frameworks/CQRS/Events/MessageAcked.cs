using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class MessageAcked : ISystemEvent
	{
		public string QueueName { get; private set; }
		public string EnvelopeId { get; private set; }

		public MessageAcked(string queueName, string envelopeId)
		{
			QueueName = queueName;
			EnvelopeId = envelopeId;
		}

		public override string ToString()
		{
			return string.Format("Message {0} acked at {1}", EnvelopeId, QueueName);
		}
	}
}
