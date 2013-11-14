using VirtoCommerce.ManagementClient.Core.Infrastructure;


namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces
{
	public interface IInventoryViewModel: IViewModel
	{
		Foundation.Inventories.Model.Inventory InnerItem { get; set; }
	}
}
