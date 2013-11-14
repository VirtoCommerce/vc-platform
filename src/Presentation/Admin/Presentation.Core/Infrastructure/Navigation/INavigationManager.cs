using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation
{
    public interface INavigationManager
    {
        /// <summary>
        /// Navigate to Parent. If parent is unknown then to default 
        /// Current doesn't close
        /// </summary>
        void Back();

        /// <summary>
        /// Gets the name of the navigation item by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        NavigationItem GetNavigationItemByName(string name);

        /// <summary>
        /// Uns the register navigation item.
        /// </summary>
        /// <param name="navItem">The nav item.</param>
        void UnRegisterNavigationItem(NavigationItem navItem);

        /// <summary>
        /// Registers the navigation item.
        /// </summary>
        /// <param name="item">The item.</param>
        void RegisterNavigationItem(NavigationItem navItem);

        /// <summary>
        /// Shows the navigation menu.
        /// </summary>
        void ShowNavigationMenu();

        void NavigateByName(string name);

        /// <summary>
        /// Switches the view.
        /// </summary>
        /// <param name="navItem">The navigation item.</param>
        void Navigate(NavigationItem navItem);

        object GetViewFromRegion(NavigationItem navItem);

        /// <summary>
        /// Navigates to default page.
        /// </summary>
        void NavigateToDefaultPage();

        /// <summary>
        /// register composite commands
        /// </summary>
        /// <param name="viewModel">must implement ISupportAcceptChanges to register</param>
        void RegisterCompositeCommand(IViewModel viewModel);
    }
}
