#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class CartClientExtension
    {
        #region Public Methods and Operators

        public static CartClient CreateCartClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateCartClient(source, connectionString);
        }

        public static CartClient CreateCartClient(this CommerceClients source, string serviceUrl)
        {
            var client = new CartClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
