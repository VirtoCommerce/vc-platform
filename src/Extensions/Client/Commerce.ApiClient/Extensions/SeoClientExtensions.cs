#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class SeoClientExtensions
    {
        #region Public Methods and Operators

        public static SeoClient CreateDefaultSeoClient(this CommerceClients source)
        {
            var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
            return CreateSeoClient(source, connectionString);
        }

        public static SeoClient CreateSeoClient(this CommerceClients source, string serviceUrl)
        {
            var client = new SeoClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
