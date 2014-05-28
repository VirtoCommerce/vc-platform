using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Reporting;
using VirtoCommerce.Foundation.Reporting.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Reporting
{
    public class ReportingModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IAuthenticationContext _authContext;

        public ReportingModule(IUnityContainer container, IAuthenticationContext authContext)
        {
            _container = container;
            _authContext = authContext;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();
            RegisterConfigurationViews();
            if (_authContext.CheckPermission(PredefinedPermissions.ReportingViewReports))
            {
                var navigationManager = _container.Resolve<NavigationManager>();
                //Register home view
                var homeViewModel = _container.Resolve<IReportingHomeViewModel>();
                var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

                navigationManager.RegisterNavigationItem(homeNavItem);

                //Register menu view
                var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
                {
                    NavigateCommand = new DelegateCommand<NavigationItem>((x) => navigationManager.Navigate(homeNavItem)),
                    Caption = "Reports",
                    Category = NavigationNames.ModuleName,
                    ImageResourceKey = "Icon_Module_Reporting",
                    ItemBackground = Colors.LightSlateGray,
                    Order = 51
                };

                navigationManager.RegisterNavigationItem(menuNavItem);
            }
        }

        private void RegisterViewsAndServices()
        {
            _container.RegisterType<IReportViewModel, ReportViewModel>();

            _container.RegisterType<IReportingHomeViewModel, ReportingHomeViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterService<IReportingService>(
                _container.Resolve<IServiceConnectionFactory>()
                    .GetConnectionString(ReportingConfiguration.Instance.ReportingService.ServiceUri),
                ReportingConfiguration.Instance.ReportingService.WSEndPointName
                );

            var resources = new ResourceDictionary
            {
                Source =
                    new Uri("/VirtoCommerce.ManagementClient.Reporting;component/ReportingModuleDictionary.xaml",
                        UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resources);
        }

        private void RegisterConfigurationViews()
        {
        }
    }
}
