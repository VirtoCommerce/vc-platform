using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
    public class CustomersMainViewModel : SubTabsDefaultViewModel, ICustomersMainViewModel
    {
        public CustomersMainViewModel(ICustomersHomeViewModel customersHomeViewModel, ISearchHomeViewModel searchHomeViewModel, IAuthenticationContext authContext)
        {
            ViewTitle = new ViewTitleBase() { Title = "Cases", SubTitle = "Customer Service".Localize() };
            SubItems = new List<ItemTypeHomeTab>
	            {
		            new ItemTypeHomeTab
			            {
				            IdTab = NavigationNames.HomeName,
				            Caption = "Cases", Category = NavigationNames.ModuleName,
				            ViewModel = customersHomeViewModel
			            }
	            };

            if (authContext.CheckPermission(PredefinedPermissions.CustomersSearchCases))
            {
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNameSearch, Caption = "Search", Category = NavigationNames.ModuleName, ViewModel = searchHomeViewModel });
            }
            CurrentTab = SubItems[0];
        }

        private void Testy()
        {
            (this.CurrentTab.ViewModel as CustomersHomeViewModel).ShowCustomerChoiceDialogRequest.Raise(null);
        }


    }
}
