using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.Configuration
{
	public class ConfigurationModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;

		public ConfigurationModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
			_authContext = authContext;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();
			RegisterConfigurationViews();

			if (HasPermissions())
			{
				var navigationManager = _container.Resolve<NavigationManager>();
				//Register home view
				var homeViewModel = _container.Resolve<IConfigurationHomeViewModel>();
				var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);
				navigationManager.RegisterNavigationItem(homeNavItem);
				//Register menu view
				var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
					{
						NavigateCommand =
							new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); }),
                        Caption = "Settings",
                        Category = NavigationNames.ModuleName,
						ItemBackground = Color.FromRgb(132, 94, 178),
						ImageResourceKey = "Icon_Module_Settings",
						Order = 110
					};
				navigationManager.RegisterNavigationItem(menuNavItem);
			}
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IBuildSettingsRepository, DSSearchClient>();
			_container.RegisterType<ISearchEntityFactory, SearchEntityFactory>();

			_container.RegisterType<IConfigurationHomeViewModel, ConfigurationHomeViewModel>(new ContainerControlledLifetimeManager());

			var resources = new ResourceDictionary
				{
					Source =
						new Uri("/VirtoCommerce.ManagementClient.Configuration;component/ConfigurationModuleDictionary.xaml",
								UriKind.Relative)
				};
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}

		private void RegisterConfigurationViews()
		{
			_container.RegisterType<IBuildSettingsViewModel, BuildSettingsViewModel>();

			//ConfigurationManager.Settings.Add(new ConfigurationSection { Caption = "General", Order = 80 });
			if (_authContext.CheckPermission(PredefinedPermissions.SettingsSearch))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { Caption = "Search", Category = NavigationNames.ModuleName, Order = 20, ViewModel = _container.Resolve<IBuildSettingsViewModel>() });
			}
		}

		private bool HasPermissions()
		{
			return _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSettings) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSystemJobs) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigEmailTemplates) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigDisplayTemplates) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsCustomerInfo) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsCustomerCaseTypes) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsCustomerRules) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsFulfillment) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsStores) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsJurisdiction) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsJurisdictionGroups) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsTaxCategories) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsTaxes) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsTaxImport) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsShippingMethods) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsShippingPackages) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsShippingOptions) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsContent_Places) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsPayment_Methods) ||
				   _authContext.CheckPermission(PredefinedPermissions.SettingsSearch);
		}
	}
}
