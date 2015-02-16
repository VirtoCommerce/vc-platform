using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class CartClientExtension
	{
		public static CartClient CreateCartClient(this CommerceClients source)
		{
			var connectionString = ClientContext.Configuration.ConnectionString;
			return CreateCartClient(source, connectionString);
		}

		public static CartClient CreateCartClient(this CommerceClients source, string serviceUrl)
		{
			var connectionString = serviceUrl;
			var client = new CartClient(new Uri(connectionString), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
