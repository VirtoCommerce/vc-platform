using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class StoreClientExtensions
	{
		public static StoreClient CreateStoreClient(this CommerceClients source)
		{
			var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
			return CreateStoreClient(source, connectionString);
		}

		public static StoreClient CreateStoreClient(this CommerceClients source, string serviceUrl)
		{
			var client = new StoreClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
