using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using VirtoCommerce.Foundation.Frameworks.CQRS.Events;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Engines
{
	public sealed class SingleThreadConsumingProcess : IEngineProcess
	{
		readonly IConsumerFactory _consumerFactory;
		readonly ISystemObserver _observer;
		readonly IQueueReader _queueReader;
		readonly CancellationTokenSource _disposal = new CancellationTokenSource();

		public SingleThreadConsumingProcess(ISystemObserver observer, IConsumerFactory consumerFactory, IQueueReader queueReader)
		{
			_consumerFactory = consumerFactory;
			_observer = observer;
			_queueReader = queueReader;
		}

		public void Dispose()
		{
			_disposal.Dispose();
		}

		#region IEngineProcess Members

		public void Initialize()
		{
		}

		public Task Start(CancellationToken token, IEnumerable<string> queues)
		{
			return Task.Factory.StartNew(() => ReceiveMessages(token, queues), token);
		}

		#endregion

		void ReceiveMessages(CancellationToken outer, IEnumerable<string> queues)
		{
			using (var source = CancellationTokenSource.CreateLinkedTokenSource(_disposal.Token, outer))
			{
				var token = source.Token;
				MessageEnvelope envelope;
				foreach (var queue in queues)
				{
					while (_queueReader.TakeMessage(queue, token, out envelope))
					{
						try
						{

							var msg = envelope.MessageReference.Message;
							var consumers = _consumerFactory.GetMessageConsumers(msg);
							foreach (var consumer in consumers)
							{
								_observer.Notify(new ConsumeBegin(msg, consumer, envelope.QueueName));
								consumer.Consume(msg);
								_observer.Notify(new ConsumeEnd(msg, consumer, envelope.QueueName));
							}

						}
						catch (Exception ex)
						{
							_observer.Notify(new FailedToConsumeMessage(ex, envelope.EnvelopeId, envelope.QueueName));
						}
						try
						{
							_queueReader.DeleteMessage(envelope);
						}
						catch (Exception ex)
						{
							// not a big deal. Message will be processed again.
							_observer.Notify(new FailedToAckMessage(ex, envelope.EnvelopeId, envelope.QueueName));
						}
					}
				}
			}
		}

	
	}
}
