using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface ILineItemAddViewModel : IViewModel
    {
        InteractionRequest<Confirmation> ItemDetailsConfirmRequest { get; }

        ObservableCollection<Item> AvailableItems { get; }
        ObservableCollection<ILineItemViewModel> SelectedItemsToAdd { get; }
    }
}
