using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
	public interface IShipmentViewModel : IViewModel
	{
		InteractionRequest<Confirmation> CommonShipmentConfirmRequest { get; }

		DelegateCommand ReleaseShipmentCommand { get; }
		DelegateCommand CompleteShipmentCommand { get; }

		DelegateCommand CancelShipmentCommand { get; }

		DelegateCommand AddLineItemCommand { get; }
		DelegateCommand<ShipmentItem> MoveLineItemCommand { get; }

		Shipment CurrentShipment { get; set; }
		void PropertyChanged_EventsAdd();
		void PropertyChanged_EventsRemove();
		void RaiseCanExecuteChanged();
	}
}
