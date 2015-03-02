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

		public Task<Contact> GetContactByIdAsync(string customerId)
		{
			return GetAsync<Contact>(
				this.CreateRequestUri(string.Format(RelativePaths.GetContactById, customerId)),
				useCache: false);
		}

        public Task<Contact> UpdateContactAsync(Contact contact)
        {
            return SendAsync<Contact>(
                this.CreateRequestUri(RelativePaths.SendContant),
                HttpMethod.Put);
        }

		protected class RelativePaths
		{
			public const string GetContactById = "contacts/{0}";
            public const string SendContant = "contacts";
		}
	}
}
