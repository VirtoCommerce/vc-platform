using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CatalogImportJobHomeViewModel : ImportJobHomeViewModel, ICatalogImportJobHomeViewModel
	{
		
		#region Constructor
		public CatalogImportJobHomeViewModel(
			IRepositoryFactory<IImportRepository> importRepository,
			IViewModelsFactory<ICreateImportJobViewModel> wizardVmFactory,
			IViewModelsFactory<IImportJobRunViewModel> runVmFactory,
			IViewModelsFactory<IImportJobViewModel> itemVmFactory,
			IViewModelsFactory<IImportJobProgressViewModel> progressVmFactory,
			IImportJobEntityFactory entityFactory,
			IAuthenticationContext authContext, 
			SubTabsDefaultViewModel parentViewModel)
			: base(importRepository, wizardVmFactory, runVmFactory, itemVmFactory, progressVmFactory, entityFactory, authContext, parentViewModel)
		{
			AvailableImporters = new[]
				{
					ImportEntityType.Product,
					ImportEntityType.Bundle,
					ImportEntityType.Category,
					ImportEntityType.Association,
					ImportEntityType.DynamicKit,
					ImportEntityType.Package,
					ImportEntityType.Sku,
					ImportEntityType.ItemRelation,
					ImportEntityType.ItemAsset
				};
			DefaultImporter = AvailableImporters.First().ToString();
		}
		#endregion

		

	}
}
