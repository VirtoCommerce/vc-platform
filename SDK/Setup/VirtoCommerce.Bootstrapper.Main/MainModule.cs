using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.Bootstrapper.Main.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

using VirtoCommerce.Bootstrapper.Main.ViewModels;

namespace VirtoCommerce.Bootstrapper.Main
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer _container;

        private BootstrapperApplication _installer;

        public MainModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            try
            {
                _installer = _container.Resolve<BootstrapperApplication>();
            }
            catch
            {
            }

            RegisterViewsAndServices();

            if (_installer == null || (_installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full))
            {
                var navigationManager = _container.Resolve<NavigationManager>();

                var mainViewModel = _container.Resolve<IMainViewModel>();
                var mainNavigationItem = new NavigationItem(NavigationNames.Main, mainViewModel);
                navigationManager.RegisterNavigationItem(mainNavigationItem);

                var helpViewModel = _container.Resolve<IHelpViewModel>();
                var helpNavigationItem = new NavigationItem(NavigationNames.Help, helpViewModel);
                navigationManager.RegisterNavigationItem(helpNavigationItem);

                navigationManager.Navigate(mainNavigationItem);

                if (_installer != null) _installer.Engine.CloseSplashScreen();
            }
        }

        protected void RegisterViewsAndServices()
        {
			//ViewModels factory
			_container.RegisterType(typeof(IViewModelsFactory<>), typeof(UnityViewModelsFactory<>), new ContainerControlledLifetimeManager());
			_container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IHelpViewModel, HelpViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ILayoutStepViewModel, LayoutStepViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IInstallationStepViewModel, InstallationStepViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IModificationStepViewModel, ModificationStepViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOperationProgressStepViewModel, OperationProgressStepViewModel>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IOperationCompletedStepViewModel, OperationCompletedStepViewModel>(new ContainerControlledLifetimeManager());

            if (_installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full)
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/VirtoCommerce.Bootstrapper.Main;component/MainModuleDictionary.xaml", UriKind.Relative) });
            }
        }
    }
}