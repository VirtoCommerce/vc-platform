#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class StoreClientExtensions
    {
        #region Public Methods and Operators

        public static StoreClient CreateStoreClient(this CommerceClients source)
        {
            return CreateStoreClient(source, ClientContext.Configuration.ConnectionString);
        }

        public static StoreClient CreateStoreClient(this CommerceClients source, string serviceUrl)
        {
            var client = new StoreClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
