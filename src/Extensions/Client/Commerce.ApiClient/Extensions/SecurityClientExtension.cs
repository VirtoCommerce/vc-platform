using System;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class SecurityClientExtension
	{
		public static SecurityClient CreateSecurityClient(this CommerceClients source)
		{
			var connectionString = ClientContext.Configuration.ConnectionString + "security/";
			return CreateSecurityClient(source, connectionString);
		}

		public static SecurityClient CreateSecurityClient(this CommerceClients source, string serviceUrl)
		{
			var client = new SecurityClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
			return client;
		}
	}
}
