using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Main.ViewModel;

namespace VirtoCommerce.ManagementClient.Main
{
	public class MainModule : IModule
	{
		private readonly IUnityContainer _container;

		public MainModule(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			RegisterViewsAndServices();

			var navigationManager = _container.Resolve<NavigationManager>();
			//Register home view
			var homeViewModel = _container.Resolve<IMainHomeViewModel>();
			var homeNavItem = new NavigationItem(NavigationNames.HomeName, homeViewModel);

			navigationManager.RegisterNavigationItem(homeNavItem);

			//Register menu view
			var menuNavItem = new NavigationMenuItem(NavigationNames.MenuName);
			menuNavItem.NavigateCommand = new DelegateCommand<NavigationItem>((x) => { navigationManager.Navigate(homeNavItem); });
			menuNavItem.Order = 1;
			menuNavItem.Caption = "Dashboard";
			menuNavItem.ItemBackground = Color.FromRgb(141, 187, 29);
			menuNavItem.ImageResourceKey = "Icon_Module_Home";

			navigationManager.RegisterNavigationItem(menuNavItem);
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IMainHomeViewModel, MainHomeViewModel>();

			var resources = new ResourceDictionary();
			resources.Source = new Uri("/VirtoCommerce.ManagementClient.Main;component/MainModuleDictionary.xaml", UriKind.Relative);
			Application.Current.Resources.MergedDictionaries.Add(resources);
		}
	}
}
