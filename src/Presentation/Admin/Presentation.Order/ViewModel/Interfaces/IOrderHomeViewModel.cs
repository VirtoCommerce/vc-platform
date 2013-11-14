using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface IOrderHomeViewModel : IViewModel
    {
        DelegateCommand<string> SearchOrdersCommand { get; }
        ICollectionView OrderItemsSource { get; }
    }
}
