using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
	public interface ICompleteShipmentViewModel: IViewModel
	{
		string ShipmentId { get; }
		string TrackingNumber { get; }
	}
}
