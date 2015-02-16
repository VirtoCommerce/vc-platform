using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class ContentClientExtension
	{
		public static ContentClient CreateDefaultContentClient(this CommerceClients source, string language = "")
		{
			var session = ClientContext.Session;
			language = String.IsNullOrEmpty(language) ? session.Language : language;

			var connectionString = String.Format("{0}{1}/{2}/", ClientContext.Configuration.ConnectionString, "mp", language);
			return CreateContentClient(source, connectionString);
		}

		public static ContentClient CreateContentClient(this CommerceClients source, string serviceUrl)
		{
			var connectionString = serviceUrl;
			var client = new ContentClient(new Uri(connectionString), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
