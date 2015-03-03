using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace CommerceFoundation.UI.FunctionalTests.TestHelpers
{
    public class TestNavigationManager:INavigationManager
    {
        public void Back()
        {
            
        }

        public NavigationItem GetNavigationItemByName(string name)
        {
            throw new NotImplementedException();
        }

        public void UnRegisterNavigationItem(NavigationItem navItem)
        {
            
        }

        public void RegisterNavigationItem(NavigationItem navItem)
        {
            
        }

        public void ShowNavigationMenu()
        {
            
        }

        public void NavigateByName(string name)
        {
           
        }

        public void Navigate(NavigationItem navItem)
        {
           
        }

        public object GetViewFromRegion(NavigationItem navItem)
        {
            throw new NotImplementedException();
        }

        public void NavigateToDefaultPage()
        {
           
        }

        public void RegisterCompositeCommand(VirtoCommerce.ManagementClient.Core.Infrastructure.IViewModel viewModel)
        {
           
        }
    }
}
