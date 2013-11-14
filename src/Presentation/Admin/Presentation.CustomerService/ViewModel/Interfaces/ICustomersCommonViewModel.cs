using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces
{
	public interface ICustomersCommonViewModel
	{
		DelegateCommand RefreshCommand { get; }
		ICollectionView ListItemsSource { get; }

		DelegateCommand<Contact> CreateNewEmailCaseCommand { get; }
		DelegateCommand<Contact> DeleteCustomerCommand { get; }
		DelegateCommand<string> DeleteCaseCommand { get; }

		void Refresh();
	}
}
