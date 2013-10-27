using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings
{
    public class CustomersMainSettingsViewModel : SubTabsDefaultViewModel, ICustomersMainSettingsViewModel
    {
		#region Constructor

		public CustomersMainSettingsViewModel(
			ICaseRulesSettingsViewModel caseRulesSettingsViewModel, 
			ICasePropertySetsSettingsViewModel casePropertySetsSettingsViewModel, 
			ICaseTemplatesSettingsViewModel caseTemplatesSettingsViewModel, 
			IAuthenticationContext authContext)
		{
			SubItems = new List<ItemTypeHomeTab>();

			if (authContext.CheckPermission(PredefinedPermissions.SettingsCustomerRules))
            {
				SubItems.Add(new ItemTypeHomeTab { Caption = "Rules", ViewModel = caseRulesSettingsViewModel, Order = 1 });
            }
			if (authContext.CheckPermission(PredefinedPermissions.SettingsCustomerInfo))
            {
				SubItems.Add(new ItemTypeHomeTab { Caption = "Info", ViewModel = casePropertySetsSettingsViewModel, Order = 2 });
            }
			if (authContext.CheckPermission(PredefinedPermissions.SettingsCustomerCaseTypes))
            {
				SubItems.Add(new ItemTypeHomeTab { Caption = "Case types", ViewModel = caseTemplatesSettingsViewModel, Order = 3 });
            }
			CurrentTab = SubItems[0];
		}

        #endregion
    }
}
