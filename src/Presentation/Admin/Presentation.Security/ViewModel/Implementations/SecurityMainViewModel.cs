using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Security.Properties;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
    public class SecurityMainViewModel : SubTabsDefaultViewModel, ISecurityMainViewModel
    {
		public SecurityMainViewModel(IViewModelsFactory<IAccountHomeViewModel> accountVmFactory, IViewModelsFactory<IRoleHomeViewModel> roleVmFactory, IAuthenticationContext authContext)
		{
            ViewTitle = new ViewTitleBase
            {
                Title = Resources.Users,
                SubTitle = Resources.USER_MANAGEMENT
            };

			SubItems = new List<ItemTypeHomeTab>();
			var parameters = new KeyValuePair<string, object>("parentViewModel", this);

			if (authContext.CheckPermission(PredefinedPermissions.SecurityAccounts))
			{
				SubItems.Add(new ItemTypeHomeTab { Caption = Resources.Accounts, ViewModel = accountVmFactory.GetViewModelInstance(parameters) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SecurityRoles))
			{
				SubItems.Add(new ItemTypeHomeTab { Caption = "Roles", ViewModel = roleVmFactory.GetViewModelInstance(parameters) });
			}
			CurrentTab = SubItems[0];
		}
    }
}
