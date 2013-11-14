using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces
{
    public interface IShippingMethodSettingsViewModel : IViewModel
    {
        ObservableCollection<ShippingMethod> Items { get; }
        DelegateCommand ItemAddCommand { get; }
        DelegateCommand<ShippingMethod> ItemEditCommand { get; }
        DelegateCommand<ShippingMethod> ItemDeleteCommand { get; }
        
        void RaiseCanExecuteChanged();
    }
}
