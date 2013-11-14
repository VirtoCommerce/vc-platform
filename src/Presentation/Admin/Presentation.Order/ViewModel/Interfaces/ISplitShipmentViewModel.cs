using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface ISplitShipmentViewModel : IViewModel
    {
        Foundation.Orders.Model.ShipmentItem MovingShippingItem { get; }
        int MoveQuantity { get; set; }

        InteractionRequest<Confirmation> CreateNewAddressRequest { get; }

        ObservableCollection<Shipment> AvailableTargetShipments { get; }
        ObservableCollection<ShippingMethod> AvailableShippingMethods { get; }

        Foundation.Orders.Model.Shipment TargetShipment { get; set; }
        bool IsNewShipmentSelected { get; }
    }
}
