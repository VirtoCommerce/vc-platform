using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class FailedToAccessStorage : ISystemEvent
	{
		public Exception Exception { get; private set; }
		public string QueueName { get; private set; }
		public string MessageId { get; private set; }

		public FailedToAccessStorage(Exception exception, string queueName, string messageId)
		{
			Exception = exception;
			QueueName = queueName;
			MessageId = messageId;
		}
	}
}
