using System;
using System.Collections.Generic;
using System.Linq;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace CommerceFoundation.UI.FunctionalTests.Fulfillment
{
	public class TestFulfillmentViewModelFactory<T> : IViewModelsFactory<T> where T : IViewModel
	{

		private readonly Uri _storeServiceUri;
		private readonly Uri _catalogServiceUri;
		private readonly Uri _orderServiceUri;
	    private readonly Uri _appconfigServiceUri;



		public TestFulfillmentViewModelFactory(Uri storeServiceUri, Uri catalogServiceUri,
			Uri orderServiceUri, Uri appconfigServiceUri)
		{
			_storeServiceUri = storeServiceUri;

			_catalogServiceUri = catalogServiceUri;
			_orderServiceUri = orderServiceUri;
		    _appconfigServiceUri = appconfigServiceUri;
		}
		
		public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
		{
			if (typeof(T) == typeof(IStoreOverviewStepViewModel))
			{
				return (T)CreateStoreOverviewStep(parameters);
			}

			if (typeof(T) == typeof(IStoreLocalizationStepViewModel))
			{
				return (T)CreateStoreLocalizationStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IStoreTaxesStepViewModel))
			{
				return (T)CreateStoreTaxesStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IStorePaymentsStepViewModel))
			{
				return (T)CreatePaymentsStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IStoreNavigationStepViewModel))
			{
				return (T)CreateNavigationStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IStoreLinkedStoresStepViewModel))
			{
				return (T)CreateStoreLinkedStoresStepViewModel(parameters);
			}

			if (typeof(T) == typeof(IStoreSettingStepViewModel))
			{
				return (T)CreateSettingStepViewModel(parameters);
			}


			if (typeof(T) == typeof(IStoreSettingViewModel))
			{
				return (T)CreateStoreSettignsViewModel(parameters);
			}

			return default(T);
		}

	    private IStoreOverviewStepViewModel CreateStoreOverviewStep(params KeyValuePair<string, object>[] parameters)
		{
			var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);
			var catalogRepositoryFactory =
				new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(_catalogServiceUri);
			var fulfillmentRepositoryFactory =
				new DSRepositoryFactory<IFulfillmentCenterRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);
			var countryRepositoryFactory =
				new DSRepositoryFactory<ICountryRepository, DSOrderClient, OrderEntityFactory>(_orderServiceUri);


			IStoreEntityFactory entityFactory = new StoreEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value as Store;
			var retVal = new StoreOverviewStepViewModel(entityFactory, item, storeRepositoryFactory,
				catalogRepositoryFactory, countryRepositoryFactory, fulfillmentRepositoryFactory);

			return retVal;
		}

		private IStoreLocalizationStepViewModel CreateStoreLocalizationStepViewModel(
			params KeyValuePair<string, object>[] parameters)
		{
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);
		    var appConfigRepositoryFactory =
                new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(_appconfigServiceUri);

            IStoreEntityFactory entityFactory=new StoreEntityFactory();
		    var item = parameters.Single(x => x.Key == "item").Value as Store;
		    var retVal = new StoreLocalizationStepViewModel(entityFactory, item, storeRepositoryFactory,
		        appConfigRepositoryFactory);

			return retVal;
		}

	    private IStoreTaxesStepViewModel CreateStoreTaxesStepViewModel(params KeyValuePair<string, object>[] parameters)
	    {
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);

            IStoreEntityFactory entityFactory = new StoreEntityFactory();
            var item = parameters.Single(x => x.Key == "item").Value as Store;
            var retVal = new StoreTaxesStepViewModel(entityFactory, item, storeRepositoryFactory);

            return retVal;
	    }

	    private IStorePaymentsStepViewModel CreatePaymentsStepViewModel(params KeyValuePair<string, object>[] parameters)
	    {
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);
	        var paymentsRepositoryFactory =
	            new DSRepositoryFactory<IPaymentMethodRepository, DSOrderClient, OrderEntityFactory>(_orderServiceUri);

            IStoreEntityFactory entityFactory = new StoreEntityFactory();
            var item = parameters.Single(x => x.Key == "item").Value as Store;
            var retVal = new StorePaymentsStepViewModel(entityFactory, item, storeRepositoryFactory, paymentsRepositoryFactory);

	        return retVal;
	    }

	    private IStoreNavigationStepViewModel CreateNavigationStepViewModel(
	        params KeyValuePair<string, object>[] parameters)
	    {
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);

            IStoreEntityFactory entityFactory = new StoreEntityFactory();
            var item = parameters.Single(x => x.Key == "item").Value as Store;
            var retVal = new StoreNavigationStepViewModel(entityFactory, item, storeRepositoryFactory);

            return retVal;
	    }

	    private IStoreLinkedStoresStepViewModel CreateStoreLinkedStoresStepViewModel(
	        params KeyValuePair<string, object>[] parameters)
	    {
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);

            IStoreEntityFactory entityFactory = new StoreEntityFactory();
            var item = parameters.Single(x => x.Key == "item").Value as Store;
            var retVal = new StoreLinkedStoresStepViewModel(entityFactory, item, storeRepositoryFactory);

            return retVal;
	    }

	    private IStoreSettingStepViewModel CreateSettingStepViewModel(params KeyValuePair<string, object>[] parameters)
	    {
            var storeRepositoryFactory = new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(_storeServiceUri);

            IStoreEntityFactory entityFactory = new StoreEntityFactory();
            var item = parameters.Single(x => x.Key == "item").Value as Store;
            var retVal = new StoreSettingsStepViewModel(entityFactory, item, storeRepositoryFactory, this as IViewModelsFactory<IStoreSettingViewModel>);

            return retVal;
	    }


        private IStoreSettingViewModel CreateStoreSettignsViewModel(params KeyValuePair<string, object>[] parameters)
	    {

            var item = parameters.Single(x => x.Key == "item").Value as StoreSetting;

	        var retVal = new StoreSettingViewModel(item);
	        return retVal;
	    }
	}
}
