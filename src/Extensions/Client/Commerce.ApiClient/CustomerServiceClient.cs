using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
	public class CustomerServiceClient : BaseClient
	{
		public CustomerServiceClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
		{
		}

		public CustomerServiceClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
			: base(adminBaseEndpoint, handler)
		{
		}

		public Task<Contract> GetContactById(string customerId)
		{
			return GetAsync<Contract>(
				this.CreateRequestUri(string.Format(RelativePaths.GetContactById, customerId)),
				useCache: false);
		}

		protected class RelativePaths
		{
			public const string GetContactById = "contacts/{0}";
		}
	}
}
