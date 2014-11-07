using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MerchandisingModule.Data.Services;
using VirtoCommerce.MerchandisingModule.Services;

namespace VirtoCommerce.MerchandisingModule.Web
{
    using VirtoCommerce.Foundation.Assets.Factories;
    using VirtoCommerce.Foundation.Assets.Repositories;
    using VirtoCommerce.Foundation.Assets.Services;
    using VirtoCommerce.Foundation.Data.Azure.Asset;

    [Module(ModuleName = "MerchModule", OnDemand = true)]
    public class MerchModule : IModule
    {
        private readonly IUnityContainer _container;
        public MerchModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
            _container.RegisterType<IMerchantItemService, MerchantItemService>();

            _container.RegisterType<IAssetRepository, AzureBlobAssetRepository>();
            _container.RegisterType<IBlobStorageProvider, AzureBlobAssetRepository>();
            _container.RegisterType<IAssetUrl, AzureBlobAssetRepository>();
            _container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();
            _container.RegisterType<IAssetService, AssetService>();
        }
    }
}
