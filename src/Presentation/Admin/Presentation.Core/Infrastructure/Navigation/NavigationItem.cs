
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation
{
    /// <summary>
    /// Represent data structure for navigation system
    /// </summary>
    public class NavigationItem : ViewModelBase, IViewModel
    {
        public NavigationItem(string name, object content)
            : this(name, null, null, content)
        {
        }

        public NavigationItem(string name, string homeNavigationName, string menuNavigationName, object content)
        {
            Content = content;
            Name = name;
            HomeNavigationItemName = homeNavigationName;
            MenuNavigationItemName = menuNavigationName;
        }

        /// <summary>
        /// Navigation item key
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// name for Home navigation item
        /// </summary>
        /// <value>
        /// The name of the home region.
        /// </value>
        public string HomeNavigationItemName { get; set; }

        /// <summary>
        /// name for Menu navigation item
        /// </summary>
        /// <value>
        /// The name of the menu navigation item.
        /// </value>
        public string MenuNavigationItemName { get; set; }

        public object Content { get; set; }

    }
}
