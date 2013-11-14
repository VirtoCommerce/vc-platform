using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
	public interface IReceiveInventoryViewModel: IViewModel
	{
		ObservableCollection<Foundation.Inventories.Model.Inventory> InventoryItems { get; }
		FulfillmentCenter SelectedFulfillmentCenter { get; set; }
	}
}
