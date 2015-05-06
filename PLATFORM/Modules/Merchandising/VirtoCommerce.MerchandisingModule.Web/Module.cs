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
			_container.RegisterType<IBrowseFilterService, FilterService>();
			_container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
		}



		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
		}

		public void PostInitialize()
		{
			var settingsManager = _container.Resolve<ISettingsManager>();
			var cacheManager = _container.Resolve<CacheManager>();
			var cacheSettings = new[] 
			{
				new CacheSettings("MarketingController.EvaluatePromotions", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.MarketingTimeout", 1))),
				new CacheSettings("MarketingController.EvaluateDynamicContent", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.MarketingTimeout", 1))),
				new CacheSettings("ProductController.Search", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.SearchTimeout", 30))),
				new CacheSettings("PriceController.GetProductPrices", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.PricingTimeout", 30))),
				new CacheSettings("PriceController.GetPriceListStack", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.PricingTimeout", 30)))

			};
			cacheManager.AddCacheSettings(cacheSettings);

		}

		#endregion
	}
}
