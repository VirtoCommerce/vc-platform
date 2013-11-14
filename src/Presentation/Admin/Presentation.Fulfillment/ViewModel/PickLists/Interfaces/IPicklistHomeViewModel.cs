using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces
{
	public interface IPicklistHomeViewModel : IViewModel
	{
		//ICollectionView SelectedPicklistItems { get; }

		/// <summary>
		/// For complete shipment command purposes
		/// </summary>
		IMainFulfillmentViewModel ParentViewModel { get; set; }
	}
}
