using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Commands;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation
{
    /// <summary>
    /// Represent-s navigation functionality (Switch view, Main Menu)
    /// </summary>
    public sealed class NavigationManager : INavigationManager
    {
        private List<NavigationItem> _registeredNavigationItems = new List<NavigationItem>();
        private IRegionManager _regionManager;
        private GlobalCommandsProxy _globalCommandsProxy;

        private IRegion _mainRegion;
        private IRegion MainRegion 
        {
            get
            {
                if (_mainRegion == null)
                {
                    _mainRegion = _regionManager.Regions[RegionNames.MainRegion];
                }
                return _mainRegion;
            }
        }


        public NavigationManager(IRegionManager regionManager, GlobalCommandsProxy globalCommandsProxy)
        {
            _globalCommandsProxy = globalCommandsProxy;
            _regionManager = regionManager;
        }



        #region Public methods

        private static string _currentNavigationItemName;
        /// <summary>
        /// Navigate to Parent. If parent is unknown then to default 
        /// Current doesn't close
        /// </summary>
        public void Back()
        {
            NavigationItem navItem = GetNavigationItemByName(_currentNavigationItemName);
            if (navItem != null)
            {
                {
                    string backNavigationItemName = navItem.HomeNavigationItemName;
                    NavigationItem backNavItem = GetNavigationItemByName(backNavigationItemName);
                    if (backNavItem != null)
                    {
                        Navigate(backNavItem);
                    }
                    else
                    {
                        NavigateToDefaultPage();
                    }
                }
            }
        }


        /// <summary>
        /// Gets the name of the navigation item by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public NavigationItem GetNavigationItemByName(string name)
        {
            return _registeredNavigationItems.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Uns the register navigation item.
        /// </summary>
        /// <param name="navItem">The nav item.</param>
        public void UnRegisterNavigationItem(NavigationItem navItem)
        {
            if (navItem == null)
            {
                throw new ArgumentNullException("navItem");
            }

            _registeredNavigationItems.RemoveAll(x => x.Name == navItem.Name);

            var mainView = MainRegion.GetView(navItem.Name);
            if (mainView != null)
            {
                MainRegion.Remove(mainView);
            }
        }
        /// <summary>
        /// Registers the navigation item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void RegisterNavigationItem(NavigationItem navItem)
        {
            if (navItem == null)
            {
                throw new ArgumentNullException("navItem");
            }

            if (!_registeredNavigationItems.Contains(navItem))
            {
                if (!(navItem is NavigationMenuItem))
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                    {
                        MainRegion.Add(navItem.Content, navItem.Name);
                    });

                _registeredNavigationItems.Add(navItem);
            }
        }

        /// <summary>
        /// Shows the navigation menu.
        /// </summary>
        public void ShowNavigationMenu()
        {
            var menuRegion = _regionManager.Regions.FirstOrDefault(x => x.Name == RegionNames.MenuRegion);

            foreach (var menuItem in _registeredNavigationItems.OfType<NavigationMenuItem>().OrderBy(x => x.Order))
            {
                var needAddView = menuRegion == null;
                if (menuRegion != null)
                {
                    needAddView = !menuRegion.Views.Contains(menuItem);
                }
                if (needAddView)
                {
                    _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => menuItem);
                }
            }

        }

        public void NavigateByName(string name)
        {
            var navigationData = GetNavigationItemByName(name);
            if (navigationData != null)
            {
                Navigate(navigationData);
            }
        }

        /// <summary>
        /// Switches the view.
        /// </summary>
        /// <param name="navItem">The navigation item.</param>
        public void Navigate(NavigationItem navItem)
        {
            if (navItem == null)
            {
                throw new ArgumentNullException("navItem");
            }
			
            var content = navItem.Content;
			
            //check that navigation item already registered
            var alreadyExistNavItem = GetNavigationItemByName(navItem.Name) != null;

            if (!alreadyExistNavItem)
            {
                //register navigation item
                RegisterNavigationItem(navItem);

                //Update currently opened counter
                //var menuNavItem = GetNavigationItemByName(navItem.MenuNavigationItemName) as NavigationMenuItem;
                //if (menuNavItem != null)
                //{
                //    menuNavItem.OpenViewCount++;
                //}

                //register composite commands
                if (content is ISupportAcceptChanges)
                {
                    _globalCommandsProxy.SaveChangesAllCommand.RegisterCommand(((ISupportAcceptChanges)content).SaveChangesCommand);
                    _globalCommandsProxy.CancelAllCommand.RegisterCommand(((ISupportAcceptChanges)content).CancelCommand);
                }

                if (content is IClosable)
                {
                    //Cleanup actions
                    ((IClosable)content).CloseViewRequestedEvent -= RaiseCloseViewRequestedEvent;
                    ((IClosable)content).CloseViewRequestedEvent += RaiseCloseViewRequestedEvent;
                }

                //Handle minimize request
                if (content is IMinimizable)
                {
                    ((IMinimizable)content).MinimizableViewRequestedEvent += (sender, args) =>
                    {
                        //Show home view
                        var homeViewNavigationItem = GetNavigationItemByName(navItem.HomeNavigationItemName);
                        Navigate(homeViewNavigationItem);
                    };
                }
            }

            //Selection in open  tracking region
            if (content is IOpenTracking)
            {
                var openTracking = content as IOpenTracking;
                openTracking.IsActive = true;
            }
			
            //Initialize before open
            if (content is ISupportDelayInitialization)
            {
	            var registration = backgroundWorkers.SingleOrDefault(t => t.Item1.Name == navItem.Name);
				if (registration==null)
	            {
		            using (var backgroundWorker = new BackgroundWorker())
		            {
			            var newRegistration = new Tuple<NavigationItem, BackgroundWorker>(navItem, backgroundWorker);
						backgroundWorkers.Add(newRegistration);
			            backgroundWorker.DoWork += (sender, args) =>
			            {
				            args.Result = args.Argument;
				            var delayedInitialization = (ISupportDelayInitialization) args.Argument;
				            delayedInitialization.IsInitializing = true;
				            delayedInitialization.InitializeForOpen();
			            };

			            backgroundWorker.RunWorkerCompleted += (sender, args) =>
			            {
							backgroundWorkers.Remove(newRegistration);
				            ((ISupportDelayInitialization) args.Result).IsInitializing = false;
				            
			            };
			            backgroundWorker.RunWorkerAsync(content);
		            }
	            }
            }


			if (content is ViewModelBase)
			{
				((ViewModelBase)content).InitializeGestures();
			}


            //Show view
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) delegate
                {

                    var mainView = MainRegion.GetView(navItem.Name);
                    if (mainView != null)
                    {
                        MainRegion.Activate(mainView);
                        _currentNavigationItemName = navItem.Name;
                    }
                });
        }

		List<Tuple<NavigationItem, BackgroundWorker>> backgroundWorkers = new List<Tuple<NavigationItem,BackgroundWorker>>();

        public object GetViewFromRegion(NavigationItem navItem)
        {
            var mainView = MainRegion.GetView(navItem.Name);
            return mainView;
        }

        /// <summary>
        /// Navigates to default page.
        /// </summary>
        public void NavigateToDefaultPage()
        {
            var navMenuItem = _registeredNavigationItems.OfType<NavigationMenuItem>().OrderBy(x => x.Order).FirstOrDefault();
            if (navMenuItem != null)
            {
                navMenuItem.NavigateCommand.Execute(null);
            }
        }

        /// <summary>
        /// register composite commands
        /// </summary>
        /// <param name="viewModel">must implement ISupportAcceptChanges to register</param>
        public void RegisterCompositeCommand(IViewModel viewModel)
        {
            var acceptChanges = viewModel as ISupportAcceptChanges;
            if (acceptChanges != null)
            {
                _globalCommandsProxy.SaveChangesAllCommand.RegisterCommand(acceptChanges.SaveChangesCommand);
                _globalCommandsProxy.CancelAllCommand.RegisterCommand(acceptChanges.CancelCommand);
            }
        }

        #endregion

        #region private members

        private void RaiseCloseViewRequestedEvent(object sender, EventArgs args)
        {
            var closable = (IClosable) sender;
            if (closable != null)
            {
                closable.CloseViewRequestedEvent -= RaiseCloseViewRequestedEvent;
                var navItem = closable.NavigationData;

                //var menuNavItem = GetNavigationItemByName(navItem.MenuNavigationItemName) as NavigationMenuItem;
                //if (menuNavItem != null)
                //{
                //    menuNavItem.OpenViewCount = Math.Max(0, menuNavItem.OpenViewCount - 1);
                //}


                //unregister navigation item in navigation manager
                UnRegisterNavigationItem(navItem);

                //unregister composite commands
                if (sender is ISupportAcceptChanges)
                {
                    _globalCommandsProxy.SaveChangesAllCommand.UnregisterCommand(
                        ((ISupportAcceptChanges) sender).SaveChangesCommand);
                    _globalCommandsProxy.CancelAllCommand.UnregisterCommand(
                        ((ISupportAcceptChanges) sender).CancelCommand);
                }


                //Show home view
                var homeViewNavigationItem = GetNavigationItemByName(navItem.HomeNavigationItemName);
                if (homeViewNavigationItem != null)
                {
                    Navigate(homeViewNavigationItem);
                }
            }

        }

        #endregion

    }
}
