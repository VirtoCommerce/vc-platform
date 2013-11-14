using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces
{
	public interface IInventoryHomeViewModel : IViewModel
	{
		ICollectionView SelectedInventoryItems { get; }
		IViewModel SelectedInventoryItem { get; set; }
		/// <summary>
		/// For complete shipment command purposes
		/// </summary>
		IMainFulfillmentViewModel ParentViewModel { get; set; }
	}
}
