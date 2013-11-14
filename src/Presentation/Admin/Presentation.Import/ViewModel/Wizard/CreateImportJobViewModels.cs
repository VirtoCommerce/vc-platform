using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.ManagementClient.Import.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Wizard
{
	public class CreateImportJobViewModel : WizardContainerStepsViewModel, ICreateImportJobViewModel
	{
		public CreateImportJobViewModel(
			IViewModelsFactory<IImportJobOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<IImportJobMappingStepViewModel> mappingVmFactory,
			ImportJob item, 
			ImportEntityType[] entityImporters)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			var parentVM = new KeyValuePair<string, object>("parentVM", this);
			var _entityImporters = new KeyValuePair<string, object>("entityImporters", entityImporters);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter, parentVM, _entityImporters));
			RegisterStep(mappingVmFactory.GetViewModelInstance(itemParameter, parentVM));
		}

	}

	/// <summary>
	/// overview step view model
	/// </summary>
	public class ImportJobOverviewStepViewModel : ImportJobViewModel, IImportJobOverviewStepViewModel
	{
		public ImportJobOverviewStepViewModel(IRepositoryFactory<IImportRepository> repositoryFactory, 
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, ImportJob item, WizardViewModelBare parentVM,
			IImportJobEntityFactory importFactory,
			IViewModelsFactory<IPickAssetViewModel> assetVmFactory,
			IViewModelsFactory<IColumnMappingViewModel> mappingVmFactory,
			IImportService importService,
			ImportEntityType[] entityImporters,
			IAuthenticationContext authContext)
			: base(repositoryFactory, catalogRepositoryFactory, importFactory, item, parentVM, assetVmFactory, mappingVmFactory, importService, entityImporters, authContext)
		{
		}
		
		public override string[] CsvFileColumns
		{
			get
			{
				return base.CsvFileColumns;
			}
			set
			{
				if (_parentViewModel != null && _parentViewModel.AllRegisteredSteps.Count > 1)
				{
					((ImportJobViewModel)_parentViewModel.AllRegisteredSteps[1]).CsvFileColumns = value;
				}
				base.CsvFileColumns = value;
			}
		}
	}
	
	/// <summary>
	/// column mapping step view model
	/// </summary>
	public class ImportJobMappingStepViewModel : ImportJobViewModel, IImportJobMappingStepViewModel
	{
		public ImportJobMappingStepViewModel(IRepositoryFactory<IImportRepository> repositoryFactory, IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, 
			ImportJob item, WizardViewModelBare parentVM,
			IImportJobEntityFactory importFactory,
			IViewModelsFactory<IPickAssetViewModel> assetVmFactory,
			IViewModelsFactory<IColumnMappingViewModel> mappingVmFactory,
			IImportService importService,
			ImportEntityType[] entityImporters,
			IAuthenticationContext authContext)
			: base(repositoryFactory, catalogRepositoryFactory, importFactory, item, parentVM, assetVmFactory, mappingVmFactory, importService, entityImporters, authContext)
		{
		}
	}
}
