using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Presentation.Catalog.ViewModel;
using Presentation.Configuration;
using Presentation.Configuration.Model;
using Presentation.Core.Infrastructure.Navigation;
using Presentation.Fulfillment.ViewModel;
using Presentation.Fulfillment.ViewModel.Wizard;
using VirtoSoftware.CommerceFoundation.Catalogs.Repositories;
using VirtoSoftware.CommerceFoundation.Data.Catalogs;
using VirtoSoftware.CommerceFoundation.Data.Stores;
using VirtoSoftware.CommerceFoundation.Inventories.Factories;
using VirtoSoftware.CommerceFoundation.Inventories.Repositories;
using VirtoSoftware.CommerceFoundation.Orders.Repositories;
using VirtoSoftware.CommerceFoundation.Stores.Repositories;

namespace Presentation.Fulfillment
{
    public class FulfillmentModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly NavigationManager _navigationManager;

        public FulfillmentModule(IUnityContainer container, IRegionManager regionManager, NavigationManager navigationManager)
        {
            _container = container;
            _regionManager = regionManager;
            _navigationManager = navigationManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
            RegisterConfigurationViews();

            var navigationManager = _container.Resolve<NavigationManager>();

            //Register menu item
            //var homeViewModel = _container.Resolve<IInventoryHomeViewModel>();
            var homeViewModel = _container.Resolve<IMainFulfillmentViewModel>();
            var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

            navigationManager.RegisterNavigationItem(homeNavItem);

            var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName);
            menuNavItem.NavigateCommand = new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); });
            menuNavItem.Caption = "Fulfillment";
            menuNavItem.ImageResourceKey = "Fulfillment";//"/Presentation.Fulfillment;component/Resources/images/fulfillment.png";
            menuNavItem.Order = 60;

            navigationManager.RegisterNavigationItem(menuNavItem);

            //_regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => _container.Resolve<IFulfillmentNavigationCommandViewModel>().View);
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ISearchItemViewModel, SearchItemViewModel>();
            // _container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFulfillmentCenterRepository, DSStoreClient>();
            _container.RegisterType<ICatalogRepository, DSCatalogClient>();
            //_container.RegisterType<IInventoryRepository, DSInventoryClient>();

            ////Mock service
            _container.RegisterType<IInventoryRepository, Presentation.Fulfillment.Services.MockInventoryService>(new ContainerControlledLifetimeManager());

			_container.RegisterType<ISearchItemViewModel, SearchItemViewModel>();
			_container.RegisterType<IInventoryViewModel, InventoryViewModel>();
			_container.RegisterType<IReceiveInventoryViewModel, ReceiveInventoryViewModel>();
			_container.RegisterType<IInventoryHomeViewModel, InventoryHomeViewModel>();
            _container.RegisterType<IMainFulfillmentViewModel, MainFulfillmentViewModel>();
			_container.RegisterType<IEditQuantityViewModel, EditQuantityViewModel>();

            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/Presentation.Fulfillment;component/FulfillmentModuleDictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(resources);
        }

        private void RegisterConfigurationViews()
        {
            _container.RegisterType<IStoresSettingsViewModel, StoresSettingsViewModel>();

            _container.RegisterType<ICreateStoreViewModel, CreateStoreViewModel>();
            _container.RegisterType<IStoreViewModel, StoreViewModel>();
            _container.RegisterType<IStoreOverviewStepViewModel, StoreOverviewStepViewModel>();
            _container.RegisterType<IStoreLocalizationStepViewModel, StoreLocalizationStepViewModel>();
            _container.RegisterType<IStoreTaxesStepViewModel, StoreTaxesStepViewModel>();
            _container.RegisterType<IStorePaymentsStepViewModel, StorePaymentsStepViewModel>();
            _container.RegisterType<IStoreLinkedStoresStepViewModel, StoreLinkedStoresStepViewModel>();
            _container.RegisterType<IStoreSettingStepViewModel, StoreSettingsStepViewModel>();
            _container.RegisterType<IStoreNavigationStepViewModel, StoreNavigationStepViewModel>();

            _container.RegisterType<IStoreSettingViewModel, StoreSettingViewModel>();

            _container.RegisterType<IFulfillmentCentersSettingsViewModel, FulfillmentCentersSettingsViewModel>();
            _container.RegisterType<IFulfillmentCenterViewModel, FulfillmentCenterViewModel>();

            _container.RegisterType<ICreateFulfillmentCenterViewModel, CreateFulfillmentCenterViewModel>();
            _container.RegisterType<IFulfillmentCenterOverviewStepViewModel, FulfillmentCenterOverviewStepViewModel>();
            _container.RegisterType<IFulfillmentCenterAddressStepViewModel, FulfillmentCenterAddressStepViewModel>();

            _container.RegisterType<ICountryRepository, VirtoSoftware.CommerceFoundation.Data.Orders.DSOrderClient>();

            ConfigurationManager.Settings.Add(new ConfigurationSection { Caption = "Stores", Order = 70, ViewModel = _container.Resolve<IStoresSettingsViewModel>() });
            ConfigurationManager.Settings.Add(new ConfigurationSection { Caption = "Fulfillment Centers", Order = 60, ViewModel = _container.Resolve<IFulfillmentCentersSettingsViewModel>() });
        }
    }
}
