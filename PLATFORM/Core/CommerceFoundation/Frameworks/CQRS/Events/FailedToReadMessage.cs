using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class FailedToReadMessage : ISystemEvent
	{
		public Exception Exception { get; private set; }
		public string QueueName { get; private set; }

		public FailedToReadMessage(Exception exception, string queueName)
		{
			Exception = exception;
			QueueName = queueName;
		}
	}
}
