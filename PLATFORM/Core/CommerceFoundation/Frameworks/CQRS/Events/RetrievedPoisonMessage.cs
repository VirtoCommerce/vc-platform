using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class RetrievedPoisonMessage : ISystemEvent
	{
		public string QueueName { get; private set; }
		public string MessageId { get; private set; }

		public RetrievedPoisonMessage(string queueName, string messageId)
		{
			QueueName = queueName;
			MessageId = messageId;
		}
	}
}
