using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Configuration;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment
{
	public class FulfillmentModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;

		public FulfillmentModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
			_authContext = authContext;
		}

		#region IModule Members

		public void Initialize()
		{
			if (true)
			{
				RegisterViewsAndServices();
				RegisterConfigurationViews();

				if (_authContext.CheckPermission(PredefinedPermissions.FulfillmentPicklistsManage) ||
					_authContext.CheckPermission(PredefinedPermissions.FulfillmentInventoryReceive) ||
					_authContext.CheckPermission(PredefinedPermissions.FulfillmentReturnsManage))
				{
					var navigationManager = _container.Resolve<NavigationManager>();

					//Register menu item
					var homeViewModel = _container.Resolve<IMainFulfillmentViewModel>();
					var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

					navigationManager.RegisterNavigationItem(homeNavItem);

					var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
					{
					    NavigateCommand = new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); }),
                        Caption = "Fulfillment",
                        Category = NavigationNames.ModuleName,
					    ImageResourceKey = "Icon_Module_Fulfillment",
					    ItemBackground = Colors.DarkOliveGreen,
					    Order = 60
					};

				    navigationManager.RegisterNavigationItem(menuNavItem);
				}
			}
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IFulfillmentCenterRepository, DSStoreClient>();
			_container.RegisterType<IFulfillmentRepository, DSOrderClient>();
			_container.RegisterType<ICatalogRepository, DSCatalogClient>();
			_container.RegisterType<ICustomerRepository, DSCustomerClient>();
#if USE_MOCK
            _container.RegisterType<IInventoryRepository, VirtoCommerce.ManagementClient.Fulfillment.Services.MockInventoryService>(new ContainerControlledLifetimeManager());
#else
			_container.RegisterType<IInventoryRepository, DSInventoryClient>();
#endif
			_container.RegisterType<IInventoryViewModel, InventoryViewModel>();
			_container.RegisterType<IReceiveInventoryViewModel, ReceiveInventoryViewModel>();
			_container.RegisterType<IInventoryHomeViewModel, InventoryHomeViewModel>();
			_container.RegisterType<IPicklistHomeViewModel, PicklistHomeViewModel>();
			_container.RegisterType<ICompleteShipmentViewModel, CompleteShipmentViewModel>();
			_container.RegisterType<ICreatePicklistWizardViewModel, CreatePicklistWizardViewModel>();

			_container.RegisterType<ICreatePicklistStepViewModel, CreatePicklistStepViewModel>();
			_container.RegisterType<IPicklistViewModel, PicklistViewModel>();
			_container.RegisterType<IRmaHomeViewModel, RmaHomeViewModel>();
			_container.RegisterType<IRmaViewModel, RmaViewModel>();
			_container.RegisterType<IMainFulfillmentViewModel, MainFulfillmentViewModel>();
			_container.RegisterType<IEditQuantityViewModel, EditQuantityViewModel>();

			ResourceDictionary resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.Fulfillment;component/FulfillmentModuleDictionary.xaml", UriKind.Relative);
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
			_container.RegisterType<ISeoViewModel, StoreSeoViewModel>();

			_container.RegisterType<IStoreSettingViewModel, StoreSettingViewModel>();

			_container.RegisterType<IFulfillmentCentersSettingsViewModel, FulfillmentCentersSettingsViewModel>();
			_container.RegisterType<IFulfillmentCenterViewModel, FulfillmentCenterViewModel>();

			_container.RegisterType<ICreateFulfillmentCenterViewModel, CreateFulfillmentCenterViewModel>();
			_container.RegisterType<IFulfillmentCenterOverviewStepViewModel, FulfillmentCenterOverviewStepViewModel>();
			_container.RegisterType<IFulfillmentCenterAddressStepViewModel, FulfillmentCenterAddressStepViewModel>();




			_container.RegisterType<ICountryRepository, DSOrderClient>();

			if (!_container.IsRegistered<Foundation.Stores.Factories.IStoreEntityFactory>())
			{
				_container.RegisterType<Foundation.Stores.Factories.IStoreEntityFactory, Foundation.Stores.Factories.StoreEntityFactory>(new ContainerControlledLifetimeManager());
			}
			if (!_container.IsRegistered<IStoreRepository>())
			{
				_container.RegisterType<IStoreRepository, DSStoreClient>();
			}
			if (_authContext.CheckPermission(PredefinedPermissions.SettingsStores))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.StoresSettingsHomeName, Caption = "Stores", Category = NavigationNames.ModuleName, Order = 70, ViewModel = _container.Resolve<IStoresSettingsViewModel>() });
			}
			if (_authContext.CheckPermission(PredefinedPermissions.SettingsFulfillment))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.HomeName, Caption = "Fulfillment", Category = NavigationNames.ModuleName, Order = 60, ViewModel = _container.Resolve<IFulfillmentCentersSettingsViewModel>() });
			}
		}
	}
}
