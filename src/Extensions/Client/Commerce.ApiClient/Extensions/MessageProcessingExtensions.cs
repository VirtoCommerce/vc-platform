#region

using System.Configuration;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class MessageProcessingExtensions
    {
        #region Public Methods and Operators

        public static AzureSubscriptionMessageProcessingHandler CreateAzureSubscriptionMessageProcessingHandler(
            this CommerceClients source)
        {
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-AzureApiKey"];
            var appId = ConfigurationManager.AppSettings["vc-public-ApiAppId"];
            var secretKey = ConfigurationManager.AppSettings["vc-public-ApiSecretKey"];

            var handler = new AzureSubscriptionMessageProcessingHandler(subscriptionKey, appId, secretKey);
            return handler;
        }

        #endregion
    }
}
