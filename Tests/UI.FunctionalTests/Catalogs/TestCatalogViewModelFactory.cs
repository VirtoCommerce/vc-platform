using System;
using System.Collections.Generic;
using System.Linq;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using UI.FunctionalTests.Helpers.Common;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;

namespace CommerceFoundation.UI.FunctionalTests.Catalogs
{
	public class TestCatalogViewModelFactory<T> : IViewModelsFactory<T> where T : IViewModel
	{
		private readonly Uri _catalogServiceUri;
		private readonly Uri _appConfigServiceUri;
		public TestCatalogViewModelFactory(Uri catalogServiceUri, Uri appConfigServiceUri)
		{
			_catalogServiceUri = catalogServiceUri;
			_appConfigServiceUri = appConfigServiceUri;
		}

		public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
		{

			if (typeof(T) == typeof(ICatalogOverviewStepViewModel))
			{
				return (T)CreateCatalogOverviewStepViewModel(parameters);
			}

			if (typeof(T) == typeof(ICategoryOverviewStepViewModel))
			{
				return (T)CreateCategoryOverviewStepViewModel(parameters);
			}

			if (typeof(T) == typeof(ICategoryPropertiesStepViewModel))
			{
				return (T)CreateCategoryPropertiesStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IPropertyValueBaseViewModel))
			{
				return (T)CreatePropertyValueBaseViewModel(parameters);
			}

			if (typeof(T) == typeof(IPriceListOverviewStepViewModel))
			{
				return (T)CeratePriceListOverviewStep(parameters);
			}
			if (typeof(T) == typeof(IPriceListViewModel))
			{
				return (T)CreatePriceList(parameters);
			}
			
			return default(T);
		}

		private ICatalogOverviewStepViewModel CreateCatalogOverviewStepViewModel(IEnumerable<KeyValuePair<string, object>> parameters)
		{
			var catalogRepositoryFactory = new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var appConfigRepositoryFactory = new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(_appConfigServiceUri);

			var entityFactory = new CatalogEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value;
			var retVal = new CatalogOverviewStepViewModel(catalogRepositoryFactory, appConfigRepositoryFactory, entityFactory, item as Catalog);
			return retVal;
		}

		private ICategoryOverviewStepViewModel CreateCategoryOverviewStepViewModel(IEnumerable<KeyValuePair<string, object>> parameters)
		{
			var catalogRepositoryFactory = new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var entityFactory = new CatalogEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "itemModel").Value as CategoryStepModel;
			var retVal = new CategoryOverviewStepViewModel(catalogRepositoryFactory, null, entityFactory, item);
			return retVal;
		}

		private ICategoryPropertiesStepViewModel CreateCategoryPropertiesStepViewModel(IEnumerable<KeyValuePair<string, object>> parameters)
		{
			var catalogRepositoryFactory = new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var entityFactory = new CatalogEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "itemModel").Value as CategoryStepModel;
			var retVal = new CategoryPropertiesStepViewModel(catalogRepositoryFactory, null, entityFactory, item);
			return retVal;
		}
		
		private IPropertyValueBaseViewModel CreatePropertyValueBaseViewModel(KeyValuePair<string, object>[] parameters)
		{
			return new PropertyValueBaseViewModel(null, (PropertyAndPropertyValueBase)parameters[0].Value, "en-US");
		}


		private IPriceListOverviewStepViewModel CeratePriceListOverviewStep(params KeyValuePair<string, object>[] parameters)
		{
			var priceListRepositoryFactory = new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var appConfigRepositoryFactory = new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(_appConfigServiceUri);

			ICatalogEntityFactory entityFactory = new CatalogEntityFactory();
			IAuthenticationContext authContext = new TestAuthenticationContext();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value;
			var retVal = new PriceListOverviewStepViewModel(priceListRepositoryFactory, appConfigRepositoryFactory, entityFactory,
															authContext, item as Pricelist);
			return retVal;
		}

		private IPriceListViewModel CreatePriceList(params KeyValuePair<string, object>[] parameters)
		{
			var priceListRepositoryFactory = new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var appConfigRepositoryFactory = new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(_appConfigServiceUri);

			// create Item using EntityFactory
			var entityFactory = new CatalogEntityFactory();

			IAuthenticationContext authenticationContext = new TestAuthenticationContext();
			var navigationManager = new TestNavigationManager();

			var item = parameters.SingleOrDefault(x => x.Key == "item").Value;
			var retVal = new PriceListViewModel(priceListRepositoryFactory, appConfigRepositoryFactory, null, entityFactory,
															navigationManager, authenticationContext, item as Pricelist);

			return retVal;
		}
	}
}
