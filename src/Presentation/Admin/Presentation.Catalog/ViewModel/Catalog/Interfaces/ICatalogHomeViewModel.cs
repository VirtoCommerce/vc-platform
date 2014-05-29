using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.Collections;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface ICatalogHomeViewModel : IViewModel
    {
        InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }
        InteractionRequest<Confirmation> CommonConfirmRequest { get; }
        InteractionRequest<Notification> CommonNotifyRequest { get; }

        DelegateCommand CreateCatalogCommand { get; }
        DelegateCommand CreateLinkedCategoryCommand { get; }
        DelegateCommand CreateCategoryCommand { get; }
        
        DelegateCommand<IViewModel> OpenCatalogEntityCommand { get; }
        DelegateCommand<IViewModel> TreeItemDeleteCommand { get; }
        DelegateCommand<IList> ItemMoveCommand { get; }
        DelegateCommand<IList> ItemDuplicateCommand { get; }
        DelegateCommand<IList> ItemDeleteCommand { get; }

        DelegateCommand QueryCreateCommand { get; }
        DelegateCommand<ItemFilter> QueryRunCommand { get; }
        DelegateCommand<ItemFilter> QueryEditCommand { get; }
        DelegateCommand<ItemFilter> QueryDeleteCommand { get; }

        DelegateCommand SearchAllItemsCommand { get; }
        DelegateCommand ClearFiltersCommand { get; }
        DelegateCommand SearchItemsCommand { get; }
        DelegateCommand RefreshTreeItemsCommand { get; }

        ObservableCollection<IViewModel> RootCatalogs { get; }
        ICollectionView ListItemsSource { get; }

        IViewModel SelectedCatalogItem { get; }

        ObservableCollection<CatalogBase> SearchFilterCatalogs { get; }
        ObservableCollection<ItemFilter> AllQueries { get; }

        string SearchFilterAll { get;  }

        string SearchFilterName { get;  }
        string SearchFilterCode { get;  }
        string SearchFilterItemType { get;  }
    }
}
