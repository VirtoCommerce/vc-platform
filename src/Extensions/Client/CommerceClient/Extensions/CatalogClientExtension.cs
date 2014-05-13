using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Client.Extensions
{
    using Microsoft.Practices.ServiceLocation;

    public static class CatalogClientExtension
    {
        public static CatalogClient CreateCatalogClient(this CommerceClients source)
        {
            return ServiceLocator.Current.GetInstance<CatalogClient>();
        }
    }
}
