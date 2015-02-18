#region

using System;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ContentClientExtension
    {
        #region Public Methods and Operators

        public static ContentClient CreateContentClient(this CommerceClients source, string serviceUrl)
        {
            var client = new ContentClient(
                new Uri(serviceUrl),
                source.CreateAzureSubscriptionMessageProcessingHandler());
            return client;
        }

        public static ContentClient CreateDefaultContentClient(this CommerceClients source)
        {
            var language = Thread.CurrentThread.CurrentUICulture.ToString();

            var connectionString = String.Format(
                "{0}{1}/{2}/",
                ClientContext.Configuration.ConnectionString,
                "mp",
                language);
            return CreateContentClient(source, connectionString);
        }

        #endregion
    }
}
