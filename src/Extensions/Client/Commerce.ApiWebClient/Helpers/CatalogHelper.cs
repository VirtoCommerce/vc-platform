using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ApiWebClient.Clients;

namespace VirtoCommerce.ApiWebClient.Helpers
{
    public class CatalogHelper
    {
        public static CatalogClient CatalogClient
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CatalogClient>();
            }
        }
    }
}
