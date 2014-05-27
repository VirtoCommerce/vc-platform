using System;
using System.Waf.Applications.Services;
using System.Waf.VirtoCommerce.ManagementClient.Services;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers
{
	public class CustomersModule : IModule
	{

		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;

		public CustomersModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
			_authContext = authContext;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();

			if (_authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll) ||
				_authContext.CheckPermission(PredefinedPermissions.CustomersViewAssignedCases))
			{
				var navigationManager = _container.Resolve<NavigationManager>();

				//Register menu item
				var homeViewModel = _container.Resolve<ICustomersMainViewModel>();
				var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

				navigationManager.RegisterNavigationItem(homeNavItem);

			    var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
			    {
			        NavigateCommand = new DelegateCommand<NavigationItem>((x) => navigationManager.Navigate(homeNavItem)),
                    Caption = "Customer Service",
                    Category = NavigationNames.ModuleName,
			        ImageResourceKey = "Icon_Module_Customers",
			        ItemBackground = Color.FromRgb(248, 154, 45),
			        Order = 52
			    };

			    navigationManager.RegisterNavigationItem(menuNavItem);
			}

		}

		#endregion

		private void RegisterViewsAndServices()
		{

			_container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(new ContainerControlledLifetimeManager());

			_container.RegisterType<ICustomersMainViewModel, CustomersMainViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ICustomersHomeViewModel, CustomersHomeViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ICustomersDetailViewModel, CustomersDetailViewModel>();
			_container.RegisterType<CaseDetailViewModel>();
			_container.RegisterType<CustomerDetailViewModel>();
			//_container.RegisterType<ICustomerViewModel, CustomerViewModel>();

			// search
			_container.RegisterType<ISearchHomeViewModel, SearchHomeViewModel>(new ContainerControlledLifetimeManager());



			_container.RegisterType<ICaseAlertsEvaluator, CaseAlertsEvaluator>();
			_container.RegisterType<ICaseAlertEvaluationContext, CaseAlertEvaluationContext>();
			_container.RegisterType<ICaseAlertsService, CaseAlertsService>();


			_container.RegisterType<ICustomerRepository, DSCustomerClient>();

			//Dialogs
			_container.RegisterType<IAddressDialogViewModel, AddressDialogViewModel>();
			_container.RegisterType<IKnowledgeGroupViewModel, KnowledgeGroupViewModel>();
			_container.RegisterType<IPhoneNumberDialogViewModel, PhoneNumberDialogViewModel>();
			_container.RegisterType<IEmailDialogViewModel, EmailDialogViewModel>();
			_container.RegisterType<ICreateCustomerDialogViewModel, CreateCustomerDialogViewModel>();
			_container.RegisterType<ICreateUserDialogViewModel, CreateUserDialogViewModel>();
			_container.RegisterType<ICustomerChoiceDialogViewModel, CustomerChoiceDialogViewModel>();

			//Settings
			_container.RegisterType<ICustomersMainSettingsViewModel, CustomersMainSettingsViewModel>();
			_container.RegisterType<ILabelsSettingsViewModel, LabelsSettingsViewModel>();
			_container.RegisterType<ILabelViewModel, LabelViewModel>();

			_container.RegisterType<ICaseRulesSettingsViewModel, CaseRulesSettingsViewModel>();
			_container.RegisterType<ICaseRuleViewModel, CaseRuleViewModel>();
			_container.RegisterType<ICreateCaseRuleViewModel, CreateCaseRuleViewModel>();
			_container.RegisterType<ICaseRuleOverviewStepViewModel, CaseRuleOverviewStepViewModel>();

			_container.RegisterType<ICasePropertySetsSettingsViewModel, CasePropertySetsSettingsViewModel>();
			_container.RegisterType<ICasePropertySetViewModel, CasePropertySetViewModel>();
			_container.RegisterType<ICasePropertyViewModel, CasePropertyViewModel>();
			_container.RegisterType<IMultiLineEditViewModel, MultiLineEditViewModel>();

			_container.RegisterType<ICaseTemplatesSettingsViewModel, CaseTemplatesSettingsViewModel>();
			_container.RegisterType<ICaseTemplateViewModel, CaseTemplateViewModel>();
			_container.RegisterType<ICreateCaseTemplateViewModel, CreateCaseTemplateViewModel>();
			_container.RegisterType<ICaseTemplateOverviewStepViewModel, CaseTemplateOverviewStepViewModel>();
			_container.RegisterType<ICaseTemplatePropertyViewModel, CaseTemplatePropertyViewModel>();
			_container.RegisterType<IFileDialogService, FileDialogService>();

			if (_authContext.CheckPermission(PredefinedPermissions.SettingsCustomerRules) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsCustomerInfo) ||
				_authContext.CheckPermission(PredefinedPermissions.SettingsCustomerCaseTypes))
			{
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.HomeName, Caption = "Customers", Category = NavigationNames.ModuleName, Order = 100, ViewModel = _container.Resolve<ICustomersMainSettingsViewModel>() });
			}
			var resources = new ResourceDictionary
				{
					Source =
						new Uri("/VirtoCommerce.ManagementClient.Customers;component/CustomersModuleDictionary.xaml",
								UriKind.Relative)
				};
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}

	}
}
