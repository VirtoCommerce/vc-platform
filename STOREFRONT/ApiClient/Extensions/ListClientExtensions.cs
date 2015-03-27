#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ListClientExtensions
    {
        #region Public Methods and Operators

        public static ListClient CreateListClient(this CommerceClients source)
        {
            return CreateListClient(source, ClientContext.Configuration.ConnectionString);
        }

        public static ListClient CreateListClient(this CommerceClients source, string serviceUrl)
        {
            var client = new ListClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
