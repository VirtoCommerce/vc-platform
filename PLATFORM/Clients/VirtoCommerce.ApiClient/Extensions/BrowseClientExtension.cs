using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class BrowseClientExtension
    {
        public static BrowseClient CreateCatalogClient(this CommerceClients source)
        {
            var connectionString = ConnectionHelper.GetConnectionString("commerce-api");
            var client = new BrowseClient(new Uri(connectionString), "secret");
            return client;
        }
    }
}
