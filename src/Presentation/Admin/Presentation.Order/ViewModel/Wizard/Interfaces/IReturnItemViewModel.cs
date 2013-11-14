using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Builders;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
	public interface IReturnItemViewModel : IViewModel
	{
		ReturnBuilder.ReturnLineItem ReturnLineItem { get; set; }
		decimal QuantityToMove { get; set; }
		string SelectedReason { get; set; }
		string[] AvailableReasons { get; }

		bool IsBulkReturn { get; set; }
	}
}
