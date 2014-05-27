using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CatalogMainViewModel : SubTabsDefaultViewModel, ICatalogMainViewModel
	{
		public CatalogMainViewModel(
			IViewModelsFactory<ICatalogHomeViewModel> catalogHomeVmFactory,
			IViewModelsFactory<ICatalogImportJobHomeViewModel> importVmFactory,
			IReviewsHomeViewModel reviewsHomeViewModel,
			IAuthenticationContext authContext)
		{
			SubItems = new List<ItemTypeHomeTab>();
			var parameters = new KeyValuePair<string, object>("parentViewModel", this);

            SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Catalog", Category = NavigationNames.ModuleName, ViewModel = catalogHomeVmFactory.GetViewModelInstance(parameters) });
			if (authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsManage) ||
				authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsRun))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Import", Category = NavigationNames.ModuleName, ViewModel = importVmFactory.GetViewModelInstance(parameters) });
			}
			if (authContext.CheckPermission(PredefinedPermissions.CatalogCustomerReviewsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNameReviews, Caption = "Reviews", Category = NavigationNames.ModuleName, ViewModel = reviewsHomeViewModel });
			}
			CurrentTab = SubItems[0];
		}
	}
}
