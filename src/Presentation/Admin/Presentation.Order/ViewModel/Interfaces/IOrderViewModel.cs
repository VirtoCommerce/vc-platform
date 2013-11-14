using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface IOrderViewModel : IViewModel
    {
        InteractionRequest<Confirmation> CommonOrderCommandConfirmRequest { get; }
        InteractionRequest<Confirmation> CommonOrderWizardDialogInteractionRequest { get; }

        DelegateCommand CancelOrderCommand { get; }
        DelegateCommand HoldOrderCommand { get; }
        DelegateCommand ReleaseHoldCommand { get; }
        DelegateCommand CreateRmaRequestCommand { get; }
        DelegateCommand CreateExchangeCommand { get; }

        ObservableCollection<ShippingMethod> AvailableShippingMethods { get; }

        ObservableCollection<IShipmentViewModel> OrderShipmentViewModels { get; }
        void InitializeOrderShipmentViewModels();

        ObservableCollection<IRmaRequestViewModel> RmaRequestViewModels { get; }
    }
}
