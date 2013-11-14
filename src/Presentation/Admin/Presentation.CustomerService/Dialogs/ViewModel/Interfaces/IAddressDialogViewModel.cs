using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
	public interface IAddressDialogViewModel: IViewModel
	{
		bool Validate();
		Address InnerItem { get; }
	}
}
