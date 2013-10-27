using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
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
				SubItems.Add(new ItemTypeHomeTab { Caption = "Tax categories", ViewModel = taxCategorySettingsViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsJurisdiction))
			{
				SubItems.Add(new ItemTypeHomeTab { Caption = "Jurisdictions", ViewModel = jSettingsVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("jurisdictionType", JurisdictionTypes.Taxes)) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsJurisdictionGroups))
			{
				SubItems.Add(new ItemTypeHomeTab { Caption = "Jurisdiction groups", ViewModel = jGroupVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("jurisdictionType", JurisdictionTypes.Taxes)) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsTaxes))
			{
				SubItems.Add(new ItemTypeHomeTab { Caption = "Taxes", ViewModel = taxSettingsViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.SettingsTaxImport))
			{
				SubItems.Add(new ItemTypeHomeTab { IdTab = Configuration.NavigationNames.HomeName, Caption = "Import", ViewModel = importVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("parentViewModel", this)) });
			}
			CurrentTab = SubItems[0];
		}
	}
}
