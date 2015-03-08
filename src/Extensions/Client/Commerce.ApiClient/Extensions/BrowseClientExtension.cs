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
            /*
            if (String.IsNullOrEmpty(language))
            {
                language = Thread.CurrentThread.CurrentUICulture.ToString();
            }

            
            // http://localhost/admin/api/mp/{0}/{1}/
            var connectionString = String.Format(
                "{0}{1}/stores/{2}/{3}/",
                ClientContext.Configuration.ConnectionString,
                "mp",
                storeId,
                language);
            */
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
