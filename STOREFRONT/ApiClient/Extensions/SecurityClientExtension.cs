#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class SecurityClientExtension
    {
        #region Public Methods and Operators

        public static SecurityClient CreateSecurityClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString + "security/";
            return CreateSecurityClient(source, connectionString);
        }

        public static SecurityClient CreateSecurityClient(this CommerceClients source, string serviceUrl)
        {
            var client = new SecurityClient(
                new Uri(serviceUrl),
                source.CreateMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
