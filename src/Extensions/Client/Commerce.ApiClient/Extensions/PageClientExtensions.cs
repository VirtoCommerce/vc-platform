#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class PageClientExtensions
    {
        #region Public Methods and Operators

        public static PageClient CreatePageClient(this CommerceClients source)
        {
            return CreatePageClient(source, ClientContext.Configuration.ConnectionString);
        }

        public static PageClient CreatePageClient(this CommerceClients source, string serviceUrl)
        {
            var client = new PageClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
