#region

using System;
using System.Linq;
using System.Threading;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;

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
                source.CreateMessageProcessingHandler());
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

        public static string TryGetValue(this ContactProperty[] propeties, string name)
        {
            var retVal = string.Empty;

            if (propeties.Any(p => p.Name.Equals(name)))
            {
                retVal = propeties.First(p => p.Name.Equals(name)).Value;
            }

            return retVal;
        }

        #endregion
    }
}
