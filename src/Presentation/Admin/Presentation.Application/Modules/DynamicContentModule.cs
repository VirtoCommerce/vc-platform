using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.Windows;
using System;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent
{
	public class DynamicContentModule : IModule
	{
		private readonly IUnityContainer _container;
	    private readonly IAuthenticationContext _authContext;

        public DynamicContentModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
            _authContext = authContext;
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
			_container.RegisterType<IDynamicContentEntityFactory, DynamicContentEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IDynamicContentRepository, DSDynamicContentClient>();

			////Mock service
			//_container.RegisterType<IDynamicContentRepository, VirtoCommerce.ManagementClient.DynamicContent.Services.MockDynamicContentService>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IDynamicContentHomeViewModel, DynamicContentHomeViewModel>();
			_container.RegisterType<IDynamicContentItemViewModel, DynamicContentItemViewModel>();
			_container.RegisterType<IPropertyEditViewModel, PropertyEditViewModel>();
			_container.RegisterType<IContentPublishingHomeViewModel, ContentPublishingHomeViewModel>();
			_container.RegisterType<IContentPublishingItemViewModel, ContentPublishingItemViewModel>();
			_container.RegisterType<ISearchCategoryViewModel, SearchCategoryViewModel>();
			
			//Create Dynamic content Wizard
			_container.RegisterType<ICreateDynamicContentItemViewModel, CreateDynamicContentItemViewModel>();
			_container.RegisterType<IDynamicContentItemOverviewStepViewModel, DynamicContentItemOverviewStepViewModel>();
			_container.RegisterType<IDynamicContentItemPropertiesStepViewModel, DynamicContentItemPropertiesStepViewModel>();

			//Create Content Publishing Wizard
			_container.RegisterType<ICreateContentPublishingItemViewModel, CreateContentPublishingItemViewModel>();
			_container.RegisterType<IContentPublishingOverviewStepViewModel, ContentPublishingOverviewStepViewModel>();
			_container.RegisterType<IContentPublishingContentPlacesStepViewModel, ContentPublishingContentPlacesStepViewModel>();
			_container.RegisterType<IContentPublishingDynamicContentStepViewModel, ContentPublishingDynamicContentStepViewModel>();
			_container.RegisterType<IContentPublishingConditionsStepViewModel, ContentPublishingConditionsStepViewModel>();

			_container.RegisterType<ICountryRepository, DSOrderClient>();

			ResourceDictionary resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.DynamicContent;component/DynamicContentModuleDictionary.xaml", UriKind.Relative);
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}

        private void RegisterConfigurationViews()
        {
            _container.RegisterType<IContentPlaceSettingsViewModel, ContentPlaceSettingsViewModel>();
            _container.RegisterType<IContentPlaceViewModel, ContentPlaceViewModel>();

            //Create Content Place Wizard
            _container.RegisterType<ICreateContentPlaceViewModel, CreateContentPlaceViewModel>();
            _container.RegisterType<IContentPlaceOverviewStepViewModel, ContentPlaceOverviewStepViewModel>();

            if (_authContext.CheckPermission(PredefinedPermissions.SettingsContent_Places))
            {
                ConfigurationManager.Settings.Add(new ConfigurationSection { IdTab = NavigationNames.HomeNameDynamicContent, Caption = "Content places", Category = NavigationNames.ModuleName, Order = 60, ViewModel = _container.Resolve<IContentPlaceSettingsViewModel>() });
            }
        }
	}
}
