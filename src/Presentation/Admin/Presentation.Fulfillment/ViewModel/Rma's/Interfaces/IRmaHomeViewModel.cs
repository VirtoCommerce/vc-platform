using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces
{
	public interface IRmaHomeViewModel : IViewModel
	{
		//ICollectionView SourceItems { get; }

		/// <summary>
		/// For complete shipment command purposes
		/// </summary>
		IMainFulfillmentViewModel ParentViewModel { get; set; }
	}
}
