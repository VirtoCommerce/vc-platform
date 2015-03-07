using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class CustomerServiceClientExtension
    {
        #region Public Methods and Operators

        public static CustomerServiceClient CreateCustomerServiceClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateCustomerServiceClient(source, connectionString);
        }

        public static CustomerServiceClient CreateCustomerServiceClient(this CommerceClients source, string serviceUrl)
        {
            var client = new CustomerServiceClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
