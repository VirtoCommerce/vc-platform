using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class CustomerServiceClient : BaseClient
    {
        #region Constructors and Destructors

        public CustomerServiceClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public CustomerServiceClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Task<Contact> GetContactByIdAsync(string customerId)
        {
            return this.GetAsync<Contact>(
                this.CreateRequestUri(string.Format(RelativePaths.GetContactById, customerId)),
                useCache: false);
        }

        public Task<Contact> UpdateContactAsync(Contact contact)
        {
            return this.SendAsync<Contact>(
                this.CreateRequestUri(RelativePaths.SendContant),
                HttpMethod.Put);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string GetContactById = "contacts/{0}";
            public const string SendContant = "contacts";

            #endregion
        }
    }
}
