using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces
{
	public interface ICustomersDetailViewModel : IViewModel
	{
		Case InnerItem { get; }
		Contact CurrentCustomer { get; }
		CustomerDetailViewModel CustomerDetailViewModel { get; }

		void RiseOpenItemCommand();
	}
}
