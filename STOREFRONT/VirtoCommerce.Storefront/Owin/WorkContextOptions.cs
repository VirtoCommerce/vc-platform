using VirtoCommerce.Client.Api;

namespace VirtoCommerce.Storefront.Owin
{
    public class WorkContextOptions
    {
        public IStoreModuleApi StoreApi { get; set; }
        public IVirtoCommercePlatformApi PlatformApi { get; set; }
        public ICustomerManagementModuleApi CustomerApi { get; set; }

        public WorkContextOptions(IStoreModuleApi storeApi, IVirtoCommercePlatformApi platformApi, ICustomerManagementModuleApi customerApi)
        {
            StoreApi = storeApi;
            PlatformApi = platformApi;
            CustomerApi = customerApi;
        }
    }
}
