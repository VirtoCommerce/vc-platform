using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public Task<IEnumerable<InventoryInfo>> GetItemsInventories(string[] itemsIds)
        {
            var parameters = new List<string>();

            foreach (var itemId in itemsIds)
            {
                parameters.Add(string.Format("ids={0}", itemId));
            }

            var inventories = GetAsync<IEnumerable<InventoryInfo>>(
                CreateRequestUri(RelativePaths.GetItemsInventories, string.Join("&", parameters)), false);

            return inventories;
        }

        protected class RelativePaths
        {
            public const string GetItemsInventories = "inventory/products";
        }
    }
}