#region Usings

using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Titles;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

#endregion

namespace VirtoCommerce.ManagementClient.Catalog
{
	public class CatalogModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;
		private readonly NavigationManager _navManager;

		public CatalogModule(IUnityContainer container, IAuthenticationContext authContext, NavigationManager navManager)
		{
			_container = container;
			_authContext = authContext;
			_navManager = navManager;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();

			if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage))
			{
				//Register menu item
				IViewModel homeViewModel = _container.Resolve<ICatalogMainViewModel>();
				var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

				_navManager.RegisterNavigationItem(homeNavItem);

				var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
					{
						NavigateCommand = new DelegateCommand<NavigationItem>((x) => _navManager.Navigate(homeNavItem)),
						Caption = "Catalogs", 
                        Category = NavigationNames.ModuleName,
						ImageResourceKey = "Icon_Module_Catalogs",
						Order = 30,
						ItemBackground = Color.FromRgb(80, 133, 215)
					};
				_navManager.RegisterNavigationItem(menuNavItem);
			}

			if (_authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage)
				|| _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage))
			{
				//Register PRICELIST menu item
				IViewModel homeViewModel = _container.Resolve<IMainPriceListViewModel>();
				var homeNavItemPriceList = new NavigationItem(NavigationNames.HomeNamePriceList, homeViewModel);
				_navManager.RegisterNavigationItem(homeNavItemPriceList);

				var menuNavItem = new NavigationMenuItem(NavigationNames.MenuNamePriceList)
					{
						NavigateCommand = new DelegateCommand<NavigationItem>((x) => _navManager.Navigate(homeNavItemPriceList)),
                        Caption = "Price Lists",
                        Category = NavigationNames.ModuleName,
						ImageResourceKey = "Icon_Module_PriceLists",
						ItemBackground = Color.FromRgb(211, 66, 58),
						Order = 31
					};
				_navManager.RegisterNavigationItem(menuNavItem);
			}
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ICatalogRepository, DSCatalogClient>();
			_container.RegisterType<IPricelistRepository, DSCatalogClient>();

			_container.RegisterType<IImportJobEntityFactory, ImportJobEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IImportRepository, DSImportClient>();
#if USE_MOCK			
			_container.RegisterType<IImportRepository, VirtoCommerce.ManagementClient.Catalog.Services.MockImportJobService>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ICatalogRepository, VirtoCommerce.ManagementClient.Catalog.Services.MockCatalogService>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IPriceListRepository, MockCatalogService>(new ContainerControlledLifetimeManager());
#else
			_container.RegisterType<IOrderEntityFactory, OrderEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IOrderRepository, DSOrderClient>();

			_container.RegisterService<IImportService>(
				_container.Resolve<IServiceConnectionFactory>().GetConnectionString(ImportConfiguration.Instance.ImportingService.ServiceUri),
				ImportConfiguration.Instance.ImportingService.WSEndPointName
				);
#endif

			//Create Catalog wizard
			_container.RegisterType<ICreateCatalogViewModel, CreateCatalogViewModel>();
			_container.RegisterType<ICatalogOverviewStepViewModel, CatalogOverviewStepViewModel>();

			_container.RegisterType<ICreateVirtualCatalogViewModel, CreateVirtualCatalogViewModel>();
			_container.RegisterType<IVirtualCatalogOverviewStepViewModel, VirtualCatalogOverviewStepViewModel>();

			//Create Category wizard
			_container.RegisterType<ICreateCategoryViewModel, CreateCategoryViewModel>();
			_container.RegisterType<ICategoryOverviewStepViewModel, CategoryOverviewStepViewModel>();
			_container.RegisterType<ICategoryPropertiesStepViewModel, CategoryPropertiesStepViewModel>();
			_container.RegisterType<ICategorySeoViewModel, CategorySeoViewModel>();

			//Create Item Wizard
			_container.RegisterType<ICreateItemViewModel, CreateItemViewModel>();
			_container.RegisterType<IItemTypeSelectionStepViewModel, ItemTypeSelectionStepViewModel>();
			_container.RegisterType<IItemOverviewStepViewModel, ItemOverviewStepViewModel>();
			_container.RegisterType<IItemPropertiesStepViewModel, ItemPropertiesStepViewModel>();
			_container.RegisterType<IItemPricingStepViewModel, ItemPricingStepViewModel>();
			_container.RegisterType<IItemSeoViewModel, ItemSeoViewModel>();

			// tree
			_container.RegisterType<ITreeCatalogViewModel, TreeCatalogViewModel>();
			_container.RegisterType<ITreeCategoryViewModel, TreeCategoryViewModel>();
			_container.RegisterType<ITreeVirtualCatalogViewModel, TreeVirtualCatalogViewModel>();

			_container.RegisterType<IQueryViewModel, QueryViewModel>();

			// catalog
			_container.RegisterType<ICatalogMainViewModel, CatalogMainViewModel>();
			_container.RegisterType<ICatalogHomeViewModel, CatalogHomeViewModel>();
			_container.RegisterType<ICatalogViewModel, CatalogViewModel>();
			_container.RegisterType<ICatalogDeleteViewModel, CatalogDeleteViewModel>();
			_container.RegisterType<ICategoryViewModel, CategoryViewModel>();
			_container.RegisterType<ILinkedCategoryViewModel, LinkedCategoryViewModel>();
			_container.RegisterType<IItemViewModel, ItemViewModel>();
			_container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();

			_container.RegisterType<IVirtualCatalogViewModel, VirtualCatalogViewModel>();

			_container.RegisterType<IPropertyViewModel, PropertyViewModel>();
			_container.RegisterType<IPropertySetViewModel, PropertySetViewModel>();

			_container.RegisterType<IPriceViewModel, PriceViewModel>();

			_container.RegisterType<IItemRelationViewModel, ItemRelationViewModel>();

			_container.RegisterType<IAssociationGroupEditViewModel, AssociationGroupEditViewModel>();
			_container.RegisterType<IAssociationGroupViewModel, AssociationGroupViewModel>();
			_container.RegisterType<IAssociationViewModel, AssociationViewModel>();

			_container.RegisterType<IEditorialReviewViewModel, EditorialReviewViewModel>();

			_container.RegisterType<ICategoryItemRelationViewModel, CategoryItemRelationViewModel>();

			_container.RegisterType<IItemAssetViewModel, ItemAssetViewModel>();

			// edit catalog dialogs
			_container.RegisterType<IPropertyAttributeViewModel, PropertyAttributeViewModel>();
			_container.RegisterType<IPropertyValueViewModel, PropertyValueViewModel>();

			// create category wizard dialogs
			_container.RegisterType<IPropertyValueBaseViewModel, PropertyValueBaseViewModel>();

			// Search dialogs
			_container.RegisterType<ISearchCategoryViewModel, SearchCategoryViewModel>();
			_container.RegisterType<ISearchItemViewModel, SearchItemViewModel>();

			// PriceLists
			_container.RegisterType<IMainPriceListViewModel, MainPriceListViewModel>();
			_container.RegisterType<IPriceListHomeViewModel, PriceListHomeViewModel>();
			_container.RegisterType<IPriceListViewModel, PriceListViewModel>();
			_container.RegisterType<ICreatePriceListViewModel, CreatePriceListViewModel>();
			_container.RegisterType<IPriceListOverviewStepViewModel, PriceListOverviewStepViewModel>();

			_container.RegisterType<IPriceListAssignmentHomeViewModel, PriceListAssignmentHomeViewModel>();
			_container.RegisterType<IPriceListAssignmentViewModel, PriceListAssignmentViewModel>();
			_container.RegisterType<ICreatePriceListAssignmentViewModel, CreatePriceListAssignmentViewModel>();
			_container.RegisterType<IPriceListAssignmentOverviewStepViewModel, PriceListAssignmentOverviewStepViewModel>();
			_container.RegisterType<IPriceListAssignmentSetConditionsStepViewModel, PriceListAssignmentSetConditionsStepViewModel>();
			_container.RegisterType<IPriceListAssignmentSetDatesStepViewModel, PriceListAssignmentSetDatesStepViewModel>();

			_container.RegisterType<ICountryRepository, DSOrderClient>();

			//Import
			_container.RegisterType<ICatalogImportJobHomeViewModel, CatalogImportJobHomeViewModel>();
			_container.RegisterType<IPricelistImportJobHomeViewModel, PricelistImportJobHomeViewModel>();

			//Shell Captions
			_container.RegisterType<ITitleHomeCaptionViewModel, CatalogHomeTitleViewModel>();


			ResourceDictionary resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.Catalog;component/CatalogModuleDictionary.xaml", UriKind.Relative);
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}


	}
}
