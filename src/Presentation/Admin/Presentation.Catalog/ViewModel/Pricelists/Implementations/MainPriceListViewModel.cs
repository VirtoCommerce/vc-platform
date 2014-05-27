using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{

	public class MainPriceListViewModel : SubTabsDefaultViewModel, IMainPriceListViewModel
	{
		public MainPriceListViewModel(IPriceListHomeViewModel priceListHomeViewModel, IPriceListAssignmentHomeViewModel priceListAssignmentHomeViewModel, IViewModelsFactory<IPricelistImportJobHomeViewModel> pricelistImportVmFactory, IAuthenticationContext authContext)
		{
            ViewTitle = new ViewTitleBase()
            {
                Title = "Price Lists",
                SubTitle = "MERCHANDISE MANAGEMENT".Localize()
            };

			SubItems = new List<ItemTypeHomeTab>();
			if (authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNamePriceList, Caption = "Price Lists", Category = NavigationNames.ModuleName, ViewModel = priceListHomeViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNamePriceListAssignment, Caption = "Price List assignments", Category = NavigationNames.ModuleName, ViewModel = priceListAssignmentHomeViewModel });
			}
			if (authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsImport_Jobs) ||
				authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsImport_JobsRun))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeNamePriceList, Caption = "Import", Category = NavigationNames.ModuleName, ViewModel = pricelistImportVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("parentViewModel", this)) });
			}
			CurrentTab = SubItems[0];
		}
	}
}
