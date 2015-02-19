using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Extensions
{
	public static class CustomerServiceClientExtension
	{
		public static CustomerServiceClient CreateCustomerServiceClient(this CommerceClients source, string serviceUrl)
		{
			var client = new CustomerServiceClient(
				new Uri(serviceUrl),
				source.CreateMessageProcessingHandler());
			return client;
		}

		public static CustomerServiceClient CreateDefaultCustomerServiceClient(this CommerceClients source)
		{
			var connectionString = ClientContext.Configuration.ConnectionString;
			return CreateCustomerServiceClient(source, connectionString);
		}
	}
}
