using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class MarketingClientExtension
    {
        public static MarketingClient CreateMarketingClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;

            return CreateMarketingClientWithUri(source, connectionString);
        }

        public static MarketingClient CreateMarketingClientWithUri(this CommerceClients source, string serviceUrl)
        {
            return new MarketingClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
        }

    }
}