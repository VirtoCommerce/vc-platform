using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces
{
	public interface IDisplayTemplateViewModel : IViewModel, IExpressionViewModel
    {
		DisplayTemplateMapping InnerItem { get; set; }
		InteractionRequest<Confirmation> CommonConfirmRequest { get; }
		IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory { get; }
		IViewModelsFactory<ISearchItemViewModel> SearchItemVmFactory { get; }
		IRepositoryFactory<IStoreRepository> StoreRepositoryFactory { get; }

		void UpdateFromExpressionElementBlock();
    }
}
