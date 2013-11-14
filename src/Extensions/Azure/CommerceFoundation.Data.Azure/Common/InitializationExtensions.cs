namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    using System;
    using System.Collections.Generic;

    using Microsoft.WindowsAzure.Storage;

    public static class InitializationExtensions
	{
        /// <summary>
        /// Ensures the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="containers">The containers.</param>
        /// <param name="queues">The queues.</param>
		[CLSCompliant(false)]
		public static void Ensure(this CloudStorageAccount account, IEnumerable<string> tables = null, IEnumerable<string> containers = null, IEnumerable<string> queues = null)
		{
			var tableClient = account.CreateCloudTableClient();
			var blobClient = account.CreateCloudBlobClient();
			var queueClient = account.CreateCloudQueueClient();


			if (tables != null)
			{
				foreach (var table in tables)
				{
					var t = tableClient.GetTableReference(table);
					t.CreateIfNotExists();
				}
			}
			if (containers != null) foreach (var container in containers) blobClient.GetContainerReference(container).CreateIfNotExists();
			if (queues != null) foreach (var queue in queues) queueClient.GetQueueReference(queue).CreateIfNotExists();
		}
	}
}
