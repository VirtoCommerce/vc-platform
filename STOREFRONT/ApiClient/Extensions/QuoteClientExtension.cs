using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class QuoteClientExtension
    {
        public static QuoteClient CreateQuoteClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateQuoteClientWithUri(source, connectionString);
        }

        public static QuoteClient CreateQuoteClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var client = new QuoteClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
            return client;
        }
        public static QuoteClient CreateQuoteClient(this CommerceClients source, string serviceUrl, string appId, string secretKey)
        {
            return new QuoteClient(new Uri(serviceUrl), appId, secretKey);
        }
    }
}