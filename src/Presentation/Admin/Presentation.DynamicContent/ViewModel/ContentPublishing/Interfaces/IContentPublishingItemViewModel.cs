using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces
{
    public interface IContentPublishingItemViewModel : IViewModelDetailBase, IExpressionViewModel
    {
        DynamicContentPublishingGroup InnerItem { get; }
        InteractionRequest<Confirmation> CommonConfirmRequest { get; }

        ObservableCollection<DynamicContentItem> AllAvailableDynamicContent { get; }
        ObservableCollection<DynamicContentItem> InnerItemDynamicContent { get; }
        ObservableCollection<DynamicContentPlace> AllAvailableContentPlaces { get; }
        ObservableCollection<DynamicContentPlace> InnerItemContentPlaces { get; }

        IRepositoryFactory<IStoreRepository> StoreRepositoryFactory { get; }
        IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory { get; }
        IRepositoryFactory<ICountryRepository> CountryRepositoryFactory { get; }
        IRepositoryFactory<IAppConfigRepository> AppConfigRepositoryFactory { get; }
    }
}