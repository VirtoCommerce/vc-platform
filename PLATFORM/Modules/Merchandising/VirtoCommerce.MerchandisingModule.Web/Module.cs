using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Controllers;
using VirtoCommerce.MerchandisingModule.Web.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : IModule
    {
       private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

		#region IModule Members
		public void Initialize()
		{
			var settingsManager = _container.Resolve<ISettingsManager>();
			var cacheProvider = _container.Resolve<ICacheProvider>();
			var cacheSettings = new[] 
			{
				new CacheSettings("ProductController.Search", TimeSpan.FromMinutes(settingsManager.GetValue("Catalogs.Caching.SearchTimeout", 30)), "", true)
			};
			var cacheManager = new CacheManager(x => cacheProvider, x => cacheSettings.FirstOrDefault(y => y.Group == x));


			_container.RegisterType<IBrowseFilterService, FilterService>();
			_container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
			_container.RegisterType<ProductController>(new InjectionConstructor(_container.Resolve<ICatalogSearchService>(),
																				_container.Resolve<ICategoryService>(),
																				_container.Resolve<IStoreService>(),
																				_container.Resolve<IItemService>(),
																				_container.Resolve<IBlobUrlResolver>(),
																				_container.Resolve<IBrowseFilterService>(),
																				_container.Resolve<IItemBrowsingService>(),
																				cacheManager));
		}



		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
		}

		public void PostInitialize()
		{
		}

		#endregion
	}
}
