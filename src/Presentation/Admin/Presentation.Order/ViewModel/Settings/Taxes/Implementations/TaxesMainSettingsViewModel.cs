using System.Collections.Generic;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations
{
	public class TaxesMainSettingsViewModel : SubTabsDefaultViewModel, ITaxesMainSettingsViewModel
	{
		public TaxesMainSettingsViewModel(
			ITaxCategorySettingsViewModel taxCategorySettingsViewModel,
			ITaxSettingsViewModel taxSettingsViewModel,
			IViewModelsFactory<IJurisdictionSettingsViewModel> jSettingsVmFactory,
			IViewModelsFactory<IJurisdictionGroupSettingsViewModel> jGroupVmFactory,
			IViewModelsFactory<ITaxImportHomeViewModel> importVmFactory,
			IAuthenticationContext authContext)
		{
			SubItems = new List<ItemTypeHomeTab>();

			if (authContext.CheckPermission(PredefinedPermissions.SettingsTaxCategories))
			{
                SubItems.Add(new ItemTypeHomeTab { Caption = "Tax categories", Category = NavigationNames.ModuleName, ViewModel = taxCategorySettingsViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsJurisdiction))
			{
                SubItems.Add(new ItemTypeHomeTab { Caption = "Jurisdictions", Category = NavigationNames.ModuleName, ViewModel = jSettingsVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("jurisdictionType", JurisdictionTypes.Taxes)) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsJurisdictionGroups))
			{
                SubItems.Add(new ItemTypeHomeTab { Caption = "Jurisdiction groups", Category = NavigationNames.ModuleName, ViewModel = jGroupVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("jurisdictionType", JurisdictionTypes.Taxes)) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsTaxes))
			{
                SubItems.Add(new ItemTypeHomeTab { Caption = "Taxes", Category = NavigationNames.ModuleName, ViewModel = taxSettingsViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsTaxImport))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = Configuration.NavigationNames.HomeName, Caption = "Import", Category = NavigationNames.ModuleName, ViewModel = importVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("parentViewModel", this)) });
			}
			CurrentTab = SubItems[0];
		}
	}
}
