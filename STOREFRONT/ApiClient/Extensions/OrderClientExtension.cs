using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class OrderClientExtension
    {
        #region Public Methods and Operators

        public static OrderClient CreateOrderClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateOrderClient(source, connectionString);
        }

        public static OrderClient CreateOrderClient(this CommerceClients source, string serviceUrl)
        {
            var client = new OrderClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
