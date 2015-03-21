namespace VirtoCommerce.Foundation.Data.Azure.CQRS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.Foundation.Frameworks.CQRS;

    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    public abstract class AzureQueueBase
	{
		protected const string BlobName = "messages-blob";
		private List<CloudQueue> _queues = new List<CloudQueue>();
		[CLSCompliant(false)]
		protected CloudBlobContainer CloudBlob { get; private set; }
		[CLSCompliant(false)]
		protected CloudStorageAccount CloudAccount { get; private set; }
		protected IMessageSerializer Serializer { get; private set; }
		protected ISystemObserver Observer { get; private set; }

		public AzureQueueBase(ISystemObserver observer,
							  IMessageSerializer serializer)
		{
			this.CloudAccount = AzureConfiguration.Instance.AzureStorageAccount;
			this.Serializer = serializer;
			this.Observer = observer;

			var blobClient = this.CloudAccount.CreateCloudBlobClient();
			blobClient.RetryPolicy = new NoRetry();

			this.CloudBlob = blobClient.GetContainerReference(BlobName);
			this.CloudBlob.CreateIfNotExists();
		}

		[CLSCompliant(false)]
		protected CloudQueue GetQueue(string queueName)
		{
			var retval = this._queues.FirstOrDefault(x => x.Name == queueName);
			if (retval == null)
			{
				var queueClient = this.CloudAccount.CreateCloudQueueClient();
				queueClient.RetryPolicy = new NoRetry();
				retval = queueClient.GetQueueReference(queueName);
				retval.CreateIfNotExists();
				this._queues.Add(retval);
			}

			return retval;
		}

	}
}
