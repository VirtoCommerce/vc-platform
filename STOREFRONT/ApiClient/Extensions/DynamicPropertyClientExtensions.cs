using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class DynamicPropertyClientExtensions
    {
        #region Public Methods and Operators

        public static DynamicPropertyServiceClient CreateDynamicPropertyClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateDynamicPropertyClient(source, connectionString);
        }

        public static DynamicPropertyServiceClient CreateDynamicPropertyClient(this CommerceClients source, string serviceUrl)
        {
            var client = new DynamicPropertyServiceClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        public static DynamicPropertyServiceClient CreateDynamicPropertyClient(this CommerceClients source, string serviceUrl, string appId, string secretKey)
        {
            var client = new DynamicPropertyServiceClient(new Uri(serviceUrl), appId, secretKey);
            return client;
        }


        #endregion
    }
}
