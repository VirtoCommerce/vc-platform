using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces
{
    public interface IReviewsHomeViewModel : IViewModel
    {
        DelegateCommand<string> SearchItemsCommand { get; }
        ICollectionView ItemsSource { get; }
    }
}
