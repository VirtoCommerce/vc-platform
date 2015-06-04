using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public interface IMessageSender
	{
		void DelaySend(string queueName, TimeSpan timeout, IMessage message);
		void DelaySendBatch(string queueName, TimeSpan timeout, params IMessage[] messages);
		void Send(string queueName, IMessage message);
		void SendBatch(string queueName, params IMessage[] messageItems);
	}
}
