using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class FailedToConsumeMessage : ISystemEvent
	{
		public Exception Exception { get; private set; }
		public string EnvelopeId { get; private set; }
		public string QueueName { get; private set; }

		public FailedToConsumeMessage(Exception exception, string envelopeId, string queueName)
		{
			Exception = exception;
			EnvelopeId = envelopeId;
			QueueName = queueName;
		}
	}
}
