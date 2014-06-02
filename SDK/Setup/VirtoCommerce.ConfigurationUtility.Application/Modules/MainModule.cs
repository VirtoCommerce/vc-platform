using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.ConfigurationUtility.Application.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Implementations;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ConfigurationUtility.Main
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer _container;

        #region Constructors

        public MainModule(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Implementation of IModule

        public void Initialize()
        {
            RegisterViewsAndServices();

            var navigationManager = _container.Resolve<NavigationManager>();

            // Projects window
            var projectsViewModel = _container.Resolve<IProjectsViewModel>();
            var projectsNavigationItem = new NavigationItem(NavigationNames.ProjectsName, projectsViewModel);
            navigationManager.RegisterNavigationItem(projectsNavigationItem);

            navigationManager.Navigate(projectsNavigationItem);
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IProjectsViewModel, ProjectsViewModel>();
            _container.RegisterType<IConfigurationWizardViewModel, ConfigurationWizardViewModel>();
            _container.RegisterType<IProjectLocationStepViewModel, ProjectLocationStepViewModel>();
            _container.RegisterType<IDatabaseSettingsStepViewModel, DatabaseSettingsStepViewModel>();
            _container.RegisterType<ISearchSettingsStepViewModel, SearchSettingsStepViewModel>();
            _container.RegisterType<IConfirmationStepViewModel, ConfirmationStepViewModel>();
            _container.RegisterType<IConfigurationViewModel, ConfigurationViewModel>();
            _container.RegisterType<IProjectRepository, ProjectsRepository>();
            _container.RegisterType(typeof(IRepositoryFactory<>), typeof(UnityRepositoryFactory<>), new ContainerControlledLifetimeManager());
            _container.RegisterType(typeof(IViewModelsFactory<>), typeof(UnityViewModelsFactory<>), new ContainerControlledLifetimeManager());
            _container.RegisterType<IElementRepository, DummyElementRepository>(new ContainerControlledLifetimeManager());

            System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("/VirtoCommerce.ConfigurationUtility.Main;component/MainModuleDictionary.xaml", UriKind.Relative)
                });
        }

        #endregion
    }

    public class UnityViewModelsFactory<T> : IViewModelsFactory<T> where T : IViewModel
    {
        private readonly IUnityContainer _container;

        public UnityViewModelsFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
        {
            using (T retVal = parameters != null ? _container.Resolve<T>(parameters.Select(param => new ParameterOverride(param.Key, param.Value)).ToArray()) : _container.Resolve<T>())
            {
                return retVal;
            }
        }
    }
}