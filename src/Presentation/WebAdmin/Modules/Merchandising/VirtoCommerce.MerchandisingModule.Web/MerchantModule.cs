using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Search.Providers.Elastic;


namespace VirtoCommerce.MerchandisingModule.Web
{

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
		}
    }
}
