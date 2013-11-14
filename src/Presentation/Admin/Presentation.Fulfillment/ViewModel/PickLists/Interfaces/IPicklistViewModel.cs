using VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces
{
	public interface IPicklistViewModel: IViewModel
	{
		Picklist Picklist { get; }
	}
}
