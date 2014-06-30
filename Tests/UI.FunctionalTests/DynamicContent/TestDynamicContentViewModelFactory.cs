using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;

namespace CommerceFoundation.UI.FunctionalTests.DynamicContent
{
    public class TestDynamicContentViewModelFactory<T> : IViewModelsFactory<T> where T : IViewModel
    {
        #region Fields

        private readonly Uri _dynamicContentServiceUri;

        #endregion

        #region Constructor

        public TestDynamicContentViewModelFactory(Uri dynamicContentServiceUri)
        {
            _dynamicContentServiceUri = dynamicContentServiceUri;
        }

        #endregion

        public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
        {
            if (typeof(T) == typeof(IContentPublishingOverviewStepViewModel))
            {
                return (T)CreateContentPublishingOverviewStepViewModel(parameters);
            }

            if (typeof(T) == typeof(IContentPublishingContentPlacesStepViewModel))
            {
                return (T)CreateContentPublishingContentPlacesStepViewModel(parameters);
            }

            if (typeof(T) == typeof(IContentPublishingDynamicContentStepViewModel))
            {
                return (T)CreateContentPublishingDynamicContentStepViewModel(parameters);
            }

            if (typeof(T) == typeof(IContentPublishingConditionsStepViewModel))
            {
                return (T)CreateContentPublishingConditionsStepViewModel(parameters);
            }

            return default(T);
        }

        #region Private methods

        private IContentPublishingOverviewStepViewModel CreateContentPublishingOverviewStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
            var repositoryFactory =
                new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                    _dynamicContentServiceUri);

            IDynamicContentEntityFactory entityFactory = new DynamicContentEntityFactory();
            var item = parameters.SingleOrDefault(x => x.Key == "item").Value as DynamicContentPublishingGroup;

            var retval = new ContentPublishingOverviewStepViewModel(repositoryFactory, entityFactory, item);

            return retval;
        }

        private IContentPublishingContentPlacesStepViewModel CreateContentPublishingContentPlacesStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
            var repositoryFactory =
                  new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                      _dynamicContentServiceUri);

            IDynamicContentEntityFactory entityFactory = new DynamicContentEntityFactory();
            var item = parameters.SingleOrDefault(x => x.Key == "item").Value as DynamicContentPublishingGroup;

            var retval = new ContentPublishingContentPlacesStepViewModel(repositoryFactory, entityFactory, item);

            return retval;
        }

        private IContentPublishingDynamicContentStepViewModel CreateContentPublishingDynamicContentStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
            var repositoryFactory =
                 new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                     _dynamicContentServiceUri);

            IDynamicContentEntityFactory entityFactory = new DynamicContentEntityFactory();
            var item = parameters.SingleOrDefault(x => x.Key == "item").Value as DynamicContentPublishingGroup;

            var retval = new ContentPublishingDynamicContentStepViewModel(repositoryFactory, entityFactory, item);

            return retval;
        }

        private IContentPublishingConditionsStepViewModel CreateContentPublishingConditionsStepViewModel(
            params KeyValuePair<string, object>[] parameters)
        {
            var repositoryFactory =
                 new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                     _dynamicContentServiceUri);

            var storeRepositoryFactory =
                 new DSRepositoryFactory<IStoreRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                     _dynamicContentServiceUri);

            var countryRepositoryFactory =
                 new DSRepositoryFactory<ICountryRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                     _dynamicContentServiceUri);

            var appConfigRepositoryFactory =
               new DSRepositoryFactory<IAppConfigRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                     _dynamicContentServiceUri);

            var searchCategoryVmFactory =
                 new TestDynamicContentViewModelFactory<ISearchCategoryViewModel>(
                     _dynamicContentServiceUri);

            IDynamicContentEntityFactory entityFactory = new DynamicContentEntityFactory();
            var item = parameters.SingleOrDefault(x => x.Key == "item").Value as DynamicContentPublishingGroup;

            var retval = new ContentPublishingConditionsStepViewModel(appConfigRepositoryFactory, countryRepositoryFactory, searchCategoryVmFactory, storeRepositoryFactory, repositoryFactory, entityFactory, item);

            return retval;
        }

        #endregion

    }
}
