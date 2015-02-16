using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class BrowseClientExtension
	{
		public static BrowseClient CreateBrowseClient(this CommerceClients source)
		{
			return source.CreateBrowseClient(ClientContext.Session.StoreId, ClientContext.Session.Language);
		}

		public static BrowseClient CreateBrowseClient(this CommerceClients source, string storeId, string language)
		{
			// http://localhost/admin/api/mp/{0}/{1}/
			var connectionString = String.Format("{0}{1}/{2}/{3}/", ClientContext.Configuration.ConnectionString, "mp", storeId, language);
			return CreateBrowseClientWithUri(source, connectionString);
		}

		public static BrowseClient CreateBrowseClientWithUri(this CommerceClients source, string serviceUrl)
		{
			var connectionString = serviceUrl;
			var client = new BrowseClient(new Uri(connectionString), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
