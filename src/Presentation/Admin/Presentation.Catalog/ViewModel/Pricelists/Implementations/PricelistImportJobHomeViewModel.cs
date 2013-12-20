using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Wizard;
using VirtoCommerce.Foundation.Importing.Services;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
    public class PricelistImportJobHomeViewModel : ImportJobHomeViewModel, IPricelistImportJobHomeViewModel
	{
		#region Constructor
		public PricelistImportJobHomeViewModel(
			IRepositoryFactory<IImportRepository> importRepository,
			IViewModelsFactory<ICreateImportJobViewModel> wizardVmFactory,
			IViewModelsFactory<IImportJobRunViewModel> runVmFactory,
			IViewModelsFactory<IImportJobViewModel> itemVmFactory,
			IImportJobEntityFactory entityFactory,
			IAuthenticationContext authContext,
			IImportService importService,
			SubTabsDefaultViewModel parentViewModel)
			:base(importRepository, wizardVmFactory, runVmFactory, itemVmFactory, entityFactory, authContext, importService, parentViewModel)
		{
			AvailableImporters = new[]
				{
					ImportEntityType.Price
				};
			DefaultImporter = AvailableImporters.First().ToString();
		}
		#endregion
		

    }
}
