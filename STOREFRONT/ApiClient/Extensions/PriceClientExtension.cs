#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class PriceClientExtension
    {
        #region Public Methods and Operators

        public static PriceClient CreatePriceClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreatePriceClientWithUri(source, connectionString);
        }

        public static PriceClient CreatePriceClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var client = new PriceClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        public static PriceClient CreatePriceClient(this CommerceClients source, string serviceUrl, string appId, string secretKey)
        {
            var client = new PriceClient(new Uri(serviceUrl), appId, secretKey);
            return client;
        }
        #endregion
    }
}
