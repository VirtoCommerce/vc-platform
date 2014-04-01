using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Client.Globalization.Repository;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Localization
{
	public class LocalizationModule : IModule
	{
		private readonly IUnityContainer _container;

		public LocalizationModule(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IAppConfigRepository, DSAppConfigClient>();

			var repositoryFactory = _container.Resolve<IRepositoryFactory<IAppConfigRepository>>();
			var localElements = new XmlElementRepository(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Temp\VirtoCommerceCMLocalization"));
			var cachedElements = new CacheElementRepository(localElements);
			var instance = new CachedDatabaseElementRepository(repositoryFactory, cachedElements, x => x.Category != "");
			_container.RegisterInstance<IElementRepository>(instance);
		}
	}
}
