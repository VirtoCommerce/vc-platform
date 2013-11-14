using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces
{
    public interface IMainFulfillmentViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
		DelegateCommand CompleteShipmentCommand { get; }
		InteractionRequest<Confirmation> CommonConfirmRequest { get; }
		InteractionRequest<Notification> CommonNotifyRequest { get; }
    }
}
