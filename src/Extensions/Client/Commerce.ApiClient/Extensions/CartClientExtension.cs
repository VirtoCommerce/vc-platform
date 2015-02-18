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
			var client = new CartClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
