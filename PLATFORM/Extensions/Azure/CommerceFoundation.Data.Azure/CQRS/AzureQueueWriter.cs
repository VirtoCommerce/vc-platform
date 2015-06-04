namespace VirtoCommerce.Foundation.Data.Azure.CQRS
{
    using System;

    using VirtoCommerce.Foundation.Frameworks.CQRS;
    using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;

    using System.IO;

    using VirtoCommerce.Foundation.Frameworks.CQRS.Serialization;

    using Microsoft.WindowsAzure.Storage.Queue;

    public class AzureQueueWriter : AzureQueueBase, IQueueWriter
	{
		private const int CloudQueueLimit = 6144;
		private const string DateFormatInBlobName = "yyyy-MM-dd-HH-mm-ss-ffff";

		public AzureQueueWriter(ISystemObserver observer,
								IMessageSerializer serializer)
			: base(observer, serializer)
		{
		}

		#region IQueueWriter Members
		public void PutMessage(MessageEnvelope envelope)
		{
			var message = this.PrepareCloudMessage(envelope);
			var queue = this.GetQueue(envelope.QueueName);
			queue.AddMessage(message);
		}
		#endregion

		private CloudQueueMessage PrepareCloudMessage(MessageEnvelope envelope)
		{
			CloudQueueMessage retVal = null;
			var buffer = SerializationUtility.Serialize(this.Serializer, envelope);
			if (buffer.Length > CloudQueueLimit && envelope.MessageReference.Message != null)
			{
				var referenceId = DateTimeOffset.UtcNow.ToString(DateFormatInBlobName) + "-" + envelope.EnvelopeId;
				var messageBuffer = SerializationUtility.Serialize(this.Serializer, envelope.MessageReference.Message);
				this.CloudBlob.GetBlobReferenceFromServer(referenceId).UploadFromStream(new MemoryStream(messageBuffer));

				var msgTypeName = envelope.MessageReference.Message.GetType().AssemblyQualifiedName;
				envelope.MessageReference = new MessageReference(this.CloudBlob.Uri.ToString(), referenceId, msgTypeName);
				buffer = SerializationUtility.Serialize(this.Serializer, envelope);
			}
			retVal = new CloudQueueMessage(buffer);
			return retVal;
		}
	}
}
