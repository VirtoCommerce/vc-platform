using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing
{
	public class MarketingModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAuthenticationContext _authContext;

		public MarketingModule(IUnityContainer container, IAuthenticationContext authContext)
		{
			_container = container;
			_authContext = authContext;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();
			RegisterConfigurationViews();
			if (_authContext.CheckPermission(PredefinedPermissions.MarketingContent_PublishingManage) ||
				_authContext.CheckPermission(PredefinedPermissions.MarketingDynamic_ContentManage) ||
				_authContext.CheckPermission(PredefinedPermissions.MarketingPromotionsManage))
			{

				var navigationManager = _container.Resolve<NavigationManager>();
				//Register main view
				var homeViewModel = _container.Resolve<IMainMarketingViewModel>();
				var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

				navigationManager.RegisterNavigationItem(homeNavItem);

				//Register menu view
			    var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName)
			    {
			        NavigateCommand = new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); }),
                    Caption = "Marketing",
                    Category = NavigationNames.ModuleName,
			        ImageResourceKey = "Icon_Module_Promotions",
			        ItemBackground = Color.FromRgb(244, 189, 26),
			        Order = 40
			    };
			    navigationManager.RegisterNavigationItem(menuNavItem);
			}
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IMarketingRepository, DSMarketingClient>();

			// store service
			if (!_container.IsRegistered<VirtoCommerce.Foundation.Stores.Factories.IStoreEntityFactory>())
			{
				_container.RegisterType<VirtoCommerce.Foundation.Stores.Factories.IStoreEntityFactory, VirtoCommerce.Foundation.Stores.Factories.StoreEntityFactory>(new ContainerControlledLifetimeManager());
			}
			if (!_container.IsRegistered<VirtoCommerce.Foundation.Stores.Repositories.IStoreRepository>())
			{
				_container.RegisterType<VirtoCommerce.Foundation.Stores.Repositories.IStoreRepository, VirtoCommerce.Foundation.Data.Stores.DSStoreClient>();
			}

			_container.RegisterType<IMainMarketingViewModel, MainMarketingViewModel>();
			_container.RegisterType<IMarketingHomeViewModel, MarketingHomeViewModel>();

			_container.RegisterType<ICartPromotionViewModel, CartPromotionViewModel>();
			_container.RegisterType<ICatalogPromotionViewModel, CatalogPromotionViewModel>();

			// wizards
			_container.RegisterType<ICreateCatalogPromotionViewModel, CreateCatalogPromotionViewModel>();
			_container.RegisterType<ICatalogPromotionOverviewStepViewModel, CatalogPromotionOverviewStepViewModel>();
			_container.RegisterType<ICatalogPromotionExpressionStepViewModel, CatalogPromotionExpressionStepViewModel>();
			_container.RegisterType<IItemTypeSelectionStepViewModel, ItemTypeSelectionStepViewModel>();

			_container.RegisterType<ICreateCartPromotionViewModel, CreateCartPromotionViewModel>();
			_container.RegisterType<ICartPromotionOverviewStepViewModel, CartPromotionOverviewStepViewModel>();
			_container.RegisterType<ICartPromotionExpressionStepViewModel, CartPromotionExpressionStepViewModel>();
			_container.RegisterType<ICartPromotionCouponStepViewModel, CartPromotionCouponStepViewModel>();

			ResourceDictionary resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.Marketing;component/MarketingModuleDictionary.xaml", UriKind.Relative);
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}

		private void RegisterConfigurationViews()
		{
		}
	}
}
