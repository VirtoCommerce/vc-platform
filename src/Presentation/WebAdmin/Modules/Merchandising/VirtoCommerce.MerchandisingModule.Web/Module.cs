using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
			_container.RegisterType<IDynamicContentService, DynamicContentService>();
			_container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>();
			_container.RegisterType<IDynamicContentEvaluator, DynamicContentEvaluator>();
			_container.RegisterType<ICacheRepository, HttpCacheRepository>();
		}
    }
}
