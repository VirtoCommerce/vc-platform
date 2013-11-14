using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Reviews.Model;

namespace VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces
{
    public interface IReviewEditViewModel : IViewModel
    {
        ReviewBase InnerItem { get; }
    }
}
