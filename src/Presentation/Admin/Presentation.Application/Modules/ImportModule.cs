using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VirtoCommerce.ManagementClient.Import.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Wizard;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.DataManagement.Services;

namespace VirtoCommerce.ManagementClient.Import
{
    public class ImportModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

		public ImportModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
            RegisterConfigurationViews();
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
			_container.RegisterType<IImportJobEntityFactory, ImportJobEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IImportRepository, DSImportClient>();
			_container.RegisterType<IImportService, ImportService>();
			_container.RegisterType<IDataManagementService, DataManagementService>();
			
            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/VirtoCommerce.ManagementClient.Import;component/ImportModuleDictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(resources);
        }

        private void RegisterConfigurationViews()
        {
			// Importing
			_container.RegisterType<IImportJobHomeViewModel, ImportJobHomeViewModel>();
			_container.RegisterType<IImportJobRunViewModel, ImportJobRunViewModel>();
			_container.RegisterType<IImportJobViewModel, ImportJobViewModel>();
			_container.RegisterType<IColumnMappingViewModel, ColumnMappingViewModel>();
			_container.RegisterType<ICreateImportJobViewModel, CreateImportJobViewModel>();
			_container.RegisterType<IImportJobOverviewStepViewModel, ImportJobOverviewStepViewModel>();
			_container.RegisterType<IImportJobMappingStepViewModel, ImportJobMappingStepViewModel>();
        }
    }
}
