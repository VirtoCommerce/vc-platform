using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations
{
	public class MainMarketingViewModel : SubTabsDefaultViewModel, IMainMarketingViewModel
	{
		public MainMarketingViewModel(IMarketingHomeViewModel marketingVm, IDynamicContentHomeViewModel contentVm, IContentPublishingHomeViewModel publishingVm, IAuthenticationContext authContext)
		{
			ViewTitle = new ViewTitleBase
			{
                Title = "Marketing",
				SubTitle = "MARKETING SERVICE".Localize()
			};

			SubItems = new List<ItemTypeHomeTab>();

			if (authContext.CheckPermission(PredefinedPermissions.MarketingPromotionsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Promotions", Category = NavigationNames.ModuleName, ViewModel = marketingVm });
			}
			if (authContext.CheckPermission(PredefinedPermissions.MarketingDynamic_ContentManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNameDynamicContent, Caption = "Dynamic content", Category = NavigationNames.ModuleName, Order = 10, ViewModel = contentVm });
			}
			if (authContext.CheckPermission(PredefinedPermissions.MarketingContent_PublishingManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNameContentPublishing, Caption = "Content publishing", Category = NavigationNames.ModuleName, Order = 20, ViewModel = publishingVm });
			}
			CurrentTab = SubItems[0];
		}
	}
}
