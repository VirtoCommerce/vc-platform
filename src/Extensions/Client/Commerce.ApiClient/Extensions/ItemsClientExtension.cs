using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class ItemsClientExtension
	{
		#region Public Methods and Operators

		public static ItemsClient CreateItemsClient(this CommerceClients source)
		{
			var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
			return CreateItemsClient(source, connectionString);
		}

		public static ItemsClient CreateItemsClient(this CommerceClients source, string serviceUrl)
		{
			var client = new ItemsClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}

		#endregion
	}
}
