using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Configuration;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.AppConfig
{
    public class AppConfigModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IAuthenticationContext _authContext;

        public AppConfigModule(IUnityContainer container, IAuthenticationContext authContext)
        {
            _container = container;
            _authContext = authContext;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
            RegisterConfigurationViews();

            var navigationManager = _container.Resolve<NavigationManager>();

            //Register menu item
            var homeViewModel = _container.Resolve<IAppConfigSettingsViewModel>();
            var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

            navigationManager.RegisterNavigationItem(homeNavItem);
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IAppConfigEntityFactory, AppConfigEntityFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAppConfigRepository, DSAppConfigClient>();
            _container.RegisterService<ICacheService>(
                _container.Resolve<IServiceConnectionFactory>()
                          .GetConnectionString(AppConfigConfiguration.Instance.CacheServiceConnection.ServiceUri, AppConfigConfiguration.Instance.CacheServiceConnection.ForceHttps),
                AppConfigConfiguration.Instance.CacheServiceConnection.WSEndPointName);

            var resources = new ResourceDictionary
                {
                    Source =
                        new Uri("/VirtoCommerce.ManagementClient.AppConfig;component/AppConfigModuleDictionary.xaml", UriKind.Relative)
                };
            Application.Current.Resources.MergedDictionaries.Add(resources);
        }

        private void RegisterConfigurationViews()
        {
            _container.RegisterType<IAddParameterViewModel, AddParameterViewModel>();
            _container.RegisterType<IAppConfigSettingOverviewStepViewModel, AppConfigSettingOverviewStepViewModel>();
            _container.RegisterType<IAppConfigSettingEditViewModel, AppConfigSettingEditViewModel>();
            _container.RegisterType<IAppConfigSettingsViewModel, AppConfigSettingsViewModel>();
            _container.RegisterType<IAppConfigMainSettingsViewModel, AppConfigMainSettingsViewModel>();
            _container.RegisterType<ISystemJobsViewModel, SystemJobsViewModel>();
            _container.RegisterType<ISystemJobEditViewModel, SystemJobEditViewModel>();
            _container.RegisterType<ISystemJobOverviewStepViewModel, SystemJobOverviewStepViewModel>();
            _container.RegisterType<ISystemJobParametersStepViewModel, SystemJobParametersStepViewModel>();
            _container.RegisterType<IDisplayTemplatesViewModel, DisplayTemplatesViewModel>();
            _container.RegisterType<IDisplayTemplateViewModel, DisplayTemplateEditViewModel>();
            _container.RegisterType<IDisplayTemplateOverviewStepViewModel, DisplayTemplateOverviewStepViewModel>();
            _container.RegisterType<IDisplayTemplateConditionsStepViewModel, DisplayTemplateConditionsStepViewModel>();
            _container.RegisterType<ILocalizationHomeViewModel, LocalizationHomeViewModel>();
            _container.RegisterType<ICacheViewModel, CacheViewModel>();

            //EmailTemplates
            _container.RegisterType<IEmailTemplatesViewModel, EmailTemplatesViewModel>();
            _container.RegisterType<IEmailTemplateEditViewModel, EmailTemplateEditViewModel>();
            _container.RegisterType<IEmailTemplateOverviewStepViewModel, EmailTemplateOverviewStepViewModel>();
            _container.RegisterType<IEmailTemplateLanguagesStepViewModel, EmailTemplateLanguagesStepViewModel>();

            _container.RegisterType<IEmailTemplateAddLanguageViewModel, EmailTemplateAddLanguageViewModel>();

            //Localization
            _container.RegisterType<ILocalizationMainViewModel, LocalizationMainViewModel>();
            _container.RegisterType<ILocalizationHomeViewModel, LocalizationHomeViewModel>();
            _container.RegisterType<ILocalizationEditViewModel, LocalizationEditViewModel>();

            //Import
            _container.RegisterType<ILocalizationImportJobHomeViewModel, LocalizationImportJobHomeViewModel>();

            //Wizards
            _container.RegisterType<ICreateAppConfigSettingViewModel, CreateAppConfigSettingViewModel>();
            _container.RegisterType<ICreateSystemJobViewModel, CreateSystemJobViewModel>();
            _container.RegisterType<ICreateDisplayTemplateViewModel, CreateDisplayTemplateViewModel>();
            _container.RegisterType<ICreateEmailTemplateViewModel, CreateEmailTemplateViewModel>();

            if (_authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSettings) ||
                _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSystemJobs) ||
                _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigEmailTemplates) ||
                _authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigDisplayTemplates))
            {
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.HomeName, Caption = "Application", Category = NavigationNames.ModuleName, Order = 1000, ViewModel = _container.Resolve<IAppConfigMainSettingsViewModel>() });
            }
        }
    }
}
