namespace VirtoCommerce.Foundation.Data.Azure.CQRS
{
    using System;

    using VirtoCommerce.Foundation.Frameworks.CQRS;
    using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;

    using System.Threading;

    using VirtoCommerce.Foundation.Frameworks.CQRS.Events;
    using VirtoCommerce.Foundation.Frameworks.CQRS.Serialization;

    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage;

    using System.IO;

    public class AzureQueueReader : AzureQueueBase, IQueueReader
	{
		private readonly Func<uint, TimeSpan> _waiter;

		private const int RetryCount = 4;
		private uint _retryCount;

		public AzureQueueReader(ISystemObserver observer,
								IMessageSerializer serializer)
			: base(observer, serializer)
		{
			this._waiter = (x) => { return TimeSpan.FromSeconds(1); };
		}

		#region IQueueReader Members

		public void DeleteMessage(MessageEnvelope envelope)
		{
			if (envelope == null)
			{
				throw new ArgumentNullException("envelope");
			}
			var queue = this.GetQueue(envelope.QueueName);
			queue.DeleteMessage((CloudQueueMessage)envelope.TransportMessage);
			if (envelope.MessageReference.StorageReference != null)
			{
				var blob = this.CloudBlob.GetBlobReferenceFromServer(envelope.MessageReference.StorageReference);
				blob.DeleteIfExists();
			}
			this.Observer.Notify(new MessageAcked(queue.Name, envelope.EnvelopeId));
		}

		public bool TakeMessage(string queueName, CancellationToken token, out MessageEnvelope envelope)
		{
			envelope = null;

			while (!token.IsCancellationRequested)
			{
				var queue = this.GetQueue(queueName);
				var result = this.TryGetMessage(queue, out envelope);
				switch (result)
				{
					case GetMessageResult.Success:
						this._retryCount = 0;
						return true;
					case GetMessageResult.Empty:
					case GetMessageResult.Retry:
						this._retryCount += 1;
						break;
					case GetMessageResult.Exception:
						return false;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (this._retryCount > RetryCount)
					break;

				var waiting = this._waiter(this._retryCount);
				token.WaitHandle.WaitOne(waiting);
			}

			return false;
		}

		#endregion

		[CLSCompliant(false)]
		public GetMessageResult TryGetMessage(CloudQueue queue, out MessageEnvelope envelope)
		{
			CloudQueueMessage message = null;
			envelope = null;
			try
			{
				message = queue.GetMessage(new TimeSpan(0, 2, 0)); // 2 min timeout
			}
			catch (Exception ex)
			{
				this.Observer.Notify(new FailedToReadMessage(ex, queue.Name));
				return GetMessageResult.Exception;
			}

			if (message == null)
			{
				return GetMessageResult.Empty;
			}

			if (message.DequeueCount > RetryCount)
			{
				this.Observer.Notify(new RetrievedPoisonMessage(queue.Name, message.Id));
				return GetMessageResult.Retry;
			}

			try
			{
				envelope = SerializationUtility.Deserialize<MessageEnvelope>(this.Serializer, message.AsBytes);
				//try download from blob
				if (!envelope.MessageReference.IsLoaded)
				{
					if (envelope.MessageReference.StorageContainer != this.CloudBlob.Uri.ToString())
					{
						throw new InvalidOperationException("Wrong container used!");
					}
					var blob = this.CloudBlob.GetBlobReferenceFromServer(envelope.MessageReference.StorageReference);
					var messageType = Type.GetType(envelope.MessageReference.MessageTypeName);
					var stream = new MemoryStream();
					blob.DownloadToStream(stream);
					envelope.MessageReference.Message = SerializationUtility.Deserialize(messageType, this.Serializer, stream.ToArray()) as IMessage;
				}
				envelope.TransportMessage = message;
			}
			catch (StorageException ex)
			{
				this.Observer.Notify(new FailedToAccessStorage(ex, queue.Name, message.Id));
				return GetMessageResult.Retry;
			}
			catch (Exception ex)
			{
				this.Observer.Notify(new FailedToDeserializeMessage(ex, queue.Name, message.Id));
				return GetMessageResult.Exception;
			}

			return GetMessageResult.Success;
		}
	}
}
