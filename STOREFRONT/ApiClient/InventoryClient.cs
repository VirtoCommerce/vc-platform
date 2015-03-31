using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class InventoryClient : BaseClient
    {
        public InventoryClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public InventoryClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public Task<ItemInventory> GetItemInventory(string itemId, bool useCache = false)
        {
            return GetAsync<ItemInventory>(CreateRequestUri(string.Format(RelativePaths.GetItemInventory, itemId)), useCache);
        }

        protected class RelativePaths
        {
            public const string GetItemInventory = "catalog/products/{0}/inventory";
        }
    }
}