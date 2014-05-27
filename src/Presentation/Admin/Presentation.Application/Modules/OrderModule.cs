#region Usings

using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Orders.StateMachines;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Currencies;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders.StateMachines;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Order.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

#endregion

namespace VirtoCommerce.ManagementClient.Order
{
	public class OrderModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;

		public OrderModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
			_authContext = authContext;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();
			RegisterConfigurationViews();
			if (_authContext.CheckPermission(PredefinedPermissions.OrdersAll))
			{
				var navigationManager = _container.Resolve<NavigationManager>();
				//Register home view
				var homeViewModel = _container.Resolve<IOrderHomeViewModel>();
				var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

				navigationManager.RegisterNavigationItem(homeNavItem);

				//Register menu view
			    var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
			    {
			        NavigateCommand = new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); }),
                    Caption = "Orders",
                    Category = NavigationNames.ModuleName,
			        ImageResourceKey = "Icon_Module_Orders",
			        ItemBackground = Colors.OrangeRed,
			        Order = 51
			    };

			    navigationManager.RegisterNavigationItem(menuNavItem);
			}
		}

		#endregion

		protected void RegisterViewsAndServices()
		{			
			_container.RegisterType<IOrderEntityFactory, OrderEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IOrderStateController, OrderStateController>();
			_container.RegisterType<ICacheRepository, HttpCacheRepository>();
			_container.RegisterType<OrderClient>();
			_container.RegisterType<StoreClient>();
			_container.RegisterType<PaymentClient>();
			_container.RegisterType<PriceListClient>();
			_container.RegisterType<IPriceListAssignmentEvaluator, PriceListAssignmentEvaluator>();
			_container.RegisterType<IPriceListAssignmentEvaluationContext, PriceListAssignmentEvaluationContext>();
			_container.RegisterType<ICustomerSessionService, CustomerSessionService>();
            _container.RegisterType<ICurrencyService, CurrencyService>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IOrderRepository, DSOrderClient>();
			_container.RegisterType<ICountryRepository, DSOrderClient>();
			_container.RegisterType<IPaymentMethodRepository, DSOrderClient>();
			_container.RegisterType<IShippingRepository, DSOrderClient>();
			_container.RegisterType<ITaxRepository, DSOrderClient>();

			_container.RegisterService<IOrderService>(
				_container.Resolve<IServiceConnectionFactory>()
						  .GetConnectionString(OrderConfiguration.Instance.OrderServiceConnection.ServiceUri, OrderConfiguration.Instance.OrderServiceConnection.ForceHttps),
				OrderConfiguration.Instance.OrderServiceConnection.WSEndPointName);

			//Import
			_container.RegisterType<ITaxImportHomeViewModel, TaxImportJobHomeViewModel>();

			_container.RegisterType<IOrderHomeViewModel, OrderHomeViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IOrderViewModel, OrderViewModel>();

			_container.RegisterType<IOrderAddressViewModel, OrderAddressViewModel>();
			_container.RegisterType<IOrderContactViewModel, OrderContactViewModel>();

			_container.RegisterType<ILineItemAddViewModel, LineItemAddViewModel>();
			_container.RegisterType<ILineItemViewModel, LineItemViewModel>();

			_container.RegisterType<ISplitShipmentViewModel, SplitShipmentViewModel>();

			_container.RegisterType<IShipmentViewModel, ShipmentViewModel>();
			_container.RegisterType<IRmaRequestViewModel, RmaRequestViewModel>();

			//Create Refund Wizard
			_container.RegisterType<ICreateRmaRequestViewModel, CreateRmaRequestViewModel>();
			_container.RegisterType<IRmaRequestReturnItemsStepViewModel, RmaRequestReturnItemsStepViewModel>();
			_container.RegisterType<IRmaRequestRefundStepViewModel, RmaRequestRefundStepViewModel>();
			_container.RegisterType<IReturnItemViewModel, ReturnItemViewModel>();

			//Create exchange Wizard
			_container.RegisterType<ICreateExchangeViewModel, CreateExchangeViewModel>();
			_container.RegisterType<IExchangeOrderStepViewModel, ExchangeOrderStepViewModel>();

			//Create payment Wizard
			_container.RegisterType<ICreatePaymentViewModel, CreatePaymentViewModel>();
			_container.RegisterType<IPaymentMethodStepViewModel, PaymentMethodStepViewModel>();
			_container.RegisterType<IPaymentDetailsStepViewModel, PaymentDetailsStepViewModel>();

			_container.RegisterType<ICreateRefundViewModel, CreateRefundViewModel>();
			_container.RegisterType<IRefundDetailsStepViewModel, RefundDetailsStepViewModel>();
			_container.RegisterType<IRefundSummaryStepViewModel, RefundSummaryStepViewModel>();

			// settings. Shipping settings
			_container.RegisterType<IShippingMainSettingsViewModel, ShippingMainSettingsViewModel>();

			// settings. ShippingOption
			_container.RegisterType<IShippingOptionSettingsViewModel, ShippingOptionSettingsViewModel>();
			_container.RegisterType<ICreateShippingOptionViewModel, CreateShippingOptionViewModel>();
			_container.RegisterType<IShippingOptionOverviewStepViewModel, ShippingOptionOverviewStepViewModel>();
			_container.RegisterType<IShippingOptionPackagesStepViewModel, ShippingOptionPackagesStepViewModel>();
			_container.RegisterType<IShippingOptionViewModel, ShippingOptionViewModel>();
			_container
				.RegisterType<IShippingOptionAddShippingPackageViewModel, ShippingOptionAddShippingPackageViewModel>();

			// settings. ShippingMethod
			_container.RegisterType<IShippingMethodSettingsViewModel, ShippingMethodSettingsViewModel>();
			_container.RegisterType<ICreateShippingMethodViewModel, CreateShippingMethodViewModel>();
			_container.RegisterType<IShippingMethodOverviewStepViewModel, ShippingMethodOverviewStepViewModel>();
			_container.RegisterType<IShippingMethodSettingsStepViewModel, ShippingMethodSettingsStepViewModel>();
			_container.RegisterType<IShippingMethodViewModel, ShippingMethodViewModel>();

			// settings. ShippingPackage
			_container.RegisterType<IShippingPackageSettingsViewModel, ShippingPackageSettingsViewModel>();
			_container.RegisterType<ICreateShippingPackageViewModel, CreateShippingPackageViewModel>();

			// settings. Packaging
			_container.RegisterType<IPackagingViewModel, PackagingViewModel>();
			_container.RegisterType<ICreatePackagingViewModel, CreatePackagingViewModel>();
			_container.RegisterType<IPackagingOverviewStepViewModel, PackagingOverviewStepViewModel>();

			// generic
			_container.RegisterType<IGeneralLanguagesStepViewModel, GeneralLanguagesStepViewModel>();
			_container.RegisterType<IGeneralLanguageViewModel, GeneralLanguageViewModel>();

			// settings. Taxes
			_container.RegisterType<ITaxesMainSettingsViewModel, TaxesMainSettingsViewModel>();
			_container.RegisterType<ITaxSettingsViewModel, TaxSettingsViewModel>();
			_container.RegisterType<ICreateTaxViewModel, CreateTaxViewModel>();
			_container.RegisterType<ITaxOverviewStepViewModel, TaxOverviewStepViewModel>();
			_container.RegisterType<ITaxViewModel, TaxViewModel>();
			_container.RegisterType<ITaxValueViewModel, TaxValueViewModel>();

			// TaxCategory
			_container.RegisterType<ITaxCategorySettingsViewModel, TaxCategorySettingsViewModel>();
			_container.RegisterType<ICreateTaxCategoryViewModel, CreateTaxCategoryViewModel>();
			_container.RegisterType<ITaxCategoryOverviewStepViewModel, TaxCategoryOverviewStepViewModel>();
			_container.RegisterType<ITaxCategoryViewModel, TaxCategoryViewModel>();

			_container.RegisterType<IJurisdictionSettingsViewModel, JurisdictionSettingsViewModel>();
			_container.RegisterType<ICreateJurisdictionViewModel, CreateJurisdictionViewModel>();
			_container.RegisterType<IJurisdictionOverviewStepViewModel, JurisdictionOverviewStepViewModel>();
			_container.RegisterType<IJurisdictionViewModel, JurisdictionViewModel>();

			_container.RegisterType<IJurisdictionGroupSettingsViewModel, JurisdictionGroupSettingsViewModel>();
			_container.RegisterType<ICreateJurisdictionGroupViewModel, CreateJurisdictionGroupViewModel>();
			_container.RegisterType<IJurisdictionGroupOverviewStepViewModel, JurisdictionGroupOverviewStepViewModel>();
			_container.RegisterType<IJurisdictionGroupViewModel, JurisdictionGroupViewModel>();

			var resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.Order;component/OrderModuleDictionary.xaml", UriKind.Relative);
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}

		private void RegisterConfigurationViews()
		{
			_container.RegisterType<IPaymentMethodsSettingsViewModel, PaymentMethodsSettingsViewModel>();
			_container.RegisterType<IPaymentMethodViewModel, PaymentMethodViewModel>();

			_container.RegisterType<ICreatePaymentMethodViewModel, CreatePaymentMethodViewModel>();
			_container.RegisterType<IPaymentMethodOverviewStepViewModel, PaymentMethodOverviewStepViewModel>();
			_container.RegisterType<IPaymentMethodPropertiesStepViewModel, PaymentMethodPropertiesStepViewModel>();

			if (_authContext.CheckPermission(PredefinedPermissions.SettingsPayment_Methods))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.PaymentsSettingsHomeName, Caption = "Payments", Category = NavigationNames.ModuleName, Order = 50, ViewModel = _container.Resolve<IPaymentMethodsSettingsViewModel>() });
			}
			if (_authContext.CheckPermission(PredefinedPermissions.SettingsShippingOptions) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsShippingMethods) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsShippingPackages) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsJurisdiction) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsJurisdictionGroups))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.ShippingSettingsHomeName, Caption = "Shipping", Category = NavigationNames.ModuleName, Order = 40, ViewModel = _container.Resolve<IShippingMainSettingsViewModel>() });
			}
			if (_authContext.CheckPermission(PredefinedPermissions.SettingsTaxCategories) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsTaxImport) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsTaxes) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsJurisdiction) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsJurisdictionGroups))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.TaxesSettingsHomeName, Caption = "Taxes", Category = NavigationNames.ModuleName, Order = 30, ViewModel = _container.Resolve<ITaxesMainSettingsViewModel>() });
			}
		}
	}
}
