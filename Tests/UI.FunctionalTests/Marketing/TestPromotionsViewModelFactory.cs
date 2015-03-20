using System;
using System.Collections.Generic;
using System.Linq;
using CommerceFoundation.UI.FunctionalTests.Catalogs;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;

namespace CommerceFoundation.UI.FunctionalTests.DynamicContent
{
	public class TestMarketingViewModelFactory<T> : IViewModelsFactory<T> where T : IViewModel
    {
        #region Fields

        private readonly Uri _marketingServiceUri;
		private readonly Uri _catalogServiceUri;
		private readonly Uri _storeServiceUri;
		private readonly Uri _orderServiceUri;
		private readonly Uri _appConfigServiceUri;

        #endregion

        #region Constructor

		public TestMarketingViewModelFactory(Uri marketingServiceUri, Uri catalogServiceUri, Uri storeServiceUri, Uri orderServiceUri, Uri appConfigServiceUri)
        {
            _marketingServiceUri = marketingServiceUri;
			_catalogServiceUri = catalogServiceUri;
			_storeServiceUri = storeServiceUri;
			_orderServiceUri = orderServiceUri;
			_appConfigServiceUri = appConfigServiceUri;
        }

        #endregion

        public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
        {
            if (typeof (T) == typeof (ICartPromotionOverviewStepViewModel))
            {
                return (T) CreateCartPromotionOverviewStepViewModel(parameters);
            }

            if (typeof (T) == typeof (ICartPromotionCouponStepViewModel))
            {
                return (T) CreatePromotionCouponStepViewModel(parameters);
            }

            if (typeof (T) == typeof (ICartPromotionExpressionStepViewModel))
            {
                return (T) CreateCartPromotionExpressionStepViewModel(parameters);
            }

			if (typeof(T) == typeof(ICatalogPromotionOverviewStepViewModel))
			{
				return (T)CreateCatalogPromotionOverviewStepViewModel(parameters);
			}

			if (typeof(T) == typeof(ICatalogPromotionExpressionStepViewModel))
			{
				return (T)CreateCatalogPromotionExpressionStepViewModel(parameters);
			}

			//if (typeof(T) == typeof(ICartPromotionViewModel))
			//{
			//	return (T)CreateCartPromotionEditViewModel(parameters);
			//}

			//if (typeof(T) == typeof(ICatalogPromotionViewModel))
			//{
			//	return (T)CreateCatalogPromotionEditViewModel(parameters);
			//}
			
            return default(T);
        }

        #region Private methods

        private ICartPromotionOverviewStepViewModel CreateCartPromotionOverviewStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
            var repositoryFactory =
                new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
                    _marketingServiceUri);

			var storeRepositoryFactory =
                new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(
                    _storeServiceUri);

            var entityFactory = new MarketingEntityFactory();
            var item = parameters.SingleOrDefault(x => x.Key == "item").Value as CartPromotion;

			var retval = new CartPromotionOverviewStepViewModel(repositoryFactory, storeRepositoryFactory, entityFactory, item);

            return retval;
        }

		private ICatalogPromotionOverviewStepViewModel CreateCatalogPromotionOverviewStepViewModel(
			params KeyValuePair<string, object>[] parameters)
		{
			var repositoryFactory =
				new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
					_marketingServiceUri);

			var catalogRepositoryFactory =
				new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(
					_catalogServiceUri);

			var pricelistRepositoryFactory =
				new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(
					_catalogServiceUri);

			var entityFactory = new MarketingEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value as CartPromotion;

			var retval = new CatalogPromotionOverviewStepViewModel(repositoryFactory, catalogRepositoryFactory, pricelistRepositoryFactory, entityFactory, item);

			return retval;
		}

        private ICartPromotionCouponStepViewModel CreatePromotionCouponStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
			var repositoryFactory =
				new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
					_marketingServiceUri);
			
			var entityFactory = new MarketingEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value as CartPromotion;

            var retval = new CartPromotionCouponStepViewModel(repositoryFactory, entityFactory, item);

            return retval;
        }

		private ICartPromotionExpressionStepViewModel CreateCartPromotionExpressionStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
			var repositoryFactory =
				new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
					_marketingServiceUri);
			
			var storeRepositoryFactory =
				 new DSRepositoryFactory<IStoreRepository, DSCatalogClient, CatalogEntityFactory>(
					 _storeServiceUri);
			
			var appConfigRepositoryFactory =
				 new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(
					 _appConfigServiceUri);

			var shippingRepositoryFactory =
				 new DSRepositoryFactory<IShippingRepository, DSOrderClient, OrderEntityFactory>(
					 _orderServiceUri);

			var searchCategoryVmFactory =
				 new TestCatalogViewModelFactory<ISearchCategoryViewModel>(_catalogServiceUri, _appConfigServiceUri);

			var searchItemVmFactory =
				 new TestCatalogViewModelFactory<ISearchItemViewModel>(_catalogServiceUri, _appConfigServiceUri);

			var entityFactory = new MarketingEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value as CartPromotion;

			var retval = new CartPromotionExpressionStepViewModel(appConfigRepositoryFactory, shippingRepositoryFactory, searchCategoryVmFactory, searchItemVmFactory, repositoryFactory, storeRepositoryFactory, entityFactory, item);

            return retval;
        }

		private ICatalogPromotionExpressionStepViewModel CreateCatalogPromotionExpressionStepViewModel(
			params KeyValuePair<string, object>[] parameters)
		{
			var repositoryFactory =
				new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
					_marketingServiceUri);
			
			var appConfigRepositoryFactory =
				 new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(
					 _appConfigServiceUri);

			var shippingRepositoryFactory =
				 new DSRepositoryFactory<IShippingRepository, DSOrderClient, OrderEntityFactory>(
					 _orderServiceUri);

			var searchCategoryVmFactory =
				 new TestCatalogViewModelFactory<ISearchCategoryViewModel>(_catalogServiceUri, _appConfigServiceUri);

			var searchItemVmFactory =
				 new TestCatalogViewModelFactory<ISearchItemViewModel>(_catalogServiceUri, _appConfigServiceUri);

			var entityFactory = new MarketingEntityFactory();
			var item = parameters.SingleOrDefault(x => x.Key == "item").Value as CartPromotion;

			var retval = new CatalogPromotionExpressionStepViewModel(appConfigRepositoryFactory, repositoryFactory, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory, entityFactory, item);

			return retval;
		}

        #endregion

    }
}
