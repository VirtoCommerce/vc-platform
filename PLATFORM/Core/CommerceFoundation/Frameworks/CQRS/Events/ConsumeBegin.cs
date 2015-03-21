using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Events
{
	public sealed class ConsumeBegin : ISystemEvent
	{
		public IMessage Message { get; set; }
		public IConsume Consumer { get; set; }
		public string QueueName { get; set; }

		public ConsumeBegin(IMessage message, IConsume consumer, string queueName)
		{
			Message = message;
			Consumer = consumer;
			QueueName = queueName;
		}
	}
}
