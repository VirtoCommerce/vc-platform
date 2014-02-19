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
using VirtoCommerce.Foundation.Importing.Services;

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
			IImportJobEntityFactory entityFactory,
			IAuthenticationContext authContext,
			IImportService importService,
			SubTabsDefaultViewModel parentViewModel)
			: base(importRepository, wizardVmFactory, runVmFactory, itemVmFactory, entityFactory, authContext, importService, parentViewModel)
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
					ImportEntityType.ItemAsset,
					ImportEntityType.Seo
				};
			DefaultImporter = AvailableImporters.First().ToString();
		}
		#endregion

		

	}
}
