using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MerchandisingModule.Data.Services;
using VirtoCommerce.MerchandisingModule.Services;

namespace VirtoCommerce.MerchandisingModule.Web
{
    using VirtoCommerce.Caching.HttpCache;
    using VirtoCommerce.Foundation.Assets.Factories;
    using VirtoCommerce.Foundation.Assets.Repositories;
    using VirtoCommerce.Foundation.Assets.Services;
    using VirtoCommerce.Foundation.Data.Azure.Asset;
    using VirtoCommerce.Foundation.Data.Marketing;
    using VirtoCommerce.Foundation.Frameworks;
    using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
    using VirtoCommerce.Foundation.Marketing.Repositories;
    using VirtoCommerce.Foundation.Marketing.Services;

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
            
            _container.RegisterType<IDynamicContentService, DynamicContentService>();
            _container.RegisterType<IDynamicContentService, DynamicContentService>();
            _container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>();
            _container.RegisterType<IDynamicContentEvaluator, DynamicContentEvaluator>();
            

            _container.RegisterType<IAssetRepository, AzureBlobAssetRepository>();
            _container.RegisterType<IBlobStorageProvider, AzureBlobAssetRepository>();
            _container.RegisterType<IAssetUrl, AzureBlobAssetRepository>();
            _container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();
            _container.RegisterType<IAssetService, AssetService>();

            _container.RegisterType<ICacheRepository, HttpCacheRepository>();
            
        }
    }
}
