using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
	public interface IEditQuantityViewModel: IViewModel
	{
		string SelectedReason { get; }
		string SelectedAction { get; }
		int NewQuantity { get; }
	}
}
