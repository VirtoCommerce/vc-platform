using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation
{
    public class NavigationMenuItem : NavigationItem
    {

        public NavigationMenuItem(string name)
            :this(name, null)
        {
        }

        public NavigationMenuItem(string name, object content)
            :base(name, content)
        {
        }

        public DelegateCommand<NavigationItem> NavigateCommand
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// Localization category for the Caption
        /// </summary>
        public string Category { get; set; }

        public string ImageResourceKey
        {
            get;
            set;
        }

        public Color ItemBackground
        {
            get;
            set;
        }
    }
}
