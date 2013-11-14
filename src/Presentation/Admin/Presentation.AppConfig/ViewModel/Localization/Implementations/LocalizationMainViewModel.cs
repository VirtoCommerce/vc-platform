using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations
{

	public class LocalizationMainViewModel : SubTabsDefaultViewModel, ILocalizationMainViewModel
	{
		public LocalizationMainViewModel(ILocalizationHomeViewModel homeViewModel, IViewModelsFactory<ILocalizationImportJobHomeViewModel> importVmFactory, IAuthenticationContext authContext)
		{
			ViewTitle = new ViewTitleBase() { Title = "Localization", SubTitle = "SETTINGS" };
			SubItems = new List<ItemTypeHomeTab>();

			if (authContext.CheckPermission(PredefinedPermissions.Name_SettingsAppConfigSettings))
			{
				SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Localizations", ViewModel = homeViewModel });
			}

			if (authContext.CheckPermission(PredefinedPermissions.Name_SettingsAppConfigSettings))
			{
				SubItems.Add(new ItemTypeHomeTab { IdTab = Configuration.NavigationNames.HomeName, Caption = "Import", ViewModel = importVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("parentViewModel", this)) });
			}
			CurrentTab = SubItems[0];
		}
	}
}
