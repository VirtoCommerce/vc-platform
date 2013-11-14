using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
    public interface ICustomerChoiceDialogViewModel: IViewModel
    {
        bool IsEmpty { get; }
        string SearchCustomerKeyword { get; }
        long TotalSearched { get; }
        Contact CurrentContact { get; }
        ObservableCollection<Contact> ContactList { get; }
        bool IsValid { get; }
        bool IsNew { get; }
        DelegateCommand SearchCustomerCommand { get; }
        DelegateCommand CreateNewCustomerCommand { get; }
    }
}
