using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.MerchandisingModule.Web.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void Initialize()
        {
            _container.RegisterType<IBrowseFilterService, FilterService>();
            _container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
        }

        public override void PostInitialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            var cacheManager = _container.Resolve<CacheManager>();
            var cacheSettings = new[] 
			{
				new CacheSettings("MP", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.Timeout", 5)))
			};
            cacheManager.AddCacheSettings(cacheSettings);

        }

        #endregion
    }
}
