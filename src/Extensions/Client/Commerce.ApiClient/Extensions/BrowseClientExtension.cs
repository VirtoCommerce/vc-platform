#region
using System;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class BrowseClientExtension
    {
        #region Public Methods and Operators
        public static BrowseClient CreateBrowseClient(this CommerceClients source, string storeId, string language = "")
        {
            if (String.IsNullOrEmpty(language))
            {
                language = Thread.CurrentThread.CurrentUICulture.ToString();
            }

            // http://localhost/admin/api/mp/{0}/{1}/
            var connectionString = String.Format(
                "{0}{1}/{2}/{3}/",
                ClientContext.Configuration.ConnectionString,
                "mp",
                storeId,
                language);
            return CreateBrowseClientWithUri(source, connectionString);
        }

        public static BrowseClient CreateBrowseClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var client = new BrowseClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
            return client;
        }
        #endregion
    }
}