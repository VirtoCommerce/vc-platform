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
            var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "cms");
            return CreateThemeClient(source, connectionString);
        }

        public static ThemeClient CreateThemeClient(this CommerceClients source, string serviceUrl)
        {
            var client = new ThemeClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
            return client;
        }

        #endregion
    }
}
