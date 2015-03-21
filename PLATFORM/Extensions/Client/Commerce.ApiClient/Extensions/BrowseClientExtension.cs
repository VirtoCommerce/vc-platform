#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class BrowseClientExtension
    {
        #region Public Methods and Operators

        public static BrowseClient CreateBrowseClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateBrowseClientWithUri(source, connectionString);
        }

        public static BrowseClient CreateBrowseClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var client = new BrowseClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
