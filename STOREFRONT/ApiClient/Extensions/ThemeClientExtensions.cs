#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ThemeClientExtensions
    {
        #region Public Methods and Operators

        public static ThemeClient CreateThemeClient(this CommerceClients source)
        {
            return CreateThemeClient(source, ClientContext.Configuration.ConnectionString);
        }

        public static ThemeClient CreateThemeClient(this CommerceClients source, string serviceUrl)
        {
            var client = new ThemeClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }

        public static ThemeClient CreateThemeClient(this CommerceClients source, string serviceUrl, string appId, string secretKey)
        {
            var client = new ThemeClient(new Uri(serviceUrl), appId, secretKey);
            return client;
        }
        #endregion
    }
}
