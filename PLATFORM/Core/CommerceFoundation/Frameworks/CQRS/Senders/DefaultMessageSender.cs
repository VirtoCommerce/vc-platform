using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Senders
{
	public class DefaultMessageSender : IMessageSender
	{
		private readonly IQueueWriter _queueWriter;

		public DefaultMessageSender(IQueueWriter queueWriter)
		{
			_queueWriter = queueWriter;
		}

		#region IMessageSender Members

		public void Send(string queueName, IMessage message)
		{
			DelaySendBatch(queueName, default(TimeSpan), message);
		}

		public void DelaySend(string queueName, TimeSpan timeout, IMessage message)
		{
			DelaySendBatch(queueName, timeout, message);
		}

		public void SendBatch(string queueName, params IMessage[] messageItems)
		{
			DelaySendBatch(queueName, default(TimeSpan), messageItems);
		}

		public void DelaySendBatch(string queueName, TimeSpan timeout, params IMessage[] messages)
		{
			var id = Guid.NewGuid().ToString().ToLowerInvariant();
			DateTimeOffset offset = DateTimeOffset.UtcNow;
			if (timeout != default(TimeSpan))
			{
				offset = DateTimeOffset.UtcNow + timeout;
			}
			foreach (var msg in messages)
			{
				var envelope = CreateMessageEnvelope(msg, queueName,  offset);
				_queueWriter.PutMessage(envelope);
			}
		}

		#endregion

		private static MessageEnvelope CreateMessageEnvelope(IMessage message, string queueName, DateTimeOffset offset)
		{
			var id = Guid.NewGuid().ToString().ToLowerInvariant();
			var messageReference = new MessageReference(message);
			return new MessageEnvelope(id, queueName, messageReference, offset);
		}

	}
}
