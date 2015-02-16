using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class ItemsClientExtension
	{
		public static ItemsClient CreateItemsClient(this CommerceClients source)
		{
			var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
			return CreateItemsClient(source, connectionString);
		}

		public static ItemsClient CreateItemsClient(this CommerceClients source, string serviceUrl)
		{
			var connectionString = serviceUrl;
			var client = new ItemsClient(new Uri(connectionString), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
