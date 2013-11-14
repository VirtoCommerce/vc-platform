using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces
{
	public interface IPromotionViewModel : IViewModel, IExpressionViewModel
    {
        Promotion InnerItem { get; }
		string CatalogId { get; }
		InteractionRequest<Confirmation> CommonConfirmRequest { get; }

		//needed for expressions
		IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory { get; }
		IViewModelsFactory<ISearchItemViewModel> SearchItemVmFactory { get; }
		IRepositoryFactory<IShippingRepository> ShippingRepositoryFactory { get; }
		IRepositoryFactory<IAppConfigRepository> AppConfigRepositoryFactory { get; }
    }

    public interface IPromotionViewModelBase : IPromotionViewModel
    {
    }
}
