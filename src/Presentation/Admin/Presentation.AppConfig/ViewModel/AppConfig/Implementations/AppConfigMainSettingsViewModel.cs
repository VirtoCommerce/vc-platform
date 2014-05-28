using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
    public class AppConfigMainSettingsViewModel : SubTabsDefaultViewModel, IAppConfigMainSettingsViewModel
    {
        public AppConfigMainSettingsViewModel(IAppConfigSettingsViewModel appConfigSetting, ISystemJobsViewModel systemJobSetting, IEmailTemplatesViewModel emailTemplates, IDisplayTemplatesViewModel displayTemplates, ILocalizationMainViewModel localizationHome, ICacheViewModel cacheViewModel, IAuthenticationContext authContext)
        {
            var items = new List<ItemTypeHomeTab>();

            if (authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSettings))
            {
                items.Add(new ItemTypeHomeTab { Caption = "Settings", Category = NavigationNames.ModuleName, ViewModel = appConfigSetting });
            }
            if (authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSystemJobs))
            {
                items.Add(new ItemTypeHomeTab { Caption = "System jobs", Category = NavigationNames.ModuleName, ViewModel = systemJobSetting });
            }
            if (authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigEmailTemplates))
            {
                items.Add(new ItemTypeHomeTab { Caption = "Email templates", Category = NavigationNames.ModuleName, ViewModel = emailTemplates });
            }
            if (authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigDisplayTemplates))
            {
                items.Add(new ItemTypeHomeTab { Caption = "Display templates", Category = NavigationNames.ModuleName, ViewModel = displayTemplates });
            }
            items.Add(new ItemTypeHomeTab { Caption = "Localization", Category = NavigationNames.ModuleName, ViewModel = localizationHome });

            if (authContext.CheckPermission(PredefinedPermissions.SettingsAppConfigSettings))
            {
                items.Add(new ItemTypeHomeTab { Caption = "Cache", Category = NavigationNames.ModuleName, ViewModel = cacheViewModel });
            }

            SubItems = items;
            if (SubItems.Count > 0)
            {
                CurrentTab = SubItems[0];
            }
        }
    }
}
