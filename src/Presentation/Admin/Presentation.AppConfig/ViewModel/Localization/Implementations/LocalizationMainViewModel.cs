using System.Collections.Generic;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations
{

	public class LocalizationMainViewModel : SubTabsDefaultViewModel, ILocalizationMainViewModel
	{
		public LocalizationMainViewModel(ILocalizationHomeViewModel homeViewModel, IViewModelsFactory<ILocalizationImportJobHomeViewModel> importVmFactory, IAuthenticationContext authContext)
		{
            ViewTitle = new ViewTitleBase() { Title = "Localization", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
			SubItems = new List<ItemTypeHomeTab>();

			if (authContext.CheckPermission(PredefinedPermissions.Name_SettingsAppConfigSettings))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Localizations", Category = NavigationNames.ModuleName, ViewModel = homeViewModel });
			}

			if (authContext.CheckPermission(PredefinedPermissions.Name_SettingsAppConfigSettings))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = Configuration.NavigationNames.HomeName, Caption = "Import", Category = NavigationNames.ModuleName, ViewModel = importVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("parentViewModel", this)) });
			}

			if (SubItems.Count > 0)
				CurrentTab = SubItems[0];
		}
	}
}
