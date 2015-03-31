using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class InventoryClientExtension
    {
        public static InventoryClient CreateInventoryClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;

            return CreateInventoryClientWithUri(source, connectionString);
        }

        public static InventoryClient CreateInventoryClientWithUri(this CommerceClients source, string serviceUrl)
        {
            return new InventoryClient(new Uri(serviceUrl), source.CreateMessageProcessingHandler());
        }
    }
}